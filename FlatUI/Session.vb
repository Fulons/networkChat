
Public Class Session
    Private circleBorderWidth = 5   'Not used
    Private roundedCornerSize = 40  'formMain_Load

    Private drag As Boolean
    Private mousex As Integer
    Private mousey As Integer

    'Custom window move 
    Private Sub formMain_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        drag = True
        mousex = System.Windows.Forms.Cursor.Position.X - Me.Left
        mousey = System.Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub

    Private Sub formMain_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If drag Then
            Me.Left = System.Windows.Forms.Cursor.Position.X - mousex
            Me.Top = System.Windows.Forms.Cursor.Position.Y - mousey
        End If
    End Sub

    Private Sub formMain_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        drag = False
    End Sub

    Private Sub Session_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim path As New Drawing2D.GraphicsPath
        path.StartFigure()
        path.AddArc(New Rectangle(0, 0, roundedCornerSize, roundedCornerSize), 180, 90)
        path.AddLine(roundedCornerSize, 0, Me.Width - roundedCornerSize, 0)
        path.AddArc(New Rectangle(Me.Width - roundedCornerSize, 0, roundedCornerSize, roundedCornerSize), -90, 90)
        path.AddLine(Me.Width, roundedCornerSize, Me.Width, Me.Height - roundedCornerSize)
        path.AddArc(New Rectangle(Me.Width - roundedCornerSize, Me.Height - roundedCornerSize, roundedCornerSize, roundedCornerSize), 0, 90)
        path.AddLine(Me.Width - roundedCornerSize, Me.Height, roundedCornerSize, Me.Height)
        path.AddArc(New Rectangle(0, Me.Height - roundedCornerSize, roundedCornerSize, roundedCornerSize), 90, 90)
        path.CloseFigure()
        Me.Region = New Region(path)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub
End Class