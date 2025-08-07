Public Enum MesssageBoxIcons As Integer
    Informations = 1
    Errors = 2
    Questions = 3
    Exclamations = 4
End Enum

Public Class Tsoft_MessageBox

    Public MessageBoxResult As Integer = -1

    Private vMsg_Heading As String = ""
    Private vMsg_Text As String = ""
    Private vMsg_FooterText As String = ""
    Private vMsg_ButtonsText As String = ""
    Private vMsg_ButIcons As Integer = 1
    Private vMsg_CurDefButton As Integer = 1
    Private vMsg_CanButton As Integer = -1


    Public Sub New(ByVal MessageText As String, Optional ByVal ButtonsText As String = "", Optional ByVal HeadingText As String = "", Optional ByVal FooterText As String = "", Optional ByVal MessageIcons As MesssageBoxIcons = 1, Optional ByVal Cursor_DefaultButton As Integer = 1, Optional ByVal CancelButton As Integer = -1)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.Width = 540
        Me.Height = 345


        vMsg_Heading = HeadingText
        If Trim(vMsg_Heading) = "" Then vMsg_Heading = "TSOFT"

        vMsg_Text = MessageText

        vMsg_FooterText = FooterText

        vMsg_ButtonsText = ButtonsText
        If Trim(vMsg_ButtonsText) = "" Then vMsg_ButtonsText = "OK"

        vMsg_ButIcons = MessageIcons
        'If vMsg_ButIcons <= 0 And vMsg_ButIcons > 4 Then vMsg_ButIcons = 1

        vMsg_CurDefButton = Cursor_DefaultButton
        If vMsg_CurDefButton <= 0 Then vMsg_CurDefButton = 1

        vMsg_CanButton = CancelButton

    End Sub



    Private Sub Tsoft_MessageBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim a() As String
        Dim vNoOf_Btns As Integer = 0
        Dim InDx As Integer = -1

        lbl_Heading.Text = vMsg_Heading
        lbl_MessageText.Text = vMsg_Text
        lbl_FooterText.Text = vMsg_FooterText

        a = Split(vMsg_ButtonsText, ",")

        vNoOf_Btns = UBound(a) + 1
        If vNoOf_Btns <= 0 Then vNoOf_Btns = 1

        If vNoOf_Btns >= 1 Then
            Button1.Visible = True
            Button1.Text = a(0)
            If vMsg_CanButton = 1 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 1) Then
                Button1.BackColor = Color.FromArgb(255, 90, 90)
                Button1.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 2 Then
            Button2.Visible = True
            Button2.Text = a(1)
            If vMsg_CanButton = 2 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 2) Then
                Button2.BackColor = Color.FromArgb(255, 90, 90)
                Button2.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 3 Then
            Button3.Visible = True
            Button3.Text = a(2)
            If vMsg_CanButton = 3 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 3) Then
                Button3.BackColor = Color.FromArgb(255, 90, 90)
                Button3.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 4 Then
            Button4.Visible = True
            Button4.Text = a(3)
            If vMsg_CanButton = 4 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 4) Then
                Button4.BackColor = Color.FromArgb(255, 90, 90)
                Button4.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 5 Then
            Button5.Visible = True
            Button5.Text = a(4)
            If vMsg_CanButton = 5 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 5) Then
                Button5.BackColor = Color.FromArgb(255, 90, 90)
                Button5.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 6 Then
            Button6.Visible = True
            Button6.Text = a(5)
            If vMsg_CanButton = 6 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 6) Then
                Button6.BackColor = Color.FromArgb(255, 90, 90)
                Button6.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 7 Then
            Button7.Visible = True
            Button7.Text = a(6)
            If vMsg_CanButton = 7 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 7) Then
                Button7.BackColor = Color.FromArgb(255, 90, 90)
                Button7.Tag = "CANCELBUTTON"
            End If
        End If

        If vNoOf_Btns >= 8 Then
            Button8.Visible = True
            Button8.Text = a(7)
            If vMsg_CanButton = 8 Or (vMsg_CanButton <= 0 And vNoOf_Btns = 8) Then
                Button8.BackColor = Color.FromArgb(255, 90, 90)
                Button8.Tag = "CANCELBUTTON"
            End If
        End If


        If vMsg_ButIcons = 1 Then
            PictureBox2.BackgroundImage = Global.Textile.My.Resources.Resources.information2
        ElseIf vMsg_ButIcons = 2 Then
            PictureBox2.BackgroundImage = Global.Textile.My.Resources.Resources.error1
        ElseIf vMsg_ButIcons = 3 Then
            PictureBox2.BackgroundImage = Global.Textile.My.Resources.Resources.question3
        ElseIf vMsg_ButIcons = 4 Then
            PictureBox2.BackgroundImage = Global.Textile.My.Resources.Resources.exclamation2
        Else
            PictureBox2.BackgroundImage = Global.Textile.My.Resources.Resources.exclamation1
        End If


        If vNoOf_Btns > 3 Then

            If vNoOf_Btns <= 4 Then
                Me.Width = 720

            Else
                If vNoOf_Btns <= 6 Then

                    Me.Height = 410
                    Me.Width = 720

                Else
                    Me.Height = 410
                    Me.Width = 720

                End If

            End If

        Else

            lbl_MessageText.Width = 350

            If vNoOf_Btns = 2 Then
                Button1.Left = Button1.Left + 40
                Button2.Left = Button2.Left + 100
            End If

        End If

        btn_Close.Left = Me.Width - btn_Close.Width

    End Sub

    Private Sub Tsoft_MessageBox_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If vMsg_CurDefButton = 8 Then
            Button8.Focus()
        ElseIf vMsg_CurDefButton = 7 Then
            Button7.Focus()
        ElseIf vMsg_CurDefButton = 6 Then
            Button6.Focus()
        ElseIf vMsg_CurDefButton = 5 Then
            Button5.Focus()
        ElseIf vMsg_CurDefButton = 4 Then
            Button4.Focus()
        ElseIf vMsg_CurDefButton = 3 Then
            Button3.Focus()
        ElseIf vMsg_CurDefButton = 2 Then
            Button2.Focus()
        Else
            Button1.Focus()
        End If
    End Sub

    Private Sub Tsoft_MessageBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            MessageBoxResult = 0
            Me.Close()
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        MessageBoxResult = 0
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click
        Dim vBTN As Button

        vBTN = sender

        MessageBoxResult = 0
        If vBTN.Name = Button1.Name Then
            MessageBoxResult = 1
        ElseIf vBTN.Name = Button2.Name Then
            MessageBoxResult = 2
        ElseIf vBTN.Name = Button3.Name Then
            MessageBoxResult = 3
        ElseIf vBTN.Name = Button4.Name Then
            MessageBoxResult = 4
        ElseIf vBTN.Name = Button5.Name Then
            MessageBoxResult = 5
        ElseIf vBTN.Name = Button6.Name Then
            MessageBoxResult = 6
        ElseIf vBTN.Name = Button7.Name Then
            MessageBoxResult = 7
        ElseIf vBTN.Name = Button8.Name Then
            MessageBoxResult = 8
        End If

        Me.Close()

    End Sub

    Private Sub Button1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.GotFocus, Button2.GotFocus, Button3.GotFocus, Button4.GotFocus, Button5.GotFocus, Button6.GotFocus, Button7.GotFocus, Button8.GotFocus
        Dim vBTN As Button

        vBTN = sender

        vBTN.BackColor = Color.Lime
        vBTN.ForeColor = Color.Blue

    End Sub

    Private Sub Button1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.LostFocus, Button2.LostFocus, Button3.LostFocus, Button4.LostFocus, Button5.LostFocus, Button6.LostFocus, Button7.LostFocus, Button8.LostFocus
        Dim vBTN As Button

        vBTN = sender

        If vBTN.Tag = "CANCELBUTTON" Then
            vBTN.BackColor = Color.FromArgb(255, 90, 90)
        Else
            vBTN.BackColor = Color.Gainsboro
        End If

        vBTN.ForeColor = Color.Black



    End Sub
End Class
