Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Public Class Form1
    Dim onoff As Boolean
    Declare Auto Function SetWindowDisplayAffinity Lib "User32.dll" Alias "SetWindowDisplayAffinity" (ByVal hWnd As Integer, ByVal dwAffinity As Integer) As Boolean


    Private Const GWL_EXSTYLE As Integer = -20
    Private Const WS_EX_LAYERED As Integer = &H80000
    Private Const WS_EX_TRANSPARENT As Integer = &H20
    Private Const WS_EX_TOOLWINDOW As Integer = &H80

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetLayeredWindowAttributes(ByVal hWnd As IntPtr, ByVal crKey As Integer, ByVal bAlpha As Byte, ByVal dwFlags As Integer) As Boolean
    End Function


    Private Sub cbProtect_CheckedChanged(sender As Object, e As EventArgs) Handles cbProtect.CheckedChanged
        If cbProtect.Checked = True Then
            Dim screen As Screen
            For Each screen In Screen.AllScreens
                Dim form As New Form()
                form.FormBorderStyle = FormBorderStyle.None
                form.WindowState = FormWindowState.Maximized
                form.StartPosition = FormStartPosition.Manual
                form.ShowInTaskbar = False
                form.ShowIcon = False
                form.TopMost = True
                form.BackColor = Color.Wheat
                form.TransparencyKey = Color.Wheat
                form.Location = screen.Bounds.Location
                form.Size = screen.Bounds.Size
                form.Show()
                Dim hWnd As Integer = form.Handle

                ' Then call the function as follows
                SetWindowDisplayAffinity(hWnd, 1)
                Dim styles As Integer = GetWindowLong(form.Handle, GWL_EXSTYLE)

                SetWindowLong(form.Handle, GWL_EXSTYLE, styles Or WS_EX_LAYERED Or WS_EX_TRANSPARENT Or WS_EX_TOOLWINDOW)
            Next
            lblStatus.Text = "Status: ON"
            lblStatus.ForeColor = Color.Green
        Else
            ' Close all forms after the loop
            Dim formsToClose As New List(Of Form)()

            For Each openForm As Form In Application.OpenForms
                If openForm IsNot Me Then ' Exclude the current form
                    formsToClose.Add(openForm)
                End If
            Next

            For Each formToClose As Form In formsToClose
                formToClose.Close()
            Next
            lblStatus.Text = "Status: OFF"
            lblStatus.ForeColor = Color.Red
        End If
    End Sub
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
            NotifyIcon1.Visible = True
        End If
    End Sub
    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = MouseButtons.Left Then
            Me.Show()
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NotifyIcon1.Icon = Me.Icon
    End Sub
End Class
