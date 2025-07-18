
Imports System.Runtime.InteropServices

Public Class Class_ActiveWindow
    <DllImport("user32", CharSet:=CharSet.Auto)> _
    Public Shared Function GetActiveWindow() As Integer
    End Function

    <DllImport("user32", CharSet:=CharSet.Auto)> _
    Public Shared Function SetActiveWindow(ByVal hwnd As Integer) As Integer
    End Function
End Class
