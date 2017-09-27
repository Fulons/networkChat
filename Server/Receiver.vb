﻿Imports System.Net.Sockets
Imports System.Threading
Imports ServerClientCommon.ServerClientCommon
Imports System.Runtime.Serialization.Formatters.Binary

Public Class Receiver
    Private receivingThread As Thread
    Private sendingThread As Thread

#Region "Properties"
    Public Property ID As Guid
    Public Property server As Server
    Public Property client As TcpClient
    Public Property otherSideReciever As Receiver
    Public Property status As StatusEnum
    Public Property messageQueue As Queue(Of MessageBase)
    Public Property totalByteUsage As Integer
    Public Property username As String
    Public Property debugMode As Boolean = False
#End Region

#Region "Constructors"
    Public Sub New()
        ID = Guid.NewGuid()
        messageQueue = New Queue(Of MessageBase)
        status = StatusEnum.Connected
    End Sub

    Public Sub New(client As TcpClient, server As Server)
        Me.New()
        Me.client = client
        Me.server = server
        Me.client.ReceiveBufferSize = 1024
        Me.client.SendBufferSize = 1024
    End Sub
#End Region

#Region "Methods"
    'Starts sending and receiving threads
    Public Sub Start()
        receivingThread = New Thread(AddressOf ReceivingMethod)
        receivingThread.IsBackground = True
        receivingThread.Start()

        sendingThread = New Thread(AddressOf SendingMethod)
        sendingThread.IsBackground = True
        sendingThread.Start()
    End Sub

    'Gracefully stops data transmittion and disconects the client
    Private Sub Disconnect()
        If status = StatusEnum.Disconnected Then Return
        If otherSideReciever IsNot Nothing Then
            otherSideReciever.otherSideReciever = Nothing
            otherSideReciever.status = StatusEnum.Validated
            otherSideReciever = Nothing
        End If
        status = StatusEnum.Disconnected
        client.Client.Disconnect(False)
        client.Close()
        server.OnClientDisconnect(Me)
    End Sub

    'Adds message to the messageQueue
    Public Sub SendMessage(message As MessageBase)
        messageQueue.Enqueue(message)
    End Sub
#End Region

#Region "Threads Methods"
    Private Sub SendingMethod()
        While (status <> StatusEnum.Disconnected)
            If (messageQueue.Count > 0) Then
                Dim message As MessageBase = messageQueue.Peek
                Try
                    Dim f As New BinaryFormatter
                    f.Serialize(client.GetStream(), message)
                Catch ex As Exception
                    Disconnect()
                Finally
                    messageQueue.Dequeue()
                End Try
            End If
            Thread.Sleep(30)
        End While
    End Sub

    Private Sub ReceivingMethod()
        While (status <> StatusEnum.Disconnected)
            If client.Available > 0 Then
                totalByteUsage += client.Available
                Try
                    Dim f As New BinaryFormatter
                    Dim msg As MessageBase = f.Deserialize(client.GetStream())
                    OnMessageReceived(msg)
                Catch ex As Exception
                    Dim e As Exception = New Exception("Unknown message. Could not deserialize", ex)
                    Debug.WriteLine(e.Message)
                End Try
            End If
            Thread.Sleep(30)
        End While
    End Sub
#End Region

#Region "Message handlers"
    Private Sub OnMessageReceived(m As MessageBase)
        If TypeOf m Is ValidationRequest Then
            ValidationRequestHandler(m)
        ElseIf TypeOf m Is SessionRequest Then
            SessionRequestHandler(m)
        ElseIf TypeOf m Is SessionResponse Then
            SessionResponseHandler(m)
        ElseIf TypeOf m Is EndSessionRequest Then
            EndSessionRequestHandler(m)
        ElseIf TypeOf m Is DisconnectRequest Then
            DisconnectRequestHandler(m)
        ElseIf TypeOf m Is PublicMessageRequest Then
            PublicMessageRequestHandler(m)
        ElseIf otherSideReciever IsNot Nothing Then
            otherSideReciever.SendMessage(m)
        End If
    End Sub

    Private Sub PublicMessageRequestHandler(request As PublicMessageRequest)
        For Each reciever In server.receivers.Where(Function(x) x IsNot Me)
            If reciever.status <> StatusEnum.Disconnected And
                reciever.status <> StatusEnum.Connected Then
                request.username = Me.username
                reciever.SendMessage(request)
            End If
        Next
    End Sub

    Private Sub EndSessionRequestHandler(request As EndSessionRequest)
        If otherSideReciever IsNot Nothing Then
            otherSideReciever.SendMessage(New EndSessionRequest())
            otherSideReciever.status = StatusEnum.Validated
            otherSideReciever.otherSideReciever = Nothing

            Me.otherSideReciever = Nothing
            Me.status = StatusEnum.Validated
            Me.SendMessage(New EndSessionResponse(request))
        End If
    End Sub

    Private Sub DisconnectRequestHandler(request As DisconnectRequest)
        If otherSideReciever IsNot Nothing Then
            otherSideReciever.SendMessage(New DisconnectRequest())
            otherSideReciever.status = StatusEnum.Validated
        End If
        Disconnect()
    End Sub

    Private Sub SessionResponseHandler(response As SessionResponse)
        For Each reciever In server.receivers.Where(Function(x) x IsNot Me)
            If reciever.username = response.username Then
                response.username = Me.username
                If response.isConfirmed Then
                    reciever.otherSideReciever = Me
                    Me.otherSideReciever = reciever
                    Me.status = StatusEnum.InSession
                    reciever.status = StatusEnum.InSession
                End If
            Else
                response.hasError = True
                response.exception = New Exception("The session request was refused by " + response.username)
            End If
            reciever.SendMessage(response)
            Return
        Next
    End Sub

    Private Sub SessionRequestHandler(request As SessionRequest)
        Dim response As SessionResponse
        If Me.status <> StatusEnum.Validated Then
            response = New SessionResponse(request)
            response.isConfirmed = False
            response.hasError = True
            response.exception = New Exception("Could not request a new session. The current client is already in session, or is not logged in,")
            SendMessage(response)
            Return
        End If

        For Each reciever In server.receivers.Where(Function(x) x IsNot Me)
            If reciever.username = request.username Then
                If reciever.status = StatusEnum.Validated Then
                    request.username = Me.username
                    reciever.SendMessage(request)
                    Return
                End If
            End If
        Next

        response = New SessionResponse(request)
        response.isConfirmed = False
        response.hasError = True
        response.exception = New Exception(request.username + "  does not exists or not loged in or in session with another user.")
        SendMessage(response)
    End Sub

    Private Sub ValidationRequestHandler(request As ValidationRequest)
        Dim response As New ValidationResponse(request)

        Dim args As New ClientValidatingEventArgs(
            Sub()
                'confirm Action
                status = StatusEnum.Validated
                username = request.username
                response.isValid = True
                'response.callbackID = request.callbackID should not need this line any longer as it is handled by ValidationResponse base class constructor 
                SendMessage(response)
                server.OnClientValidatedSuccess(Me)
            End Sub,
            Sub()
                'refuse Action
                response.isValid = False
                response.hasError = True
                response.exception = New System.Security.Authentication.AuthenticationException("Login failed for user " + request.username)
                SendMessage(response)
                server.OnClientValidateadFail(Me)
            End Sub)
        args.receiver = Me
        args.request = request

        server.OnClientValidating(args)
    End Sub
#End Region
End Class