﻿''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'	PdfFileWriter
'	PDF File Write C# Class Library.
'
'	ArcToBezier
'	Convert eliptical arc to Bezier segments.
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'	The PDF File Writer library was enhanced to allow drawing of graphic
'	artwork using Windows Presentation Foundation (WPF) classes.
'	These enhancements were proposed by Elena Malnati elena@yelleaf.com.
'	I would like to thank her for providing me with the source code
'	to implement them. Further I would like to thank Joe Cridge for
'	his contribution of code to convert elliptical arc to Bezier curve.
'	The source code was modified to be consistent in style to the rest
'	of the library. Developers of Windows Forms application can benefit
'	from all of these enhancements
'	For further information visit www.joecridge.me/bezier.pdf.
'	Also visit http://p5js.org/ for some coolness
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'	Uzi Granot
'	Version: 1.0
'	Date: April 1, 2013
'	Copyright (C) 2013-2019 Uzi Granot. All Rights Reserved
'
'	PdfFileWriter C# class library and TestPdfFileWriter test/demo
'  application are free software.
'	They is distributed under the Code Project Open License (CPOL).
'	The document PdfFileWriterReadmeAndLicense.pdf contained within
'	the distribution specify the license agreement and other
'	conditions and notes. You must read this document and agree
'	with the conditions specified in order to use this software.
'
'	For version history please refer to PdfDocument.cs
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System

