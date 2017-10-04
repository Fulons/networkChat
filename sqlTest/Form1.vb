Imports System.Data.OleDb

Public Class Form1

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub AddDataToDB()
        Dim connection As New OleDbConnection
        Dim provider As String = "PROVIDER=Microsoft.ACE.OLEDB.12.0;"
        Dim name As String = "\register.accdb"
        Dim fullDbPath As String = "C:\Users\172030\Source\Repos\networkChat\Server\bin\Debug" + name 'Application.StartupPath + name
        Dim source As String = "Data source = " & fullDbPath
        connection.ConnectionString = provider & source
        Try
            connection.Open()
            Dim dataSet As New DataSet
            Dim sql As String = "SELECT * FROM [Users]"

            Dim dataAdapter As New OleDbDataAdapter(sql, connection)
            dataAdapter.Fill(dataSet, "Users") 'System.SystemException: The source table is invalid.

            Dim commandBuilder As New OleDbCommandBuilder(dataAdapter)
            Dim dataSetNewRow As DataRow
            dataSetNewRow = dataSet.Tables("Users").NewRow()
            dataSetNewRow.Item("Username") = txtName.Text
            dataSetNewRow.Item("EmailAdress") = txtEmail.Text
            dataSetNewRow.Item("Marketing") = chkMarketing.Checked
            dataSet.Tables("Users").Rows.Add(dataSetNewRow)
            dataAdapter.Update(dataSet, "Users")

            MessageBox.Show("Your record has been added to the register!")
        Catch ex As Exception
            'System.InvalidOperationException: The connection is already open.
            'System.Data.OleDb.OleDbException: A connection-level error occurred while opening the connection.
            MessageBox.Show(ex.Message)
        Finally
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try
    End Sub

    

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddDataToDB()
    End Sub
End Class
