Public Class Delete_Old_Company_Data
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private No_Row As Long = 0

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '------
    End Sub

    Private Sub Delete_Old_Year_Data_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        FrmLdSTS = False
    End Sub

    Private Sub Delete_Old_Year_Data_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Delete_Old_Year_Data_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0
        Dim vFrmYr As Integer = 0
        Dim vToYr As Integer = 0
        Dim Comp_Id As Integer = 0

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim CompCondt As String

        FrmLdSTS = True

        con.Open()

        'vFrmYr = Val(Microsoft.VisualBasic.Left(Common_Procedures.CompGroupFnRange, 4))
        'vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))

        'cbo_FromYear.Items.Clear()
        'For i = vFrmYr To vToYr
        '    cbo_FromYear.Items.Add(Microsoft.VisualBasic.Right(i, 2) & "-" & Microsoft.VisualBasic.Right(i + 1, 2))
        'Next
        ' vFrmYr = Val(Microsoft.VisualBasic.Left(Common_Procedures.get_Company_DataBaseName(Comp_Id), 4))
        'cbo_FromYear.Items.Clear()
        'vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))
        ' Comp_Id = Common_Procedures.get_Company_DataBaseName(Comp_Id)

        'For i = 1 To Comp_Id
        '    cbo_FromYear.Items.Add(Comp_Id)
        'Next
        vFrmYr = Val(Microsoft.VisualBasic.Left(Common_Procedures.CompGroupFnRange, 4))
        vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))

        cbo_FromYear.Items.Clear()
        For i = vFrmYr To vToYr
            cbo_FromYear.Items.Add(Microsoft.VisualBasic.Right(i, 2) & "-" & Microsoft.VisualBasic.Right(i + 1, 2))
        Next

        da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head " & IIf(Trim(CompCondt) <> "", " Where ", "") & CompCondt & " order by Company_ShortName", con)
        da.Fill(dt1)
        cbo_Company.DataSource = dt1
        cbo_Company.DisplayMember = "Company_ShortName"

        cbo_Company.Enabled = True
        btn_Delete_OldData.Visible = True

    End Sub

    Private Sub Delete_Old_Year_Data_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub cbo_Company_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.GotFocus
        'With cbo_Company_id
        '    .BackColor = Color.Lime
        '    .ForeColor = Color.Blue
        '    .SelectAll()
        'End With

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Company, con, "Company_Head", "Company_ShortName", "", "(company_Idno = 0)")
    End Sub

    Private Sub cbo_Company_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.LostFocus
        With cbo_Company
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub btn_ChangePeriod_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Delete_OldData.GotFocus
        With btn_Delete_OldData
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_ChangePeriod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Delete_OldData.LostFocus
        With btn_Delete_OldData
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub

    Private Sub btn_close_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.GotFocus
        With btn_close
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_close_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.LostFocus
        With btn_close
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub
    Private Sub Cbo_Cmp_Shrt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.GotFocus
        With cbo_Company
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub Cbo_Cmp_Shrt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.LostFocus
        With cbo_Company
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_FromYear_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromYear.GotFocus
        With cbo_FromYear
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_FromYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromYear.LostFocus
        With cbo_FromYear
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub
    Private Sub cbo_FromYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FromYear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FromYear, cbo_Company, Nothing, "", "", "", "")
    End Sub

    Private Sub cbo_FromYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FromYear.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FromYear, Nothing, "", "", "", "")
        End If
    End Sub
    Private Sub cbo_Company_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Company, Nothing, cbo_FromYear, "Company_Head", "Company_ShortName", "", "(company_Idno = 0)")
    End Sub

    Private Sub cbo_Company_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Company, cbo_FromYear, "Company_Head", "Company_ShortName", "", "(company_Idno = 0)")
    End Sub
    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Delete_OldData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete_OldData.Click

        Dim tr As SqlClient.SqlTransaction

        If Trim(cbo_Company.Text) = "" Then
            MessageBox.Show("Invalid Year Code", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Company.Enabled Then cbo_Company.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the datas will lost " & Chr(13) & "Are you sure you want to Delete?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_Delete_OldData.Enabled = False
        Me.Text = ""



        Try

            Delete_All_Entry_table_By_Company()

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show(No_Row & "  Records , Old Company Data Deleted Sucessfully", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            btn_Delete_OldData.Enabled = True
            'cbo_Company.Text = ""
        Catch ex As Exception

            tr.Rollback()
            Me.Text = "COMPANY DATA DETELE ENTRY"
            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default
            btn_Delete_OldData.Enabled = True
            MessageBox.Show(ex.Message, "INVALID DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally



            btn_Delete_OldData.Enabled = True

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub Delete_All_Entry_table_By_Company()

        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Sno As Integer


        Dim company_Id = 0
        Dim Nr = 0L



        company_Id = 0
        If Trim(cbo_Company.Text) <> "" Then
            company_Id = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)
        End If

        If Val(company_Id) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Company.Enabled Then cbo_Company.Focus()
            Exit Sub
        End If

        If Trim(cbo_FromYear.Text) = "" Then
            MessageBox.Show("Invalid Year Code", "DOES  TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_FromYear.Enabled Then cbo_FromYear.Focus()
            Exit Sub
        End If

        Cmd.Connection = con
        'Cmd.Transaction = sqltr


        Cmd.CommandText = "WITH PrimaryKeys AS (
    SELECT distinct
        col.object_id,
        col.name AS pk_column_name
    FROM 
        sys.indexes AS idx
    JOIN 
        sys.index_columns AS ic ON idx.object_id = ic.object_id AND idx.index_id = ic.index_id
    JOIN 
        sys.columns AS col ON ic.object_id = col.object_id AND ic.column_id = col.column_id
    WHERE 
        idx.is_primary_key = 1 and col.name  <> 'sl_NO' and col.name  <> 'slNO' and  col.name LIKE '%code' 
)
SELECT distinct
    t.name AS table_name,
    SCHEMA_NAME(t.schema_id) AS schema_name,
       pk.pk_column_name,
        c.name as company_Id
FROM 
    sys.tables AS t
INNER JOIN 
    sys.columns AS c ON t.object_id = c.object_id
LEFT JOIN 
    PrimaryKeys AS pk ON t.object_id = pk.object_id
WHERE
    c.name LIKE '%Company_idno%'
ORDER BY 
    schema_name, table_name;
"

        '        Cmd.CommandText = "WITH PrimaryKeys AS (
        '    SELECT 
        '        col.object_id,
        '        col.name AS pk_column_name
        '    FROM 
        '        sys.indexes AS idx
        '    JOIN 
        '        sys.index_columns AS ic ON idx.object_id = ic.object_id AND idx.index_id = ic.index_id
        '    JOIN 
        '        sys.columns AS col ON ic.object_id = col.object_id AND ic.column_id = col.column_id
        '    WHERE 
        '        idx.is_primary_key = 1
        ')
        'SELECT 
        '    t.name AS table_name,
        '    SCHEMA_NAME(t.schema_id) AS schema_name,
        '    c.name AS column_name,
        '    pk.pk_column_name
        'FROM 
        '    sys.tables AS t
        'INNER JOIN 
        '    sys.columns AS c ON t.object_id = c.object_id
        'LEFT JOIN 
        '    PrimaryKeys AS pk ON t.object_id = pk.object_id
        'WHERE 
        '    c.name LIKE '%code' or  c.name LIKE 'company_Idno%'
        'ORDER BY 
        '    schema_name, table_name;
        '"

        Try


            da2 = New SqlClient.SqlDataAdapter(Cmd)
            'da2 = New SqlClient.SqlDataAdapter("Select t.name as table_name,SCHEMA_NAME (Schema_id) as schema_name,c.name as COulumn_name from sys.tables as t INNER JOIN sys.columns c ON t.OBJECT_ID=c.OBJECT_ID where c.name LIKE '%Company_idno%' ORDER BY schema_name,table_name", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            No_Row = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    If Not String.IsNullOrEmpty(dt2.Rows(i).Item("pk_column_name").ToString) And Not String.IsNullOrEmpty(dt2.Rows(i).Item("company_Id").ToString) Then

                        Cmd.CommandText = "Delete from " & dt2.Rows(i).Item("table_name").ToString & "  Where " & dt2.Rows(i).Item("company_Id").ToString & " = " & Str(Val(company_Id)) & " and  " & dt2.Rows(i).Item("pk_column_name").ToString & " LIKE '%/" & Trim(cbo_FromYear.Text) & "' "
                        Nr = Cmd.ExecuteNonQuery()

                        No_Row = No_Row + Nr
                    End If

                Next

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID DATA DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        dt2.Clear()

        Cmd.Dispose()
    End Sub


    Private Sub Delete_All_Entry_Tables(ByVal sqltr As SqlClient.SqlTransaction)
        'Dim Cmd As New SqlClient.SqlCommand
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim da2 As New SqlClient.SqlDataAdapter
        'Dim da4 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim dt2 As New DataTable
        'Dim Sno As Integer

        'Cmd.Connection = con
        'Cmd.Transaction = sqltr



        'da2 = New SqlClient.SqlDataAdapter("Select t.name as table_name,SCHEMA_NAME (Schema_id) as schema_name,c.name as COulumn_name from sys.tables as t INNER JOIN sys.columns c ON t.OBJECT_ID=c.OBJECT_ID where c.name LIKE '%Company_idno%' ORDER BY schema_name,table_name", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)






        'If dt2.Rows.Count > 0 Then

        '    For i = 0 To dt2.Rows.Count - 1

        '        Cmd.CommandText = "Delete from " & dt2.Rows(i).Item("table_name").ToString & "  Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        '        Cmd.ExecuteNonQuery()
        '    Next

        'End If

        'Cmd.Dispose()




        '    Cmd.CommandText = "Delete from AgentCommission_Processing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bale_Packing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bale_Packing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bank_Party_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bank_Party_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Beam_Knotting_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Beam_RunOut_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Delivery_Bobin_Details Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Jari_Delivery_Bobin_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_Delivery_Jari_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Jari_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Bobin_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Return_Bobin_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Jari_SalesDelivery_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Production_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Purchase_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Purchase_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Bobin_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from Bobin_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Sales_Bobin_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bobin_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from BobinSales_Invoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from BobinSales_Invoice_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from BobinSales_Invoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bundle_Packing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bundle_Packing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Bundle_UnPacking_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from bundle_Unpacking_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Buyer_Offer_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Buyer_Offer_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cheque_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Cheque_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cheque_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Closing_Stock_Value_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Excess_Short_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_BillMaking_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_BillMaking_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Processing_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Purchase_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Purchase_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cloth_Transfer_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothPurchase_Offer_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothPurchase_Offer_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothPurchase_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothPurchase_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from ClothSales_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Delivery_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Enquiry_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Enquiry_Entry_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Enquiry_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Invoice_BaleEntry_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Invoice_Bundle_Packing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Invoice_Buyer_Offer_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Invoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Invoice_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from ClothSales_Invoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_PcsDelivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_ProformaInvoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_ProformaInvoice_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_ProformaInvoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Return_Bundle_Packing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Return_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ClothSales_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Commission_Yarn_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Commission_Yarn_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Commission_Yarn_PurchaseOrder_Deatails Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Commission_Yarn_PurchaseOrder_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Commission_Yarn_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Costing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Attendance_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Attendance_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Bora_Stitching_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Cotton_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Delivery_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Delivery_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Invoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Invoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Invoice_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Invoice_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Issue_Mixing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from Cotton_Issue_Mixing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_opening_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Opening_YarnBagMixing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Packing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Packing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Purchase_Bale_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from Cotton_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_purchase_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Purchase_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Cotton_Purchase_Return_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Purchase_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Sales_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Production_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Sales_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Cotton_Waste_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DeliveryTo_Rack_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DeliveryTo_Rack_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_BillMaking_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_BillMaking_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from DoublingYarn_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Employee_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_Bag_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_Beam_Purchase_Entry_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_Beam_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_BeamBagCone_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_BeamBagCone_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_BeamBagCone_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_BeamBagCone_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Empty_Bobin_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Empty_Bobin_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from Fabric_Bundle_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Bundle_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Bundle_Entry_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Bundle_Entry_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Delivery_Sewing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Delivery_Sewing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Receipt_Sewing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Fabric_Receipt_Sewing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Finished_Product_OpeningStock_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Finished_Product_OpeningStock_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_CashSales_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_CashSales_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_CashSales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Invoice_Bale_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Invoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Invoice_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Invoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Invoice_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Proforma_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Proforma_GST_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Proforma_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Sales_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from FinishedProduct_Sales_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()





        'Cmd.CommandText = "Delete from Gate_Pass_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Gate_Pass_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from GreyFabric_OpeningStock_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Holiday_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Ic_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Ic_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Ic_Invoice_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Ic_Invoice_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from InHouse_Pavu_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Pavu_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Pavu_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Pavu_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from InHouse_Yarn_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Yarn_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Yarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Inhouse_Yarn_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Item_ExcessShort_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_PackingSlip_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_PackingSlip_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_PurchaseReturn_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_PurchaseReturn_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Item_Transfer_Head Where Company_idno  =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_Sales_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_Sales_Delivery_Jari_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_SalesDelivery_Jari_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Jari_SalesDelivery_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Job_Card_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Job_Card_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobCard_Sewing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobCard_Sewing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from JobWork_ConversionBill_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Empty_BeamBagCone_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Order_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from JobWork_Pavu_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Pavu_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_PavuYarn_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_PavuYarn_Return_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Piece_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Delete from JobWork_Piece_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Delete from JobWork_Piece_Inspection_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Sizing_Yarn_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Sizing_Yarn_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Sizing_Yarn_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Delete from JobWork_Sizing_Yarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Sizing_Yarn_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from JobWork_Weaving_Pavu_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Weaving_Pavu_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Weaving_PavuYarn_Delivery_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Weaving_PavuYarn_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Weaving_Yarn_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Weaving_Yarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Yarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Yarn_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from JobWork_Yarn_Return_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Knotting_Bill_Head Where Knotting_Bill_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from LoomNo_Production_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from LoomNo_Production_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Mark_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Mark_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Mixing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Mixing_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Mixing_Waste_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Entry_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Entry_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Entry_Tax_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Sales_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Other_GST_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Own_Order_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Own_Order_Processing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Delete from Own_Order_Sizing_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Delete from Own_Order_Weaving_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()



        'Cmd.CommandText = "Delete from Own_Order_Yarn_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Packing_Slip_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Packing_Slip_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Party_Amount_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Party_Amount_Receipt_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Delivery_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Excess_Short_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Purchase_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Pavu_Purchase_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Pavu_Receipt_Details Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Pavu_Sales_Head Where Company_idno =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Pavu_Sales_Details Where Pavu_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from YarnProcessing_Receipt_Head Where YarnProcessing_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from YarnProcessing_Receipt_Details Where YarnProcessing_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from YarnProcessing_Delivery_Head Where YarnProcessing_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from YarnProcessing_Delivery_Details Where YarnProcessing_Delivery_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from YarnProcessing_BillMaking_Head Where YarnProcessing_BillMaking_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from yARNProcessing_BillMaking_Details Where yARNProcessing_BillMaking_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from YarnBeam_Transfer_Details Where PavuYarnBeam_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Transfer_Head Where Yarn_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_test_Head Where Yarn_Test_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Return_Head Where Yarn_Sales_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Return_Details Where Yarn_Sales_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Prroforma_Head Where Yarn_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Proforma_Details Where Yarn_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Head Where Yarn_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_GST_Tax_Details Where Yarn_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Sales_Details Where Yarn_Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Receipt_Details Where PavuYarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Return_Head Where Yarn_Purchase_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Return_GST_Tax_Details Where Yarn_Purchase_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Return_Details Where Yarn_Purchase_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Receipt_Head Where Yarn_Purchase_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Receipt_Details Where Yarn_Purchase_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Order_Head Where Yarn_Purchase_Order_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Order_GST_Tax_Details Where Yarn_Purchase_Order_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Order_Details Where Yarn_Purchase_Order_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Head Where Yarn_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_GST_Tax_Details Where Yarn_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Purchase_Details Where Yarn_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Production_Head Where Yarn_Production_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Production_Details Where Yarn_Production_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Excess_Short_Head Where Yarn_Excess_Short_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Yarn_Delivery_Details Where PavuYarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaving_Yarn_Excess_Short_Head Where Yarn_Excess_Short_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Yarn_Receipt_Head Where Weaver_Yarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Yarn_Receipt_Details Where Weaver_Yarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Yarn_Delivery_Head Where Weaver_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Yarn_Delivery_Details Where Weaver_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Wages_Yarn_Details Where Weaver_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Wages_Head Where Weaver_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Wages_Details Where Weaver_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Wages_Cooly_Details Where Weaver_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Sales_Yarn_Delivery_Head Where Weaver_Sales_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Sales_Yarn_Delivery_Details Where Weaver_Sales_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Piece_Checking_Head Where Weaver_Piece_Checking_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Payment_Head Where Weaver_Payment_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Payment_Details Where Weaver_Payment_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_PavuBobin_Requirement_Head Where Weaver_PavuBobin_Requirement_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_PavuBobin_Requirement_Details Where Weaver_PavuBobin_Requirement_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Pavu_Receipt_Head Where Weaver_Pavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Pavu_Receipt_Details Where Weaver_Pavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Requirement_Details Where Weaver_Pavu_Delivery_Requirement_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Head Where Weaver_Pavu_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Details Where Weaver_Pavu_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Weaver_LoomNo_Head Where Weaver_LoomNo_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_LoomNo_Details Where Weaver_LoomNo_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_KuraiPavu_Receipt_Head Where Weaver_KuraiPavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_KuraiPavu_Receipt_Details Where Weaver_KuraiPavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Excess_Cons_Head Where Weaver_Excess_Cons_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Excess_Cons_Details Where Weaver_Excess_Cons_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Return_Jari_Details Where Bobin_Jari_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Return_Head Where Bobin_Jari_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Return_Bobin_Details Where Bobin_Jari_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_ExcessShort_Jari_Details Where Bobin_Jari_ExcessShort_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_ExcessShort_Head Where Bobin_Jari_ExcessShort_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Weaver_BobinJari_ExcessShort_Bobin_Details Where Bobin_Jari_ExcessShort_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Jari_Details Where Bobin_Jari_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Head Where Bobin_Jari_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Bobin_Details Where Bobin_Jari_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_BobinEndsCount_Details Where Bobin_Endscount_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Bobin_Return_Head Where Weaver_Bobin_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Bobin_Return_Details Where Weaver_Bobin_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Weaver_Bobin_Delivery_Requirement_Details Where Weaver_Bobin_Delivery_Requirement_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Waste_Opening_Head Where Waste_Opening_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Voucher_Head Where Voucher_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Voucher_Details Where Voucher_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Voucher_Bill_Head Where Voucher_Bill_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Voucher_Bill_Details Where Voucher_Bill_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from UnSet_Head Where UnSet_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from UnSet_Details Where UnSet_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Textile_Processing_Return_Head Where ClothProcess_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_Return_Details Where Cloth_Processing_Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_Receipt_Head Where ClothProcess_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Cloth_Processing_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_JobOrder_Head Where Job_Order_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_JobOrder_Details Where Job_Order_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_Delivery_Head Where ClothProcess_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_Delivery_Details Where Cloth_Processing_Delivery_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_BillMaking_Head Where ClothProcess_BillMaking_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Textile_Processing_BillMaking_Details Where Cloth_Processing_BillMaking_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Tex_Yarn_Delivery_Head Where Tex_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Tex_Yarn_Delivery_Details Where Tex_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from TempTable_For_NegativeStock Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from TempTable_For_Jobwork_Inspection_Stock_Posting Where JobWork_Piece_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Service_Receipt_Head Where Service_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Service_Receipt_Details Where Service_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Service_Delivery_Head Where Service_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Service_Delivery_Details Where Service_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Sales_Head Where Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Sales_Details Where Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Purchase_Head Where Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Purchase_Details Where Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Oil_Service_Entry_Head Where Oil_Service_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Return_Head Where Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Return_Details Where Return_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Receipt_Head Where Item_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Receipt_Details Where Item_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_PurchaseReturn_Head Where Item_PurchaseReturn_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_PurchaseReturn_Details Where Item_PurchaseReturn_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Purchase_Head Where Item_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Purchase_Details Where Item_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_PO_Head Where PO_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_PO_Details Where PO_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Issue_Head Where Issue_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Issue_Details Where Issue_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Excess_Short_Entry_Head Where Item_Excess_Short_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Delivery_Head Where Item_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Item_Delivery_Details Where Item_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Gate_Pass_Head Where Gate_Pass_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stores_Gate_Pass_Details Where Gate_Pass_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Stores_Dispose_Entry_Head Where Dispose_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Store_Item_Purchase_GST_Tax_Details Where Store_Item_Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Waste_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Transfer_Head Where Stock_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Transfer_Details Where Stock_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Running_Fabric_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Reeling_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Mixing_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_LooseYarn_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_HankYarn_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Empty_Bobin_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Carding_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_Bundle_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Reference_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from SpnSoft_Yarn_Receipt_Head Where Weaver_Sales_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from SpnSoft_Yarn_Receipt_Details Where Weaver_Sales_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sort_Change_Head Where Sort_Change_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Yarn_Receipt_Head Where Sizing_Yarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Yarn_Receipt_Details Where Sizing_Yarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Yarn_Delivery_Head Where Sizing_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Yarn_Delivery_Details Where Sizing_Yarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_SpecificationYarn_Details Where Sizing_Specification_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_SpecificationPavu_Details Where Sizing_Specification_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Specification_Head Where Sizing_Specification_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Pavu_Receipt_Head Where Sizing_Pavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sizing_Pavu_Receipt_Details Where Sizing_Pavu_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Set_Head Where Set_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Set_Details Where Set_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sales_InVoice_Head Where Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sales_Invoice_Details Where Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sales_Head Where Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Sales_Details Where Sales_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Running_Fabric_OutWard_Head Where Running_Fabric_OutWard_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Running_Fabric_OutWard_Details Where Running_Fabric_OutWard_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Running_Fabric_InWard_Head Where Running_Fabric_InWard_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Running_Fabric_InWard_Details Where Running_Fabric_InWard_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Roll_Packing_Head Where Roll_Packing_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Roll_Packing_Details Where Roll_Packing_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Rewinding_Receipt_Head Where Rewinding_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Rewinding_Receipt_Details Where Rewinding_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Rewinding_Delivery_Head Where Rewinding_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Rewinding_Delivery_Details Where Rewinding_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ReturnTo_Floor_Head Where ReturnTo_Floor_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ReturnTo_Floor_Details Where ReturnTo_Floor_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from report_settings_column_size Where report_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from report_settings Where report_code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Reeling_Receipt_Head Where Reeling_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Reeling_Receipt_Details Where Reeling_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Reeling_Delivery_Head Where Reeling_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Reeling_Delivery_Details Where Reeling_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Purchase_Head Where Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Purchase_Details Where Purchase_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Production_Head Where Production_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Production_Details Where Production_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from ProcessedItem_ExcessShort_Head Where ProcessedItem_ExcessShort_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Unchecked_Fabric_Opening_Head Where Processed_Unchecked_Fabric_Opening_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Waste_Delivery_Head Where Processed_Fabric_Waste_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_SalesInvoice_GST_Tax_Details Where ProcessedFabric_Sales_Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Sales_Invoice_Head Where Processed_Fabric_Sales_Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Sales_Invoice_Details Where Processed_Fabric_Sales_Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Opening_Head Where Processed_Fabric_Opening_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Invoice_BaleEntry_Details Where Sales_Invoice_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_inspection_Receipt_Details Where Processed_Fabric_Inspection_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_inspection_Head Where Processed_Fabric_inspection_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Processed_Fabric_Inspection_Details Where Processed_Fabric_Inspection_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Piece_Transfer_Head Where Piece_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Piece_Transfer_Details Where Piece_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Piece_Opening_Head Where Piece_Opening_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Piece_Excess_Short_Head Where Piece_Excess_Short_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Piece_Excess_Short_Details Where Piece_Excess_Short_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()


        'Cmd.CommandText = "Delete from Payroll_Timing_Addition_Head Where Timing_Addition_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Payroll_Timing_Addition_Details Where Timing_Addition_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Salary_Head Where Salary_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Salary_Details Where Salary_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Wages_Head Where Employee_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Wages_Details Where Employee_Wages_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Payment_Head Where Employee_Payment_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Deduction_Head Where Employee_Deduction_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Attendance_Head Where Employee_Attendance_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Employee_Attendance_Details Where Employee_Attendance_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Head Where AttendanceLog_FromMachine_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Details Where AttendanceLog_FromMachine_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PayRoll_Attendance_Timing_Details Where Employee_Attendance_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PavuYarnBeam_Transfer_Head Where PavuYarnBeam_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PavuYarn_Receipt_Head Where PavuYarn_Receipt_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PavuYarn_Delivery_Head Where PavuYarn_Delivery_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from PavuBeam_Transfer_Details Where PavuYarnBeam_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Transfer_Head Where Pavu_Transfer_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Transfer_BeamWise_head Where Pavu_Transfer_Beamwise_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "Delete from Pavu_Transfer_BeamWise_Details Where Pavu_Transfer_BeamWise_Code =" & Val(cbo_FromYear.Text) & ""
        'Cmd.ExecuteNonQuery()

        '''''''''''''''''''**************************

        'Cmd.Dispose()

    End Sub


End Class