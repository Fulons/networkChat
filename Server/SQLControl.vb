Imports System.Data.SqlClient


Public Class SQLControl

    Private connection As New SqlConnection("Server=51.7.177.8; Database=TestDatabase; User ID=sa; Password=**********;")
    Private command As SqlCommand

    Public dataAdapter As SqlDataAdapter
    Public dataTable As DataTable

    Public params As New List(Of SqlParameter)

    Public recordCount As Integer
    Public exception As String


    Public Sub New()
    End Sub

    'Overrides the connection string
    Public Sub New(connection As String)
        Me.connection = New SqlConnection(connection)
    End Sub

    Public Sub ExecuteQuery(query As String)
        recordCount = 0
        exception = ""

        Try
            connection.Open()

            command = New SqlCommand(query, connection)
            params.ForEach(Sub(p) command.Parameters.Add(p))

            params.Clear()

            dataTable = New DataTable
            dataAdapter = New SqlDataAdapter(command)
            recordCount = dataAdapter.Fill(dataTable)

        Catch ex As Exception
            exception = "ExecuteQuery Error: " & vbNewLine & ex.Message
        Finally
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try
    End Sub

    Public Sub AddParam(name As String, value As Object)
        Dim newParam As New SqlParameter(name, value)
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
