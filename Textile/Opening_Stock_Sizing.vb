Public Class Opening_Stock_Sizing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "SOPEN-"
    Private OpYrCode As String = ""
    Private Prec_ActCtrl As New Control

    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl


    Private Sub clear()

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = ""
        txt_OpAmount.Text = "0.00"
        cbo_CrDrType.Text = "Cr"

        txt_EmptyBeam.Text = ""
        txt_EmptyBags.Text = ""
        txt_EmptyCones.Text = ""

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_PavuGrid_CountName.Visible = False
        cbo_Grid_WareHouse.Visible = False

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_Grid_WareHouse.Text = ""
        cbo_PavuGrid_CountName.Text = ""
        cbo_Grid_VendorName.Text = ""
        cbo_Grid_beamwidth.Text = ""

        dgv_YarnDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Clear()

        dgv_EmptyBeamDetails.Rows.Clear()

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        tab_Main.SelectTab(0)
        'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        'dgv_YarnDetails.CurrentCell.Selected = True
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
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
        If Me.ActiveControl.Name <> cbo_Grid_WareHouse.Name Then
            cbo_Grid_WareHouse.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_PavuGrid_CountName.Name Then
            cbo_PavuGrid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_beamwidth.Name Then
            cbo_Grid_beamwidth.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_VendorName.Name Then
            cbo_Grid_VendorName.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then dgv_EmptyBeamDetails.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Sno As Integer, n As Integer
        Dim NewCode As String

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

                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        txt_OpAmount.Text = Trim(Format(Math.Abs(Val(dt2.Rows(0).Item(0).ToString)), "#########0.00"))
                        If Val(txt_OpAmount.Text) = 0 Then
                            txt_OpAmount.Text = ""
                        End If
                        If Val(dt2.Rows(0).Item(0).ToString) >= 0 Then
                            cbo_CrDrType.Text = "Cr"
                        Else
                            cbo_CrDrType.Text = "Dr"
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.DeliveryTo_Idno , a.Empty_Beam as Op_Beam , b.Beam_Width_Name , c.Vendor_Name  from Stock_Empty_BeamBagCone_Processing_Details a  LEFT OUTER JOIN Beam_Width_Head b ON a.Beam_Width_IdNo <> 0 and a.Beam_Width_IdNo = b.Beam_Width_IdNo  LEFT OUTER JOIN Vendor_Head c ON a.Vendor_IdNo <> 0 and a.Vendor_IdNo = c.Vendor_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and ( a.DeliveryTo_Idno = " & Str(Val(idno)) & " or a.ReceivedFrom_Idno = " & Str(Val(idno)) & " ) and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Empty_Beam <> 0 Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_EmptyBeamDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_EmptyBeamDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_EmptyBeamDetails.Rows(n).Cells(0).Value = Val(Sno)
                        If IsDBNull(dt2.Rows(i).Item("Op_Beam").ToString) = False Then
                            If Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) <> 0 Then
                                dgv_EmptyBeamDetails.Rows(n).Cells(1).Value = -1 * Math.Abs(Val(dt2.Rows(i).Item("Op_Beam").ToString))
                            Else
                                dgv_EmptyBeamDetails.Rows(n).Cells(1).Value = Math.Abs(Val(dt2.Rows(i).Item("Op_Beam").ToString))
                            End If
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

                da2 = New SqlClient.SqlDataAdapter("Select a.DeliveryTo_Idno , sum(a.Empty_Bags) as Op_Bags from Stock_Empty_BeamBagCone_Processing_Details a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and ( a.DeliveryTo_Idno = " & Str(Val(idno)) & " or a.ReceivedFrom_Idno = " & Str(Val(idno)) & " ) and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Empty_Bags <> 0 Group by a.DeliveryTo_Idno , A.ReceivedFrom_Idno ", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Bags").ToString) = False Then
                        If Val(dt2.Rows(0).Item("DeliveryTo_Idno").ToString) <> 0 Then
                            txt_EmptyBags.Text = -1 * Math.Abs(Val(dt2.Rows(0).Item("Op_Bags").ToString))
                        Else
                            txt_EmptyBags.Text = Math.Abs(Val(dt2.Rows(0).Item("Op_Bags").ToString))
                        End If
                        If Val(txt_EmptyBags.Text) = 0 Then
                            txt_EmptyBags.Text = ""
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.DeliveryTo_Idno , sum(a.Empty_Cones) as Op_Cones  from Stock_Empty_BeamBagCone_Processing_Details a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and ( a.DeliveryTo_Idno = " & Str(Val(idno)) & " or a.ReceivedFrom_Idno = " & Str(Val(idno)) & " ) and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Empty_Cones <> 0 Group by a.DeliveryTo_Idno , A.ReceivedFrom_Idno ", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    If IsDBNull(dt2.Rows(0).Item("Op_Cones").ToString) = False Then
                        If Val(dt2.Rows(0).Item("DeliveryTo_Idno").ToString) <> 0 Then
                            txt_EmptyCones.Text = -1 * Math.Abs(Val(dt2.Rows(0).Item("Op_Cones").ToString))
                        Else
                            txt_EmptyCones.Text = Math.Abs(Val(dt2.Rows(0).Item("Op_Cones").ToString))
                        End If
                        If Val(txt_EmptyCones.Text) = 0 Then
                            txt_EmptyCones.Text = ""
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name , Lh.Ledger_Name as Godown_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo LEFT JOIN Ledger_Head Lh ON a.WareHouse_IdNo = Lh.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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

                        If IsDBNull(dt2.Rows(i).Item("Bags").ToString) = False Then
                            If Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) <> 0 Then
                                dgv_YarnDetails.Rows(n).Cells(4).Value = -1 * Math.Abs(Val(dt2.Rows(i).Item("Bags").ToString))
                            Else
                                dgv_YarnDetails.Rows(n).Cells(4).Value = Math.Abs(Val(dt2.Rows(i).Item("Bags").ToString))
                            End If
                            If Val(dgv_YarnDetails.Rows(n).Cells(4).Value) = 0 Then
                                dgv_YarnDetails.Rows(n).Cells(4).Value = ""
                            End If
                        End If

                        If IsDBNull(dt2.Rows(i).Item("Cones").ToString) = False Then
                            If Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) <> 0 Then
                                dgv_YarnDetails.Rows(n).Cells(5).Value = -1 * Math.Abs(Val(dt2.Rows(i).Item("Cones").ToString))
                            Else
                                dgv_YarnDetails.Rows(n).Cells(5).Value = Math.Abs(Val(dt2.Rows(i).Item("Cones").ToString))
                            End If
                            If Val(dgv_YarnDetails.Rows(n).Cells(5).Value) = 0 Then
                                dgv_YarnDetails.Rows(n).Cells(5).Value = ""
                            End If
                        End If

                        If IsDBNull(dt2.Rows(i).Item("Weight").ToString) = False Then
                            If Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) <> 0 Then
                                dgv_YarnDetails.Rows(n).Cells(6).Value = -1 * Format(Math.Abs(Val(dt2.Rows(i).Item("Weight").ToString)), "########0.000")
                            Else
                                dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Math.Abs(Val(dt2.Rows(i).Item("Weight").ToString)), "########0.000")
                            End If
                            If Val(dgv_YarnDetails.Rows(n).Cells(6).Value) = 0 Then
                                dgv_YarnDetails.Rows(n).Cells(6).Value = ""
                            End If

                        End If

                        'dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)

                        'dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones").ToString)

                        'dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")


                        dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("set_no").ToString

                        dgv_YarnDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Godown_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Lot_No").ToString


                    Next i

                    'dgv_YarnDetails.CurrentCell.Selected = False

                End If

                TotalYarn_Calculation()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name from Stock_SizedPavu_Processing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(idno)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ends_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Noof_Pcs").ToString
                        dgv_PavuDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Pavu_Delivery_Code").ToString

                    Next i

                    'dgv_PavuDetails.CurrentCell.Selected = False

                End If

                TotalPavu_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

    End Sub

    Private Sub Opening_Balance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PavuGrid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PavuGrid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Opening_Balance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dttm As DateTime

        FrmLdSTS = True

        Me.Text = ""

        dttm = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
        lbl_Heading.Text = "OPENING STOCK    -    AS ON  :  " & dttm.ToShortDateString

        If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
            For i = 0 To dgv_YarnDetails.Rows.Count - 1
                dgv_YarnDetails.Columns(8).ReadOnly = False
            Next
        End If

        con.Open()

        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CrDrType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyCones.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_WareHouse.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PavuGrid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CrDrType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyCones.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_WareHouse.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PavuGrid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler btnSave.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OpAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBags.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OpAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBags.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Opening_Balance_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Opening_Balance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                Close_Form()
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
            If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub

            With dgv_EmptyBeamDetails
                If .Visible Then

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
        dgtxt_Details = CType(dgv_EmptyBeamDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        'dgv_ActCtrlName = dgv_Details.Name
        dgv_EmptyBeamDetails.EditingControl.BackColor = Color.Lime
        dgv_EmptyBeamDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_EmptyBeamDetails

            If Val(dgv_EmptyBeamDetails.CurrentCell.ColumnIndex.ToString) = 1 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
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
            If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub

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
        If Not IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then dgv_EmptyBeamDetails.CurrentCell.Selected = False
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

    Private Sub cbo_Grid_VendorName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_VendorName.TextChanged
        Try
            If cbo_Grid_VendorName.Visible Then
                If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub

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
                If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_beamwidth, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_IdNo = 0)")
        With dgv_EmptyBeamDetails

            If (e.KeyValue = 38 And cbo_Grid_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        btnSave.Focus()
                    End If

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

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        btnSave.Focus()
                    End If


                Else
                    .Focus()
                    dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(dgv_EmptyBeamDetails.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If
    End Sub


    Private Sub cbo_Grid_beamwidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_beamwidth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_beamwidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or ActiveControl.Name = dgv_PavuDetails.Name Or ActiveControl.Name = dgv_EmptyBeamDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf ActiveControl.Name = dgv_EmptyBeamDetails.Name Then
                dgv1 = dgv_EmptyBeamDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_EmptyBeamDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_EmptyBeamDetails

            ElseIf tab_Main.SelectedIndex = 0 Then
                dgv1 = dgv_YarnDetails

            ElseIf tab_Main.SelectedIndex = 1 Then
                dgv1 = dgv_PavuDetails

            ElseIf tab_Main.SelectedIndex = 1 Then
                dgv1 = dgv_EmptyBeamDetails

            Else
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function

            End If

            With dgv1

                '-------------------------- WARPING DETAILS (SET1)

                If dgv1.Name = dgv_YarnDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(1)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_YarnDetails.Text) = 0)) Then
                                tab_Main.SelectTab(1)

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
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If



                    '-------------------------- SIZING DETAILS (SET1)

                ElseIf dgv1.Name = dgv_PavuDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                    save_record()

                                Else
                                    tab_Main.SelectTab(0)
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                                    dgv_YarnDetails.CurrentCell.Selected = False
                                    cbo_Ledger.Focus()

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_PavuDetails.Text) = 0)) Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                    save_record()

                                Else
                                    tab_Main.SelectTab(0)
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                                    dgv_YarnDetails.CurrentCell.Selected = False
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
                                tab_Main.SelectTab(0)


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 2)

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
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    btnSave.Focus()
                                End If

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
                                btnSave.Focus()

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
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Opening_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Opening_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

        da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code <> ''", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("BabyCone Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(ledger_idno) from Ledger_Head where ledger_idno <> 0", con)
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
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim NewCode As String = ""
        Dim LedName As String
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouAmt As Single
        Dim Dlv_IdNo As Integer, Rec_IdNo As Integer
        Dim Cnt_ID As Integer, pCnt_ID As Integer
        Dim Mil_ID As Integer
        Dim Gdn_ID As Integer
        Dim vSetNo As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Bw_ID As Integer = 0
        Dim Ven_id As Integer = 0
        Dim vMtr_Pc As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Opening_Stock, New_Entry) = False Then Exit Sub

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

        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                Sno = Sno + 1

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(1).Value))
                If Val(Cnt_ID) = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(1)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Common_Procedures.settings.CustomerCode = "1288" Then
                    Dim l As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(8).Value))
                    If l = 0 Then
                        MessageBox.Show("Invalid Location Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tab_Main.SelectTab(0)
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(8)
                        Exit Sub
                    End If
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(2)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(3).Value))
                If Val(Mil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(3)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If
                Gdn_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(8).Value))
                'If Val(Gdn_ID) = 0 Then
                '    MessageBox.Show("Select Godown Name!...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                '    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                '    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(8)
                '    dgv_YarnDetails.CurrentCell.Selected = True
                '    Exit Sub
                'End If

                If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" And Trim(dgv_YarnDetails.Rows(i).Cells(7).Value) = "" Then
                    MessageBox.Show("Invalid SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(7)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(7).Value) <> "" Then
                    If InStr(1, Trim(dgv_YarnDetails.Rows(i).Cells(7).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(7)
                        dgv_YarnDetails.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If

                If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then

                    If InStr(1, Trim(UCase(Dup_SetCd)), "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(7).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(7)
                        dgv_YarnDetails.CurrentCell.Selected = True
                        Exit Sub
                    End If

                    Dup_SetCd = Trim(Dup_SetCd) & "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(7).Value)) & "~"

                End If

            End If

        Next


        For i = 0 To dgv_PavuDetails.RowCount - 1

            If Val(dgv_PavuDetails.Rows(i).Cells(6).Value) <> 0 Then

                If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                    MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(dgv_PavuDetails.Rows(i).Cells(1).Value), " ") > 0 Then
                    MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Val(dgv_PavuDetails.Rows(i).Cells(2).Value) = 0 Then
                    MessageBox.Show("Invalid Ends", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_PavuDetails.Rows(i).Cells(3).Value) = "" Then
                    MessageBox.Show("Invalid Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(3)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) = "" Then
                    MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(4)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(dgv_PavuDetails.Rows(i).Cells(4).Value), " ") > 0 Then
                    MessageBox.Show("Invalid Beam No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(4)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(dgv_PavuDetails.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(dgv_PavuDetails.Rows(i).Cells(4).Value)) & "~") > 0 Then
                    MessageBox.Show("Duplicate BeamNo for set no. " & Trim(dgv_PavuDetails.Rows(i).Cells(1).Value), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(4)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(dgv_PavuDetails.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(dgv_PavuDetails.Rows(i).Cells(4).Value)) & "~"

            End If

        Next

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

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
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

        tr = con.BeginTransaction

        Try

            OpDate = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
            'OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
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

                VouAmt = Val(txt_OpAmount.Text)
                If Trim(UCase(cbo_CrDrType.Text)) = "DR" Then VouAmt = -1 * VouAmt

                Sno = Sno + 1

                cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Sl_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification, Software_Module_IdNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(lbl_IdNo.Text)) & ", 'Opng', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(VouAmt)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & ")"
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

                        If Val(.Rows(i).Cells(1).Value) < 0 Then
                            Dlv_IdNo = Val(lbl_IdNo.Text)
                        Else
                            Rec_IdNo = Val(lbl_IdNo.Text)
                        End If

                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( SoftwareType_IdNo  , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Vendor_IdNo, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ( " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,  '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', " & Str(Val(Sno)) & " , " & Str(Val(Ven_id)) & ", " & Str(Val(Bw_ID)) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(1).Value))) & ", 0, 0, '' )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With

            If Val(txt_EmptyBags.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyBags.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details( SoftwareType_IdNo , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Vendor_IdNo, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values (  " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,    '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', 100, 0, 0, 0, " & Str(Math.Abs(Val(txt_EmptyBags.Text))) & ", 0, '' )"
                Nr = cmd.ExecuteNonQuery()

            End If

            If Val(txt_EmptyCones.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyCones.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( SoftwareType_IdNo , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Vendor_IdNo, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ( " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,    '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', 101, 0, 0, 0, 0, " & Str(Math.Abs(Val(txt_EmptyCones.Text))) & ", '' )"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Delivered_Bags = 0 and Delivered_Cones = 0 and Delivered_Weight = 0"
            cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_YarnDetails.RowCount - 1

                If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                    Sno = Sno + 1

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(1).Value), tr)

                    Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(3).Value), tr)
                    'Gdn_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(8).Value), tr)

                    vSetCd = ""
                    vSetNo = ""
                    Selc_SetCode = ""
                    If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                        vSetNo = Trim(dgv_YarnDetails.Rows(i).Cells(7).Value)
                        If Trim(vSetNo) <> "" Then
                            vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                            Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                        End If
                    End If

                    Dlv_IdNo = 0
                    Rec_IdNo = 0
                    If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If

                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(   SoftwareType_IdNo                                           ,               Reference_Code            ,                 Company_IdNo      ,              Reference_No          ,                                 for_OrderBy                            , Reference_Date ,       DeliveryTo_Idno      ,      ReceivedFrom_Idno     , Party_Bill_No  ,              Sl_No    ,           Count_IdNo     ,                            Yarn_Type                   ,          Mill_IdNo       ,                                        Bags                        ,                                       Cones                        ,                              Weight                                , Particulars , Posting_For ,          Set_Code      ,            Set_No      ,   WareHouse_IdNo  ,Lot_NO) " &
                                      "Values                                   ( " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,           '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(Val(lbl_IdNo.Text)) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & " ,   @OpeningDate , " & Str(Val(Dlv_IdNo)) & " , " & Str(Val(Rec_IdNo)) & " ,        ''      , " & Str(Val(Sno)) & " , " & Str(Val(Cnt_ID)) & " , '" & Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) & "' , " & Str(Val(Mil_ID)) & " , " & Str(Math.Abs(Val(dgv_YarnDetails.Rows(i).Cells(4).Value))) & " , " & Str(Math.Abs(Val(dgv_YarnDetails.Rows(i).Cells(5).Value))) & " , " & Str(Math.Abs(Val(dgv_YarnDetails.Rows(i).Cells(6).Value))) & " ,     ''      ,   'OPENING' , '" & Trim(vSetCd) & "' , '" & Trim(vSetNo) & "' ," & Val(Gdn_ID) & ",'" & Trim(dgv_YarnDetails.Rows(i).Cells(9).Value) & "')"
                    cmd.ExecuteNonQuery()

                    If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                        Nr = 0
                        cmd.CommandText = "Update Stock_BabyCone_Processing_Details set " &
                                    " Baby_Bags = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " &
                                    " Baby_Cones = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " &
                                    " Baby_Weight = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & " " &
                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " &
                                    " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "'"

                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Stock_BabyCone_Processing_Details( Reference_Code, " _
                                      & "Company_IdNo, Reference_No, For_OrderBy, Reference_Date, Ledger_IdNo, " _
                                      & "Set_Code, Set_No, setcode_forSelection, " _
                                      & "Ends_Name, Mill_Idno, Count_IdNo, Bag_No, Baby_Bags, " _
                                      & "Baby_Cones, Baby_Weight, Delivered_Bags, Delivered_Cones, Delivered_Weight) Values ( '" _
                                      & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " _
                                      & Str(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_IdNo.Text))) & ", @OpeningDate, " _
                                      & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '', " & Str(Mil_ID) & ", " & Str(Cnt_ID) & ", 1, " _
                                      & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " _
                                      & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & ", 0, 0, 0)"

                            cmd.ExecuteNonQuery()

                        End If

                    End If

                End If

            Next

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = ''"
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_PavuDetails.RowCount - 1

                If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) <> "" And Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) <> "" And Val(dgv_PavuDetails.Rows(i).Cells(6).Value) <> 0 Then

                    Sno = Sno + 1

                    pCnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_PavuDetails.Rows(i).Cells(3).Value), tr)

                    vSetCd = ""
                    Selc_SetCode = ""
                    vSetNo = Trim(dgv_PavuDetails.Rows(i).Cells(1).Value)
                    If Trim(vSetNo) <> "" Then
                        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                        Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                    End If


                    vMtr_Pc = ""
                    If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then
                        vMtr_Pc = Format(Val(dgv_PavuDetails.Rows(i).Cells(6).Value) / Val(dgv_PavuDetails.Rows(i).Cells(5).Value), "###########0.00")
                    End If


                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,   Reference_Date = @OpeningDate, Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ", Ends_Name = '" & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) & "', Count_IdNo = " & Str(Val(pCnt_ID)) & ", Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0, Sl_No = " & Str(Val(Sno)) & ", ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_PavuDetails.Rows(i).Cells(4).Value))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(5).Value)) & ", Meters_Pc = " & Str(Val(vMtr_Pc)) & ", Meters = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(6).Value)) & ", Warp_Meters = 0 , EndsCount_idno = 0 " &
                                        " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) & "'"
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details(                  SoftwareType_IdNo                                ,        Reference_Code                          , Company_IdNo                     ,    Reference_No                   ,    for_OrderBy                                                        ,    Reference_Date , Ledger_IdNo                    , Set_Code              , Set_No                , setcode_forSelection        , Ends_Name                                             , count_idno               , Mill_IdNo  , Beam_Width_Idno, Sizing_SlNo, Sl_No                , Beam_No                                               , ForOrderBy_BeamNo                                                                              ,  Gross_Weight, Tare_Weight, Net_Weight , Noof_Pcs                                                , Meters_Pc                , Meters                                                  , Warp_Meters, Pavu_Delivery_Code, Pavu_Delivery_Increment, DeliveryTo_Name , EndsCount_idno )" &
                                        " Values                                         (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate      , " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) & "', " & Str(Val(pCnt_ID)) & ", 0          , 0              , 0          , " & Str(Val(Sno)) & ", '" & Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_PavuDetails.Rows(i).Cells(4).Value))) & ",  0           , 0          , 0          , " & Str(Val(dgv_PavuDetails.Rows(i).Cells(5).Value)) & ", " & Str(Val(vMtr_Pc)) & ", " & Str(Val(dgv_PavuDetails.Rows(i).Cells(6).Value)) & ", 0          , ''                , 0                      ,               ''   ,   0          )"
                        cmd.ExecuteNonQuery()
                    End If

                End If

            Next

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_CrDrType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CrDrType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        With cbo_CrDrType
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CrDrType.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CrDrType, Nothing, Nothing, "", "", "", "")
            With cbo_CrDrType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpAmount.Focus()
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
                        txt_EmptyBeam.Focus()
                    Else
                        txt_EmptyBags.Focus()
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub cbo_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CrDrType.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CrDrType, Nothing, "", "", "", "")
            If Asc(e.KeyChar) = 13 Then
                If txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
                    txt_EmptyBeam.Focus()
                Else
                    txt_EmptyBags.Focus()
                End If
            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub cbo_CrDrType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CrDrType.LostFocus
        With cbo_CrDrType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")
        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Dim LedIdNo As Integer = 0

        With cbo_Ledger
            .BackColor = Color.White
            .ForeColor = Color.Black

            If Trim(cbo_Ledger.Text) <> "" Then

                If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

                    .Tag = .Text

                    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Text)
                    If Val(LedIdNo) <> 0 Then
                        move_record(LedIdNo)
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_EmptyBags, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim LedIdNo As Integer = 0
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Ledger.Text) <> "" Then
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then

                    cbo_Ledger.Tag = cbo_Ledger.Text

                    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    If Val(LedIdNo) <> 0 Then
                        move_record(LedIdNo)
                    End If


                End If
            End If

            If txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
                txt_EmptyBeam.Focus()

            Else
                txt_EmptyBags.Focus()

            End If

        End If

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

    Private Sub txt_OpAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OpAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With cbo_Grid_CountName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            If e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False Then
                e.Handled = True

                With dgv_YarnDetails
                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        txt_EmptyCones.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                        .CurrentCell.Selected = True


                    End If
                End With

            ElseIf e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False Then

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

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                e.Handled = True

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
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.LostFocus

        With cbo_Grid_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus

        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "mill_Head", "mill_name", "", "(Mill_IdNo = 0)")
            With cbo_Grid_MillName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            With cbo_Grid_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_YarnDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End With

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.LostFocus

        With cbo_Grid_MillName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

            With cbo_Grid_YarnType

                If Trim(.Text) = "" Then .Text = "MILL"

                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()

            End With

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

            With cbo_Grid_YarnType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

            If Asc(e.KeyChar) = 13 Then
                With dgv_YarnDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End With
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub cbo_Grid_YarnType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.LostFocus

        With cbo_Grid_YarnType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        dgv_YarnDetails_CellLeave(sender, e)
        'TotalYarn_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left
                    cbo_Grid_CountName.Top = .Top + rect.Top

                    cbo_Grid_CountName.Width = rect.Width
                    cbo_Grid_CountName.Height = rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                Else
                    'If cbo_Grid_CountName.Enabled Then
                    '    cbo_Grid_CountName.BringToFront()
                    '    cbo_Grid_CountName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_CountName.Visible = False
                cbo_Grid_CountName.Tag = -1
                cbo_Grid_CountName.Text = ""

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_YarnType.DataSource = Dt1
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top

                    cbo_Grid_YarnType.Width = rect.Width
                    cbo_Grid_YarnType.Height = rect.Height
                    cbo_Grid_YarnType.Text = .CurrentCell.Value

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()

                Else
                    'If cbo_Grid_YarnType.Enabled Then
                    '    cbo_Grid_YarnType.BringToFront()
                    '    cbo_Grid_YarnType.Focus()
                    'End If

                End If

            Else
                cbo_Grid_YarnType.Visible = False
                cbo_Grid_YarnType.Tag = -1
                cbo_Grid_YarnType.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 3 Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_MillName.DataSource = Dt1
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left
                    cbo_Grid_MillName.Top = .Top + rect.Top

                    cbo_Grid_MillName.Width = rect.Width
                    cbo_Grid_MillName.Height = rect.Height
                    cbo_Grid_MillName.Text = .CurrentCell.Value

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                Else
                    'If cbo_Grid_MillName.Enabled Then
                    '    cbo_Grid_MillName.BringToFront()
                    '    cbo_Grid_MillName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_MillName.Visible = False
                cbo_Grid_MillName.Tag = -1
                cbo_Grid_MillName.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 8 Then

                If cbo_Grid_WareHouse.Visible = False Or Val(cbo_Grid_WareHouse.Tag) <> e.RowIndex Then

                    cbo_Grid_WareHouse.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_Name from ledger_Head where ledger_type='Godown' order by Ledger_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_WareHouse.DataSource = Dt1
                    cbo_Grid_WareHouse.DisplayMember = "Ledger_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_WareHouse.Left = .Left + rect.Left
                    cbo_Grid_WareHouse.Top = .Top + rect.Top

                    cbo_Grid_WareHouse.Width = rect.Width
                    cbo_Grid_WareHouse.Height = rect.Height
                    cbo_Grid_WareHouse.Text = .CurrentCell.Value

                    cbo_Grid_WareHouse.Tag = Val(e.RowIndex)
                    cbo_Grid_WareHouse.Visible = True

                    cbo_Grid_WareHouse.BringToFront()
                    cbo_Grid_WareHouse.Focus()

                Else
                    'If cbo_Grid_Godown.Enabled Then
                    '    cbo_Grid_Godown.BringToFront()
                    '    cbo_Grid_Godown.Focus()
                    'End If

                End If

            Else
                cbo_Grid_WareHouse.Visible = False
                cbo_Grid_WareHouse.Tag = -1
                cbo_Grid_WareHouse.Text = ""

            End If
        End With

    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        Try
            If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
            With dgv_YarnDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                        TotalYarn_Calculation()
                    End If
                End If
            End With
        Catch ex As Exception
            '------
        End Try


    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.Enter
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_YarnDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_YarnDetails.KeyPress
        If dgv_YarnDetails.CurrentCell.ColumnIndex <> 9 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
        With dgv_YarnDetails
            If Asc(e.KeyChar) = 13 Then
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        Exit Sub
                    End If
                Else
                    tab_Main.SelectTab(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.CurrentRow.Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_YarnDetails.KeyUp
        dgv_YarnDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails
                If .CurrentRow.Index = 0 And .RowCount = 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

                TotalYarn_Calculation()

            End With
        End If


    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
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

    Private Sub cbo_PavuGrid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.GotFocus

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With cbo_PavuGrid_CountName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_CountName.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PavuGrid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With cbo_PavuGrid_CountName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_PavuDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With


                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    With dgv_PavuDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PavuGrid_CountName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PavuGrid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_PavuDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_CountName.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End With
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PavuGrid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PavuGrid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.LostFocus

        With cbo_PavuGrid_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        Call dgv_PavuDetails_CellLeave(sender, e)
        'TotalPavu_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 3 Then

                If cbo_PavuGrid_CountName.Visible = False Or Val(cbo_PavuGrid_CountName.Tag) <> e.RowIndex Then

                    cbo_PavuGrid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_PavuGrid_CountName.DataSource = Dt1
                    cbo_PavuGrid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_PavuGrid_CountName.Left = .Left + rect.Left
                    cbo_PavuGrid_CountName.Top = .Top + rect.Top

                    cbo_PavuGrid_CountName.Width = rect.Width
                    cbo_PavuGrid_CountName.Height = rect.Height
                    cbo_PavuGrid_CountName.Text = .CurrentCell.Value

                    cbo_PavuGrid_CountName.Tag = Val(e.RowIndex)
                    cbo_PavuGrid_CountName.Visible = True

                    cbo_PavuGrid_CountName.BringToFront()
                    cbo_PavuGrid_CountName.Focus()

                Else
                    'If cbo_PavuGrid_CountName.Enabled Then
                    '    cbo_PavuGrid_CountName.BringToFront()
                    '    cbo_PavuGrid_CountName.Focus()
                    'End If

                End If

            Else
                cbo_PavuGrid_CountName.Visible = False
                cbo_PavuGrid_CountName.Tag = -1
                cbo_PavuGrid_CountName.Text = ""

            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave

        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        With dgv_PavuDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        If dgv_PavuDetails.CurrentCell.ColumnIndex = 2 Or dgv_PavuDetails.CurrentCell.ColumnIndex = 5 Or dgv_PavuDetails.CurrentCell.ColumnIndex = 6 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        dgv_PavuDetails_KeyUp(sender, e)
    End Sub


    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If .CurrentRow.Index = 0 And .RowCount = 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

                TotalPavu_Calculation()

            End With
        End If

    End Sub

    Private Sub dgv_PavuDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

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
                If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(4).Value) <> "" And Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(5).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBms)
            .Rows(0).Cells(5).Value = Val(TotPcs)
            .Rows(0).Cells(6).Value = Format(Val(TotMtrs), "########0.000")
        End With

    End Sub

    Private Sub cbo_PavuGrid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.TextChanged
        Try
            If cbo_PavuGrid_CountName.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_PavuGrid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyCones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyCones.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyCones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged

        If tab_Main.SelectedIndex = 0 Then
            If dgv_YarnDetails.Rows.Count <= 0 Then dgv_YarnDetails.Rows.Add()
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True

        ElseIf tab_Main.SelectedIndex = 1 Then
            If dgv_PavuDetails.Rows.Count <= 0 Then dgv_PavuDetails.Rows.Add()
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub cbo_Grid_WareHouse_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_WareHouse.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_WareHouse_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_WareHouse.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_WareHouse, Nothing, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
        With dgv_YarnDetails
            If e.KeyCode = 38 And cbo_Grid_WareHouse.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible = True Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                Else
                    txt_EmptyCones.Focus()
                End If
            End If

            If e.KeyCode = 40 And cbo_Grid_WareHouse.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.CurrentRow.Cells(9)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End With
    End Sub

    Private Sub cbo_Grid_WareHouse_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_WareHouse.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_WareHouse, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
        With dgv_YarnDetails
            If Asc(e.KeyChar) = 13 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.CurrentRow.Cells(9)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End With
    End Sub

    Private Sub cbo_Grid_WareHouse_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_WareHouse.KeyUp
        If e.KeyCode = 17 And e.Control = True Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_WareHouse.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_WareHouse_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_WareHouse.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If cbo_Grid_WareHouse.Visible = True Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_WareHouse.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_WareHouse.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
End Class