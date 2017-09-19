
Imports System.Net
Imports System.Net.Sockets
Imports ServerClientCommon.ServerClientCommon

Public Class Server

#Region "Variables"
    Public Property listener As TcpListener
    Public port As Integer
    Public isStarted As Boolean = False
    Public receivers As List(Of Receiver)
#End Region

#Region "Events"
    Public Event ClientConnected(r As Receiver)
    Public Event ClientVaildating(args As ClientValidatingEventArgs)
    Public Event ClientValidatedSuccess(r As Receiver)
    Public Event ClientValidatedFail(r As Receiver)
    Public Event ClientDisconnected(r As Receiver)
#End Region

#Region "Constructors"
    Public Sub New(port As Integer)
        Me.receivers = New List(Of Receiver)
        Me.port = port
    End Sub
#End Region

#Region "Public Methods"
    'Start listening for incomming connections
    Public Sub Start()
        If isStarted = False Then
            listener = New TcpListener(IPAddress.Any, port)
            listener.Start()            '1 Start Listener
            isStarted = True

            WaitForConnection()
        End If
    End Sub

    'Gracefully shutdown the server
    Public Sub Shutdown()
        If isStarted Then
            listener.Stop()
            isStarted = False

            Debug.WriteLine("Server stoped!")
        End If
    End Sub

    Public Sub SendToAllReceivers(msg As MessageBase)
        For Each receiver In receivers
            receiver.SendMessage(msg)
        Next
    End Sub
#End Region

#Region "Incomming connections method"
    Private Sub WaitForConnection()     '2. Wait for incomming connection
        listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf ConnectionHandler), Nothing)
    End Sub

    'Async callback to initiate new client
    Private Sub ConnectionHandler(ar As IAsyncResult)
        SyncLock receivers
            Dim newClient As New Receiver(listener.EndAcceptTcpClient(ar), Me)  '3. Acept Connection
            newClient.Start()           '4. Starts reaciever threads
            receivers.Add(newClient)
            RaiseEvent ClientConnected(newClient)
        End SyncLock
        WaitForConnection()             '5. Goto stage 2
    End Sub
#End Region

#Region "Raise event access functions for Receiver class"
    Public Sub OnClientValidating(args As ClientValidatingEventArgs)
        RaiseEvent ClientVaildating(args)
    End Sub

    Public Sub OnClientValidatedSuccess(r As Receiver)
        RaiseEvent ClientValidatedSuccess(r)
    End Sub

    Public Sub OnClientValidateadFail(r As Receiver)
        RaiseEvent ClientValidatedFail(r)
    End Sub

    Public Sub OnClientDisconnect(r As Receiver)
        receivers.Remove(r)
        RaiseEvent ClientDisconnected(r)
    End Sub

#End Region

End Class



'Public Class TCPControl
'
'    Public Event MessageReceived(sender As TCPControl, Data As String)
'
'    'SERVER CONFIG
'    Public ServerIP As IPAddress = IPAddress.Parse("127.0.0.1")
'    Public ServerPort As Integer = 64555
'    Public Server As TcpListener
'
'    Private CommThread As Thread
'    Public IsListening As Boolean = True
'
'    'CLIENTS
'    Private Client As TcpClient
'    Private ClientData As StreamReader
'
'    Public Sub New()
'        Server = New TcpListener(ServerIP, ServerPort)
'        Server.Start()
'
'        CommThread = New Thread(New ThreadStart(AddressOf Listening))
'        CommThread.Start()
'    End Sub
'
'    Private Sub Listening()
'        'CREATE LISTENER LOOP
'        Do Until IsListening = False
'            'ACCEPT INCOMMING CONECTIONS
'            If Server.Pending = True Then
'                Client = Server.AcceptTcpClient()
'                ClientData = New StreamReader(Client.GetStream)
'            End If
'
'            'RAISE EVENT FOR INCOMMING MESSAGES
'            Try
'                RaiseEvent MessageReceived(Me, ClientData.ReadLine)
'            Catch ex As Exception
'
'            End Try
'
'            'REDUCE CPU USAGE
'            Thread.Sleep(100)
'        Loop
'    End Sub
'
'End Class


'Module Server
'
'    Sub Main()
'
'    End Sub

'Sub Main()
'    Dim serverSocket As New TcpListener(Net.IPAddress.Any, 8888)
'    Dim clientSocket As TcpClient
'    Dim counter As Integer
'
'    serverSocket.Start()
'    msg("Server started")
'    counter = 0
'    While (True)
'        msg("Waiting for clients")
'        counter += 1
'        clientSocket = serverSocket.AcceptTcpClient()
'        msg("Client no:" + counter.ToString + " started!")
'        Dim client As New HandleClient
'        client.startClient(clientSocket, counter)
'    End While
'End Sub
'
'Sub msg(ByVal mesg As String)
'    mesg.Trim()
'    Console.WriteLine(" >> " + mesg)
'End Sub
'
'Public Class HandleClient
'    Dim clientSocket As TcpClient
'    Dim clNo As Integer
'    Public Sub startClient(ByVal inClientSocket As TcpClient, ByVal clientNo As Integer)
'        Me.clientSocket = inClientSocket
'        Me.clNo = clientNo
'        Dim ctThread As New Threading.Thread(AddressOf doChat)
'        ctThread.Start()
'    End Sub
'
'    Private Sub doChat()
'        Dim requesCount As Integer = 0
'        Dim bytesFrom(65536) As Byte
'        Dim dataFromClient As String
'        Dim sendBytes As [Byte]()
'        Dim serverResponse As String
'        Dim rCount As String
'
'        While (True)
'            Try
'                msg("Waiting for data from client")
'                requesCount = requesCount + 1
'                Dim networkStream As NetworkStream = clientSocket.GetStream()
'                networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize)
'                dataFromClient = Encoding.ASCII.GetString(bytesFrom)
'                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"))
'                msg("From client-" + clNo.ToString + dataFromClient)
'                rCount = requesCount.ToString
'                serverResponse = "Server to client(" + clNo.ToString + ") " + rCount
'                sendBytes = Encoding.ASCII.GetBytes(serverResponse)
'                networkStream.Write(sendBytes, 0, sendBytes.Length)
'                networkStream.Flush()
'                msg(serverResponse)
'            Catch ex As Exception
'                MsgBox(ex.ToString)
'            End Try
'        End While
'
'
'    End Sub
'End Class

'End Module
