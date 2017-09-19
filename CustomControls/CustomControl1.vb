Imports System.Drawing.Drawing2D




Namespace MyControls

    '<Description("Blas"), System.ComponentModel.Browsable(True), System.ComponentModel.ComplexBindingProperties>
    Public Class buttonLayout
        Private _c1 As Color
        Private _c2 As Color
        Private _borderColor As Color
        Private _circleBorderWidth As Integer

        Public Property c1 As Color
            Get
                Return _c1
            End Get
            Set(value As Color)
                _c1 = value
            End Set
        End Property
        Public Property c2 As Color
            Get
                Return _c2
            End Get
            Set(value As Color)
                _c2 = value
            End Set
        End Property
        Public Property borderColor As Color
            Get
                Return _borderColor
            End Get
            Set(value As Color)
                _borderColor = value
            End Set
        End Property
        Public Property circleBorderWidth As Integer
            Get
                Return _circleBorderWidth
            End Get
            Set(value As Integer)
                _circleBorderWidth = value
            End Set
        End Property

        'Public Sub New()
        '    _c1 = Color.Red
        '    _c2 = Color.Blue
        '    _borderColor = Color.Black
        '    _circleBorderWidth = 5
        'End Sub

        Public Sub New(centerColor As Color, outerColor As Color, borderColor As Color, borderWidth As Integer)
            _c1 = centerColor
            _c2 = outerColor
            _borderColor = borderColor
            _circleBorderWidth = borderWidth
        End Sub
    End Class

    Public Class MyButton : Inherits Panel
        Public Property col As Color
        Enum layoutType
            layout_default
            layout_hover
            layout_down
        End Enum

        Public Property defaultLayout As buttonLayout
        Public Property hoverLayout As buttonLayout
        Public Property downLayout As buttonLayout
        Private _currentLayout As layoutType
        Public Property currentLayout As layoutType
            Get
                Return _currentLayout
            End Get
            Set(value As layoutType)
                _currentLayout = value
                Invalidate()
            End Set
        End Property

        Public Sub New()
            MyBase.New()

            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.SetStyle(ControlStyles.UserPaint, True)
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

            _currentLayout = layoutType.layout_default
            defaultLayout = New buttonLayout(Color.Red, Color.Blue, Color.Black, 5)
            hoverLayout = New buttonLayout(Color.Red, Color.Blue, Color.Black, 5)
            downLayout = New buttonLayout(Color.Red, Color.Blue, Color.Black, 5)
            BackColor = Color.Transparent

            defaultLayout.c1 = Color.Red
            defaultLayout.c2 = Color.Blue
            defaultLayout.circleBorderWidth = 5
            defaultLayout.borderColor = Color.Black
        End Sub

        Public Sub Init()
            Dim gp As New Drawing.Drawing2D.GraphicsPath
            gp.AddEllipse(-1, -1, Me.Width + 1, Me.Height + 1)
            MyBase.Region = New Region(gp)
            Me.Invalidate()
        End Sub

        Public Overrides Property BackColor As Color = Color.Transparent

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaint(e)
            Dim layout As buttonLayout
            Select Case _currentLayout
                Case layoutType.layout_default
                    layout = defaultLayout
                Case layoutType.layout_hover
                    layout = hoverLayout
                Case layoutType.layout_down
                    layout = downLayout
                Case Else
                    layout = defaultLayout
            End Select
            'Create pen used to draw ellipse border
            Dim pen As Pen = New Pen(layout.borderColor)
            pen.Width = layout.circleBorderWidth

            Dim path As GraphicsPath = New GraphicsPath
            path.AddEllipse(0, 0, MyBase.Width, MyBase.Height)

            Dim gradientBrush As New PathGradientBrush(path)
            gradientBrush.CenterColor = layout.c1
            Dim c As Color() = {layout.c2}
            gradientBrush.SurroundColors = c

            Dim halfBorderWidth As Integer = CType(layout.circleBorderWidth / 2, Integer)  'Used to fit the circle in the panel
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            e.Graphics.FillEllipse(gradientBrush, halfBorderWidth, halfBorderWidth, MyBase.Width - 1 - layout.circleBorderWidth, MyBase.Height - 1 - layout.circleBorderWidth)
            e.Graphics.DrawEllipse(pen, halfBorderWidth, halfBorderWidth, MyBase.Width - 1 - layout.circleBorderWidth, MyBase.Height - 1 - layout.circleBorderWidth)

            ResumeLayout()
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            Me.currentLayout = layoutType.layout_hover
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            Me.currentLayout = layoutType.layout_down
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            Me.currentLayout = layoutType.layout_default
        End Sub
    End Class
End Namespace