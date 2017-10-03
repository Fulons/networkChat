'Imports System.Drawing.Drawing2D    'for SmoothingMode.AntiAlias
'Imports FlatUI
Imports ServerClientCommon.ServerClientCommon

Public Class formMain
#Region "Variables"
    Private circleBorderWidth = 5   'Not used
    Private roundedCornerSize = 40  'formMain_Load

    Private WithEvents client As Client
    Private centerCircleradius As Integer = 50  'pnlClock_Paint
    Private Const stickingUpMin As Integer = 10 'Not used
    Private Const StickingUpMax As Integer = 50 'Timer2_Tick
    Private Const panelStickingUpMin As Integer = 10    'Not used
    Private Const panelStickingUpMax As Integer = 150   'Timer3_Tick

    Private drag As Boolean
    Private mousex As Integer
    Private mousey As Integer

#End Region

#Region "Form Handles"
    Private Sub formMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblWelcome.Location = New Point(CType(MyBase.Width / 2 - lblWelcome.Width / 2, Integer), lblWelcome.Location.Y)

        Dim l As New LSystem("A")
        l.AddRule("A", "AB")
        l.AddRule("B", "A")
        l.DoIteration()
        l.DoIteration()
        l.DoIteration()

        Dim path As New Drawing2D.GraphicsPath
        path.StartFigure()
        path.AddArc(New Rectangle(0, 0, roundedCornerSize, roundedCornerSize), 180, 90)
        path.AddLine(roundedCornerSize, 0, Me.Width - roundedCornerSize, 0)
        path.AddArc(New Rectangle(Me.Width - roundedCornerSize, 0, roundedCornerSize, roundedCornerSize), -90, 90)
        path.AddLine(Me.Width, roundedCornerSize, Me.Width, Me.Height - roundedCornerSize)
        path.AddArc(New Rectangle(Me.Width - roundedCornerSize, Me.Height - roundedCornerSize, roundedCornerSize, roundedCornerSize), 0, 90)
        path.AddLine(Me.Width - roundedCornerSize, Me.Height, roundedCornerSize, Me.Height)
        path.AddArc(New Rectangle(0, Me.Height - roundedCornerSize, roundedCornerSize, roundedCornerSize), 90, 90)
        path.CloseFigure()
        Me.Region = New Region(path)

        client = New Client
    End Sub
    Private Sub formMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If client IsNot Nothing Then
            client.Disconnect()
        End If
    End Sub

    'Custom window move 
    Private Sub formMain_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        drag = True
        mousex = System.Windows.Forms.Cursor.Position.X - Me.Left
        mousey = System.Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub
    Private Sub formMain_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If drag Then
            Me.Left = System.Windows.Forms.Cursor.Position.X - mousex
            Me.Top = System.Windows.Forms.Cursor.Position.Y - mousey
        End If
    End Sub
    Private Sub formMain_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        drag = False
    End Sub
#End Region

