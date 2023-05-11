﻿Namespace SVG.Path

    Public Class S : Inherits Command
        Public Property X2 As Double
        Public Property Y2 As Double
        Public Property X As Double
        Public Property Y As Double

        Public Sub New(ByVal text As String, ByVal Optional isRelative As Boolean = False)
            MyBase.isRelative = isRelative
            Dim tokens = Parse(text)
            Me.MapTokens(tokens)
        End Sub

        Public Sub New(ByVal tokens As List(Of String), ByVal Optional isRelative As Boolean = False)
            MyBase.isRelative = isRelative
            Me.MapTokens(tokens)
        End Sub

        Private Sub MapTokens(tokens As System.Collections.Generic.List(Of String))
            X2 = Double.Parse(tokens(0))
            Y2 = Double.Parse(tokens(1))
            X = Double.Parse(tokens(2))
            Y = Double.Parse(tokens(3))
        End Sub

        Public Overrides Sub Scale(ByVal factor As Double)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub Translate(ByVal deltaX As Double, ByVal deltaY As Double)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Function ToString() As String
            Return $"{If(isRelative, "s"c, "S"c)}{X2},{Y2} {X},{Y}"
        End Function
    End Class
End Namespace