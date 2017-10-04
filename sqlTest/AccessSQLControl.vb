
Imports System.Data.OleDb
Public Class AccessSQLControl
    Private connection As New OleDbConnection("PROVIDER=Microsoft.ACE.OLEDB.12.0; Data source = C:\Users\172030\Source\Repos\networkChat\Server\bin\Debug\register.accdb")
    Private command As New OleDbCommand

    Public dataAdapter As OleDbDataAdapter
    Public dataTable As DataTable

    Public params As New List(Of OleDbParameter)

    Public recordCount As Integer
    Public exception As String

    Public Sub New()
    End Sub

    Public Sub New(connection As String)
        Me.connection = New OleDbConnection(connection)
    End Sub

    Public Sub ExecuteQuery(query As String)
        recordCount = 0
        exception = ""
        Try
            connection.Open()

            command = New OleDbCommand(query, connection)
            params.ForEach(Sub(p) command.Parameters.Add(p))

            params.Clear()

            dataTable = New DataTable
            dataAdapter = New OleDbDataAdapter(command)
            recordCount = dataAdapter.Fill(dataTable)
        Catch ex As Exception
            exception = ex.Message
        Finally
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try
    End Sub

    Public Sub AddParam(name As String, value As Object)
        Dim newParam As New OleDbParameter(name, value)
        params.Add(newParam)
    End Sub

    Public Function HasException(Optional Report As Boolean = False) As Boolean
        If String.IsNullOrEmpty(exception) = False Then
            If Report = True Then
                MsgBox(exception, MsgBoxStyle.Critical, "Exception:")
            End If
            Return True
        End If
        Return False
    End Function
End Class
