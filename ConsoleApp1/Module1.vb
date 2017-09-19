Imports System.Net.Sockets
Imports System.Text

Module Module1

    Sub Main()
        Dim serverSocket As New TcpListener(Net.IPAddress.Any, 8888)
        Dim clientSocket As TcpClient
        Dim counter As Integer

        serverSocket.Start()
        msg("Server started")
        counter = 0
        While (True)
            counter += 1
            clientSocket = serverSocket.AcceptTcpClient()
            msg("Client no:" + counter.ToString + " started!")
            Dim client As New HandleClient
            client.startClient(clientSocket, counter)
        End While

    End Sub

    Sub msg(ByVal mesg As String)
        mesg.Trim()
        Console.WriteLine(" >> " + mesg)
    End Sub

    Public Class HandleClient
        Dim clientSocket As TcpClient
        Dim clNo As Integer
        Public Sub startClient(ByVal inClientSocket As TcpClient, ByVal clientNo As Integer)
            Me.clientSocket = inClientSocket
            Me.clNo = clientNo
            Dim ctThread As New Threading.Thread(AddressOf doChat)
            ctThread.Start()
        End Sub

        Private Sub doChat()
            Dim requesCount As Integer = 0
            Dim bytesFrom(10024) As Byte
            Dim dataFromClient As String
            Dim sendBytes As [Byte]()
            Dim serverResponse As String
            Dim rCount As String

            While (True)
                Try
                    requesCount = requesCount + 1
                    Dim networkStream As NetworkStream = clientSocket.GetStream()
                    networkStream.Read(bytesFrom, 0, CInt(clientSocket.ReceiveBufferSize))
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom)
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"))
                    msg("From client-" + clNo + dataFromClient)
                    rCount = requesCount.ToString
                    serverResponse = "Server to client(" + clNo + ") " + rCount
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse)
                    networkStream.Write(sendBytes, 0, sendBytes.Length)
                    networkStream.Flush()
                    msg(serverResponse)
                Catch ex As Exception

                End Try
            End While


        End Sub
    End Class

End Module
