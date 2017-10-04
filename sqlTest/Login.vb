Public Class Login


    Private Function UserExist(email As String) As Boolean
        Dim control As New AccessSQLControl
        control.AddParam("@email", email)
        control.ExecuteQuery("SELECT * FROM [Users] WHERE Email = @email;")
        If control.recordCount = 1 Then Return True
        Return False
    End Function

    Private Function UserExist(name As String, email As String) As Boolean
        Dim control As New AccessSQLControl
        control.AddParam("@email", email)
        control.AddParam("@name", name)
        control.ExecuteQuery("SELECT * FROM [Users] WHERE Email = @email AND name = @name;")
        If control.recordCount = 1 Then Return True
        Return False
    End Function

    Private Sub AddUser(name As String, email As String, marketing As Boolean)
        Dim control As New AccessSQLControl
        control.AddParam("@un", name)
        control.AddParam("@ma", email)
        control.AddParam("@ma", marketing)
        control.ExecuteQuery("INSERT INTO [Users] (Username, Email, Marketing) VALUES (@un, @em, @ma);")
        control.HasException(True)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If UserExist(txtName.Text) Then
            MessageBox.Show("A user with this email already exists. Did you mean to press login?")
        Else
            AddUser(txtName.Text, txtEmail.Text, chkMarketing.Checked)
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If UserExist(txtName.Text, txtEmail.Text) Then
            MessageBox.Show("Welcome")
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub
End Class