#Region "Network Event Handles"
    Private Sub OnClientDisconnected(client As Client) Handles client.ClientDisconnected
        'Throw New NotImplementedException()
    End Sub
    Private Sub OnFileUploadRequest(client As Client, args As FileUploadRequestEventArguments) Handles client.FileUploadRequest
        Throw New NotImplementedException()
    End Sub
    Private Sub OnFileUploadProgress(client As Client, args As FileUploadProgressEventArguments) Handles client.FileUploadProgress
        Throw New NotImplementedException()
    End Sub
    Private Sub OnGenericRequestReceived(client As Client, request As GenericRequest) Handles client.GenericRequestReceived
        Throw New NotImplementedException()
    End Sub
    Private Sub OnSessionClientDisconnected(client As Client) Handles client.SessionClientDisconnected
        Throw New NotImplementedException()
    End Sub
    Private Sub OnSessionEndedByTheRemoteClient(client As Client) Handles client.SessionEndedByTheRemoteClient
        Throw New NotImplementedException()
    End Sub
    Private Sub OnSessionRequest(client As Client, args As SessionRequestEventArguments) Handles client.SessionRequest
        Me.Invoke(
            Sub()
                If MessageBox.Show(Me, "Session request from " + args.request.username + ". Confirm request?", Me.Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                    args.Confirm()
                Else
                    args.Refuse()
                End If
            End Sub
        )
    End Sub
    Private Sub OnTextMessageReceived(client As Client, str As String) Handles client.TextMessageReceived
        AddChatText(txtChat, str)
    End Sub

    Private Sub TextToSpeech(ByRef str As String)
        If cbTextToSpeech.Checked = True Then
            Dim speaker = CreateObject("SAPI.spVoice")
            speaker.speak(str)
        End If
    End Sub
    Private Sub OnPublicMessageReceived(client As Client, request As PublicMessageRequest) Handles client.PublicMessageReceived
        TextToSpeech(request.username + " says " + request.message)
        
        AddChatText(txtChat, request.username + " >> " + request.message)
    End Sub
    Private Sub OnUserListUpdated(client As Client, str() As String) Handles client.UserListUpdated
        UpdateUserList(lboxUsers, str)
    End Sub
#End Region

#Region "Control Event Handles"
    'Private Sub pnlClock_Paint(sender As Object, e As PaintEventArgs)
    '    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
    '    e.Graphics.FillPie(Brushes.Blue, 0, 0, pnlClock.Width - 1, pnlClock.Height - 1, -90, CType(DateTime.Now.Second / 60 * 360, Integer))
    '    e.Graphics.FillEllipse(New SolidBrush(pnlClock.Parent.BackColor),
    '                           CType((pnlClock.Width - 1) / 2 - centerCircleradius / 2, Integer),
    '                           CType((pnlClock.Height - 1) / 2 - centerCircleradius / 2, Integer),
    '                           centerCircleradius, centerCircleradius)
    '    Dim p As Pen = New Pen(Color.FromArgb(128, 255, 255, 255), 3)
    '    e.Graphics.DrawEllipse(p, 1, 1, pnlClock.Width - 3, pnlClock.Height - 3)
    'End Sub

    Public Sub Connect()
        If Not String.IsNullOrEmpty(txtUsername.Text) Then
            Dim ip As String =
                TextBox1.Text + "." +
                TextBox2.Text + "." +
                TextBox3.Text + "." +
                TextBox4.Text

            If client.Connect(ip, TextBox5.Text) = True Then
                btnConnect.Text = "Connected"
                btnConnect.Enabled = False
            End If
            client.Login(txtUsername.Text, Password.TextBox1.Text,
                     Sub(senderClient, response)
                         If response.isValid = True Then
                             client.status = StatusEnum.Validated
                             ChangeText(btnConnect, "Logged In")
                         End If
                     End Sub)
            txtUsername.Enabled = False
        End If
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Password.Show()
    End Sub

    'Send message when send button is clicked
    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        SendMessage()
    End Sub
    'Send message when enter key is pressed inside txtMsg textBox
    Private Sub txtMsg_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMsg.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendMessage()
            e.Handled = True
        End If
    End Sub
    'Sends the message
    Private Sub SendMessage()
        If client.status = StatusEnum.Validated Then
            client.SendPublicMessage(txtMsg.Text)
            AddChatText(txtChat, "ME >> " + txtMsg.Text)
            txtMsg.Clear()
            txtMsg.Focus()
        End If
    End Sub
    'Assures that only numbers can be entered in the IP and port fields
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress, TextBox2.KeyPress, TextBox4.KeyPress, TextBox3.KeyPress, TextBox5.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
            e.Handled = True
        End If
    End Sub

    Delegate Sub OpenSessionDelegate()
    Public Sub OpenSession()
        If Me.InvokeRequired Then
            Me.Invoke(New OpenSessionDelegate(AddressOf OpenSession))
            Return
        End If
        Session.Show()
    End Sub

    Private Sub RequestSessionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RequestSessionToolStripMenuItem.Click
        Dim user As String = lboxUsers.SelectedItem
        If Not String.IsNullOrEmpty(user) Then
            client.SendSessionRequest(user,
                Sub()
                    OpenSession()
                End Sub)
        End If
    End Sub

#End Region

#Region "Control property handling (For thread safety)"
    'Thread safe way of changing text on button
    Delegate Sub ChangeTextDelegate(ByVal ctrl As Button, ByVal str As String)
    Private Sub ChangeText(ByVal ctrl As Button, ByVal str As String)
        If ctrl.InvokeRequired Then
            ctrl.Invoke(New ChangeTextDelegate(AddressOf ChangeText), New Object() {ctrl, str})
            Return
        End If
        ctrl.Text = str
    End Sub

    Delegate Sub AddChatTextDelegate(ByVal txtBox As TextBox, ByVal str As String)
    Private Sub AddChatText(ByVal txtBox As TextBox, ByVal str As String)
        If txtBox.InvokeRequired Then
            txtBox.Invoke(New AddChatTextDelegate(AddressOf AddChatText), New Object() {txtBox, str})
            Return
        End If
        txtBox.AppendText(str & vbCrLf)
    End Sub

    Delegate Sub UpdateUserListDelegate(lb As ListBox, str() As String)
    Private Sub UpdateUserList(lb As ListBox, str() As String)
        If lb.InvokeRequired Then
            lb.Invoke(New UpdateUserListDelegate(AddressOf UpdateUserList), New Object() {lb, str})
            Return
        End If
        lb.Items.Clear()
        For Each user In str
            If user IsNot Nothing Then
                lb.Items.Add(user)
            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

#End Region

End Class