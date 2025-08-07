Public Class Sample_Requirement_Entry
    Implements Interface_MDIActions

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

    End Sub

    Private Sub Sample_Requirement_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        Common_Procedures.CompIdNo = 0

        Me.Text = ""

        'lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
        lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

        Me.Text = lbl_Company.Text

        new_record()

    End Sub

End Class