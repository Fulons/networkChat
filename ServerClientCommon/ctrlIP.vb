Public Class ctrlIP

    Public Function GetIPString() As String
        Return New String(txtIPFirst.Text + "." + txtIPSecond.Text + "." + txtIPThird.Text + "." + txtIPFourth.Text + ":" + txtPort.Text)
    End Function
    
    Private Sub txtPort_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles txtIPFirst.KeyDown, txtIPSecond.KeyDown, txtIPThird.KeyDown, txtIPFourth.KeyDown, txtPort.KeyDown
        If Asc(e.KeyCode) > 47 And Asc(e.KeyCode) < 58 Then 'Check that inputted char is numeric
            Dim txtBox As Windows.Forms.TextBox = CType(sender, Windows.Forms.TextBox)
            txtBox.Text = txtBox.Text + Asc(e.KeyCode)
        End If
        e.Handled = True
    End Sub
End Class
