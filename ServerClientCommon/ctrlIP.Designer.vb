<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrlIP
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.txtIPFirst = New System.Windows.Forms.TextBox()
        Me.txtIPFourth = New System.Windows.Forms.TextBox()
        Me.txtIPThird = New System.Windows.Forms.TextBox()
        Me.txtIPSecond = New System.Windows.Forms.TextBox()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtIPFirst
        '
        Me.txtIPFirst.Location = New System.Drawing.Point(3, 3)
        Me.txtIPFirst.Name = "txtIPFirst"
        Me.txtIPFirst.Size = New System.Drawing.Size(27, 20)
        Me.txtIPFirst.TabIndex = 0
        Me.txtIPFirst.Text = "192"
        '
        'txtIPFourth
        '
        Me.txtIPFourth.Location = New System.Drawing.Point(102, 3)
        Me.txtIPFourth.Name = "txtIPFourth"
        Me.txtIPFourth.Size = New System.Drawing.Size(27, 20)
        Me.txtIPFourth.TabIndex = 1
        Me.txtIPFourth.Text = "192"
        '
        'txtIPThird
        '
        Me.txtIPThird.Location = New System.Drawing.Point(69, 3)
        Me.txtIPThird.Name = "txtIPThird"
        Me.txtIPThird.Size = New System.Drawing.Size(27, 20)
        Me.txtIPThird.TabIndex = 2
        Me.txtIPThird.Text = "192"
        '
        'txtIPSecond
        '
        Me.txtIPSecond.Location = New System.Drawing.Point(36, 3)
        Me.txtIPSecond.Name = "txtIPSecond"
        Me.txtIPSecond.Size = New System.Drawing.Size(27, 20)
        Me.txtIPSecond.TabIndex = 3
        Me.txtIPSecond.Text = "192"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(142, 3)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(40, 20)
        Me.txtPort.TabIndex = 4
        Me.txtPort.Text = "65555"
        '
        'ctrlIP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.txtIPSecond)
        Me.Controls.Add(Me.txtIPThird)
        Me.Controls.Add(Me.txtIPFourth)
        Me.Controls.Add(Me.txtIPFirst)
        Me.Name = "ctrlIP"
        Me.Size = New System.Drawing.Size(185, 26)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtIPFirst As System.Windows.Forms.TextBox
    Friend WithEvents txtIPFourth As System.Windows.Forms.TextBox
    Friend WithEvents txtIPThird As System.Windows.Forms.TextBox
    Friend WithEvents txtIPSecond As System.Windows.Forms.TextBox
    Friend WithEvents txtPort As System.Windows.Forms.TextBox

End Class
