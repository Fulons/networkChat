Imports System.Windows.Forms
Imports Server
Imports ServerClientCommon.ServerClientCommon
Imports System.Xml

Public Class ServerForm
    Private xmlPath As String = "D:\Password\password.xml"
#Region "Variables"
    Private WithEvents server As Server
#End Region

#Region "Form event handlers"
    Private Sub ServerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        server = New Server(6598)
        txtBox.Text = ":: SERVER STARTED ::" & vbCrLf
        server.Start()
    End Sub
    Private Sub ServerForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        server.Shutdown()
    End Sub
#End Region

#Region "Network event handles"
    Private Sub OnClientConnect(r As Receiver) Handles server.ClientConnected
        AppendConsoleText(r.username + " Connected")
    End Sub

    Private Sub OnClientVaildating(args As ClientValidatingEventArgs) Handles server.ClientVaildating
        AppendConsoleText("Validating " + args.receiver.username + "...")
        'do some vaildation
        Dim doc As New XmlDocument
        doc.Load(xmlPath)
        For Each child As XmlNode In doc.ChildNodes
            If child.Name = "Users" Then
                For Each user As XmlNode In child.ChildNodes
                    Dim u As String = user.Attributes.GetNamedItem("name").Value
                    Dim p As String = user.Attributes.GetNamedItem("password").Value
                    If args.request.username = u AndAlso args.request.password = p Then
                        args.confirmAction()
                    End If
                Next
            End If
        Next

    End Sub

    Private Sub OnClientValidatedSuccess(r As Receiver) Handles server.ClientValidatedSuccess
        AppendConsoleText("User " + r.username + " vaildated")
        UpdateClientList()
    End Sub

    Private Sub OnClientValidatedFail(r As Receiver) Handles server.ClientValidatedFail
        AppendConsoleText("User " + r.username + " has failed validation")
    End Sub

    Private Sub OnClientDisconnect(r As Receiver) Handles server.ClientDisconnected
        AppendConsoleText("User " + r.username + " has disconnected")
        UpdateClientList()
    End Sub

#End Region

#Region "Control property handling (For thread safety)"
    Private Delegate Sub UpdateClientListDelegate()
    Private Sub UpdateClientList()
        If Me.InvokeRequired Then
            Me.Invoke(New UpdateClientListDelegate(AddressOf UpdateClientList))
            Return
        End If

        'Update client list on the server form
        lbUsers.Items.Clear()
        For Each receiver In server.receivers
            lbUsers.Items.Add(receiver.username)
        Next

        'Send updated client list to all clients
        Dim users(server.receivers.Count) As String
        For i As Integer = 0 To server.receivers.Count - 1
            users(i) = server.receivers(i).username
        Next
        Dim request As New UserListUpdatedRequest
        request.list = users
        server.SendToAllReceivers(request)
    End Sub

    Private Delegate Sub AppendConsoleTextDelegate(str As String)
    Private Sub AppendConsoleText(str As String)
        If txtBox.InvokeRequired Then
            txtBox.Invoke(New AppendConsoleTextDelegate(AddressOf AppendConsoleText), New Object() {str})
            Return
        End If
        txtBox.AppendText(str & vbCrLf)
    End Sub

#End Region
    'UPDATE TEXTBOX
    'Private Sub UpdateText(TB As TextBox, txt As String)
    '    If TB.InvokeRequired Then
    '        TB.Invoke(New UpdateTextDelegate(AddressOf UpdateText), New Object() {TB, txt})
    '    Else
    '        If txt IsNot Nothing Then TB.AppendText(txt & vbCrLf)
    '    End If
    'End Sub

End Class