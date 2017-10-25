<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChatLog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.rtxtChatLog = New System.Windows.Forms.RichTextBox()
        Me.lbUsers = New System.Windows.Forms.ListBox()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'rtxtChatLog
        '
        Me.rtxtChatLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtxtChatLog.Location = New System.Drawing.Point(12, 12)
        Me.rtxtChatLog.Name = "rtxtChatLog"
        Me.rtxtChatLog.Size = New System.Drawing.Size(579, 457)
        Me.rtxtChatLog.TabIndex = 0
        Me.rtxtChatLog.Text = ""
        '
        'lbUsers
        '
        Me.lbUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbUsers.FormattingEnabled = True
        Me.lbUsers.Location = New System.Drawing.Point(597, 12)
        Me.lbUsers.Name = "lbUsers"
        Me.lbUsers.Size = New System.Drawing.Size(120, 446)
        Me.lbUsers.TabIndex = 1
        '
        'btnQuit
        '
        Me.btnQuit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnQuit.Location = New System.Drawing.Point(642, 475)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(75, 23)
        Me.btnQuit.TabIndex = 2
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = True
        '
        'frmChatLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(729, 503)
        Me.Controls.Add(Me.btnQuit)
        Me.Controls.Add(Me.lbUsers)
        Me.Controls.Add(Me.rtxtChatLog)
        Me.Name = "frmChatLog"
        Me.Text = "frmChatLog"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtxtChatLog As System.Windows.Forms.RichTextBox
    Friend WithEvents lbUsers As System.Windows.Forms.ListBox
    Friend WithEvents btnQuit As System.Windows.Forms.Button
End Class