Namespace PdfFileWriter
    ''' <summary>
    ''' Arc type for DrawArc method
    ''' </summary>
    Public Enum ArcType
        ''' <summary>
        ''' Small arc drawn in counter clock wise direction
        ''' </summary>
        SmallCounterClockWise

        ''' <summary>
        ''' Small arc drawn in clock wise direction
        ''' </summary>
        SmallClockWise

        ''' <summary>
        ''' Large arc drawn in counter clock wise direction
        ''' </summary>
        LargeCounterClockWise

        ''' <summary>
        ''' Large arc drawn in clock wise direction
        ''' </summary>
        LargeClockWise
    End Enum

    ''' <summary>
    ''' Convert eliptical arc to Bezier segments
    ''' </summary>
    Public Module ArcToBezier
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''' <summary>
        ''' Create eliptical arc
        ''' </summary>
        ''' <paramname="ArcStart">Arc start point</param>
        ''' <paramname="ArcEnd">Arc end point</param>
        ''' <paramname="Radius">RadiusX as width and RadiusY as height</param>
        ''' <paramname="Rotate">X axis rotation angle in radians</param>
        ''' <paramname="Type">Arc type enumeration</param>
        ''' <returns>Array of points.</returns>
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Function CreateArc(ByVal ArcStart As PointD, ByVal ArcEnd As PointD, ByVal Radius As SizeD, ByVal Rotate As Double, ByVal Type As ArcType) As PointD()
            Dim SegArray As PointD()
            Dim ScaleX = Radius.Width / Radius.Height

            ' circular arc
            If Math.Abs(ScaleX - 1.0) < 0.000001 Then
                SegArray = CircularArc(ArcStart, ArcEnd, Radius.Height, Type)
                ' eliptical arc
            ElseIf Rotate = 0.0 Then
                Dim ScaleStart As PointD = New PointD(ArcStart.X / ScaleX, ArcStart.Y)
                Dim ScaleEnd As PointD = New PointD(ArcEnd.X / ScaleX, ArcEnd.Y)
                SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type)
                ' eliptical arc rotated
                For Each Seg In SegArray
                    Seg.X *= ScaleX
                Next
            Else
                Dim CosR = Math.Cos(Rotate)
                Dim SinR = Math.Sin(Rotate)
                Dim ScaleStart As PointD = New PointD((CosR * ArcStart.X - SinR * ArcStart.Y) / ScaleX, SinR * ArcStart.X + CosR * ArcStart.Y)
                Dim ScaleEnd As PointD = New PointD((CosR * ArcEnd.X - SinR * ArcEnd.Y) / ScaleX, SinR * ArcEnd.X + CosR * ArcEnd.Y)
                SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type)

                For Each Seg In SegArray
                    Dim X = Seg.X * ScaleX
                    Seg.X = CosR * X + SinR * Seg.Y
                    Seg.Y = -SinR * X + CosR * Seg.Y
                Next
            End If

            ' replace start and end with original points to eliminate rounding errors
            SegArray(0).X = ArcStart.X
            SegArray(0).Y = ArcStart.Y
            SegArray(SegArray.Length - 1).X = ArcEnd.X
            SegArray(SegArray.Length - 1).Y = ArcEnd.Y
            Return SegArray
        End Function

        ''' <summary>
        ''' Create circular arc
        ''' </summary>
        ''' <paramname="ArcStart">Arc starting point</param>
        ''' <paramname="ArcEnd">Arc ending point</param>
        ''' <paramname="Radius">Arc radius</param>
        ''' <paramname="Type">Arc type</param>
        ''' <returns>Array of points.</returns>
        Friend Function CircularArc(ByVal ArcStart As PointD, ByVal ArcEnd As PointD, ByVal Radius As Double, ByVal Type As ArcType) As PointD()
            ' chord line from start point to end point
            Dim ChordDeltaX = ArcEnd.X - ArcStart.X
            Dim ChordDeltaY = ArcEnd.Y - ArcStart.Y
            Dim ChordLength = Math.Sqrt(ChordDeltaX * ChordDeltaX + ChordDeltaY * ChordDeltaY)

            ' test radius
            If 2 * Radius < ChordLength Then Throw New ApplicationException("Radius too small.")

            ' line perpendicular to chord at mid point
            ' distance from chord mid point to center of circle
            Dim ChordToCircleLen = Math.Sqrt(Radius * Radius - ChordLength * ChordLength / 4)
            Dim ChordToCircleCos = -ChordDeltaY / ChordLength
            Dim ChordToCircleSin = ChordDeltaX / ChordLength

            If Type = ArcType.SmallClockWise OrElse Type = ArcType.LargeCounterClockWise Then
                ChordToCircleCos = -ChordToCircleCos
                ChordToCircleSin = -ChordToCircleSin
            End If

            ' circle center
            Dim CenterX = (ArcStart.X + ArcEnd.X) / 2 + ChordToCircleLen * ChordToCircleCos
            Dim CenterY = (ArcStart.Y + ArcEnd.Y) / 2 + ChordToCircleLen * ChordToCircleSin

            ' arc angle
            Dim ArcAngle = 2 * Math.Asin(ChordLength / (2 * Radius))
            If ArcAngle < 0.001 Then Throw New ApplicationException("Angle too small")
            If Type = ArcType.LargeCounterClockWise OrElse Type = ArcType.LargeClockWise Then ArcAngle = 2 * Math.PI - ArcAngle

            ' segment array
            Dim SegArray As PointD()

            ' one segment equal or less than 90 deg
            If ArcAngle < Math.PI / 2 + 0.001 Then
                Dim K1 = 4 * (1 - Math.Cos(ArcAngle / 2)) / (3 * Math.Sin(ArcAngle / 2))
                If Type = ArcType.LargeClockWise OrElse Type = ArcType.SmallClockWise Then K1 = -K1
                SegArray = New PointD(3) {}
                SegArray(0) = ArcStart
                SegArray(1) = New PointD(ArcStart.X - K1 * (ArcStart.Y - CenterY), ArcStart.Y + K1 * (ArcStart.X - CenterX))
                SegArray(2) = New PointD(ArcEnd.X + K1 * (ArcEnd.Y - CenterY), ArcEnd.Y - K1 * (ArcEnd.X - CenterX))
                SegArray(3) = ArcEnd
                Return SegArray
            End If

            ' 2, 3 or 4 segments
            Dim Segments = CInt(ArcAngle / (0.5 * Math.PI)) + 1
            Dim SegAngle = ArcAngle / Segments
            Dim K = 4 * (1 - Math.Cos(SegAngle / 2)) / (3 * Math.Sin(SegAngle / 2))

            If Type = ArcType.LargeClockWise OrElse Type = ArcType.SmallClockWise Then
                K = -K
                SegAngle = -SegAngle
            End If

            ' segments array
            SegArray = New PointD(3 * Segments + 1 - 1) {}
            SegArray(0) = New PointD(ArcStart.X, ArcStart.Y)

            ' angle from cricle center to start point
            Dim SegStartX = ArcStart.X
            Dim SegStartY = ArcStart.Y
            Dim StartAngle = Math.Atan2(ArcStart.Y - CenterY, ArcStart.X - CenterX)

            ' process all segments
            Dim SegEnd = SegArray.Length

            For Seg = 1 To SegEnd - 1 Step 3
                Dim EndAngle = StartAngle + SegAngle
                Dim SegEndX = CenterX + Radius * Math.Cos(EndAngle)
                Dim SegEndY = CenterY + Radius * Math.Sin(EndAngle)
                SegArray(Seg) = New PointD(SegStartX - K * (SegStartY - CenterY), SegStartY + K * (SegStartX - CenterX))
                SegArray(Seg + 1) = New PointD(SegEndX + K * (SegEndY - CenterY), SegEndY - K * (SegEndX - CenterX))
                SegArray(Seg + 2) = New PointD(SegEndX, SegEndY)
                SegStartX = SegEndX
                SegStartY = SegEndY
                StartAngle = EndAngle
            Next

            Return SegArray
        End Function
    End Module
End Namespace
