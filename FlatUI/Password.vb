Public Class Password

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Hide()
            formMain.Connect()
        End If
    End Sub
End Class