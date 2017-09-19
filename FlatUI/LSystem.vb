﻿

Public Class LSystem
        Private axiom As String
        Private result As String
        Private rules As Dictionary(Of Char, String)

        Public Sub New(a As String)
            axiom = a
        'result = New String(axiom)
        rules = New Dictionary(Of Char, String)
        End Sub

        Public Sub AddRule(c As Char, str As String)
            rules.Add(c, str)
        End Sub

    Public Sub DoIteration()
        result = Nothing
        For Each c As Char In axiom
            Dim str As String
            If (rules.TryGetValue(c, str)) Then
                result += str
            Else
                result += c
            End If
        Next
        axiom = result
    End Sub
End Class
