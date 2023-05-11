﻿Namespace SVG.Path

    Public Class A : Inherits Command
        Public Property Rx As Double
        Public Property Ry As Double
        Public Property XRot As Double
        Public Property Large As Boolean
        Public Property Sweep As Boolean
        Public Property X As Double
        Public Property Y As Double

        Public Sub New(ByVal rx As Double, ByVal ry As Double, ByVal xRot As Double, ByVal large As Boolean, ByVal sweep As Boolean, ByVal x As Double, ByVal y As Double)
            Me.Rx = rx
            Me.Ry = ry
            Me.XRot = xRot
            Me.Large = large
            Me.Sweep = sweep
            Me.X = x
            Me.Y = y
        End Sub

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
            If (tokens.Count = 7) Then
                Me.Rx = Double.Parse(tokens(0))
                Me.Ry = Double.Parse(tokens(1))
                Me.XRot = Double.Parse(tokens(2))
                Me.Large = System.Convert.ToBoolean(Integer.Parse(tokens(3)))
                Me.Sweep = System.Convert.ToBoolean(Integer.Parse(tokens(4)))
                Me.X = Double.Parse(tokens(5))
                Me.Y = Double.Parse(tokens(6))
            End If
        End Sub


        Public Overrides Sub Scale(ByVal factor As Double)
            Rx *= factor
            Ry *= factor
            X *= factor
            Y *= factor
        End Sub

        Public Overrides Sub Translate(ByVal deltaX As Double, ByVal deltaY As Double)
            X += deltaX
            Y += deltaY
        End Sub

        Public Overrides Function ToString() As String
            Return $"{If(isRelative, "a"c, "A"c)}{Rx},{Ry} {XRot} {Convert.ToInt32(Large)},{Convert.ToInt32(Sweep)} {X},{Y}"
        End Function

    End Class
End Namespace