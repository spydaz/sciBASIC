﻿''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'	PdfFileWriter
'	PDF File Write C# Class Library.
'
'	PdfAxialShading
'	PDF Axial shading indirect object.
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

Imports SysMedia = System.Windows.Media

Namespace PdfFileWriter
    ''' <summary>
    ''' Mapping mode for axial and radial shading
    ''' </summary>
    Public Enum MappingMode
        ''' <summary>
        ''' Relative to bounding box
        ''' </summary>
        Relative
        ''' <summary>
        ''' Absolute
        ''' </summary>
        Absolute
    End Enum

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' <summary>
    ''' PDF axial shading resource class
    ''' </summary>
    ''' <remarks>
    ''' Derived class from PdfObject
    ''' </remarks>
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class PdfAxialShading
        Inherits PdfObject

        Private BBoxLeft As Double
        Private BBoxBottom As Double
        Private BBoxRight As Double
        Private BBoxTop As Double
        Private Mapping As MappingMode
        Private StartPointX As Double
        Private StartPointY As Double
        Private EndPointX As Double
        Private EndPointY As Double
        Private ExtendShadingBefore As Boolean = True
        Private ExtendShadingAfter As Boolean = True

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''' <summary>
        ''' PDF axial shading constructor.
        ''' </summary>
        ''' <paramname="Document">Parent PDF document object</param>
        ''' <paramname="BBoxLeft">Bounding box left position</param>
        ''' <paramname="BBoxBottom">Bounding box bottom position</param>
        ''' <paramname="BBoxWidth">Bounding box width</param>
        ''' <paramname="BBoxHeight">Bounding box height</param>
        ''' <paramname="ShadingFunction">Shading function</param>
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Sub New(ByVal Document As PdfDocument, ByVal BBoxLeft As Double, ByVal BBoxBottom As Double, ByVal BBoxWidth As Double, ByVal BBoxHeight As Double, ByVal ShadingFunction As PdfShadingFunction)
            MyBase.New(Document)
            ' create resource code
            ResourceCode = Document.GenerateResourceNumber("S"c)

            ' color space red, green and blue
            Dictionary.Add("/ColorSpace", "/DeviceRGB")

            ' shading type axial
            Dictionary.Add("/ShadingType", "2")

            ' add shading function to shading dictionary
            Dictionary.AddIndirectReference("/Function", ShadingFunction)

            ' bounding box
            Me.BBoxLeft = BBoxLeft
            Me.BBoxBottom = BBoxBottom
            BBoxRight = BBoxLeft + BBoxWidth
            BBoxTop = BBoxBottom + BBoxHeight

            ' assume the direction of color change is along x axis
            Mapping = MappingMode.Relative
            StartPointX = 0.0
            StartPointY = 0.0
            EndPointX = 1.0
            EndPointY = 0.0
            Return
        End Sub

        ''' <summary>
        ''' PDF axial shading constructor for unit bounding box
        ''' </summary>
        ''' <paramname="Document">Parent PDF document object</param>
        ''' <paramname="ShadingFunction">Shading function</param>
        Public Sub New(ByVal Document As PdfDocument, ByVal ShadingFunction As PdfShadingFunction)
            Me.New(Document, 0.0, 0.0, 1.0, 1.0, ShadingFunction)
        End Sub

        ''' <summary>
        ''' PDF axial shading constructor for unit bounding box
        ''' </summary>
        ''' <paramname="Document">Parent PDF document object</param>
        ''' <paramname="MediaBrush">System.Windows.Media brush</param>
        Public Sub New(ByVal Document As PdfDocument, ByVal MediaBrush As SysMedia.LinearGradientBrush)
            Me.New(Document, 0.0, 0.0, 1.0, 1.0, New PdfShadingFunction(Document, MediaBrush))
            SetAxisDirection(MediaBrush.StartPoint.X, MediaBrush.StartPoint.Y, MediaBrush.EndPoint.X, MediaBrush.EndPoint.Y, If(MediaBrush.MappingMode = SysMedia.BrushMappingMode.RelativeToBoundingBox, MappingMode.Relative, MappingMode.Absolute))
            Return
        End Sub

        ''' <summary>
        ''' Set bounding box
        ''' </summary>
        ''' <paramname="BBoxLeft">Bounding box left</param>
        ''' <paramname="BBoxBottom">Bounding box bottom</param>
        ''' <paramname="BBoxWidth">Bounding box width</param>
        ''' <paramname="BBoxHeight">Bounding box height</param>
        Public Sub SetBoundingBox(ByVal BBoxLeft As Double, ByVal BBoxBottom As Double, ByVal BBoxWidth As Double, ByVal BBoxHeight As Double)
            ' bounding box
            Me.BBoxLeft = BBoxLeft
            Me.BBoxBottom = BBoxBottom
            BBoxRight = BBoxLeft + BBoxWidth
            BBoxTop = BBoxBottom + BBoxHeight
            Return
        End Sub

        ''' <summary>
        ''' Set gradient axis direction
        ''' </summary>
        ''' <paramname="StartPointX">Start point x</param>
        ''' <paramname="StartPointY">Start point y</param>
        ''' <paramname="EndPointX">End point x</param>
        ''' <paramname="EndPointY">End point y</param>
        ''' <paramname="Mapping">Mapping mode (Relative or Absolute)</param>
        Public Sub SetAxisDirection(ByVal StartPointX As Double, ByVal StartPointY As Double, ByVal EndPointX As Double, ByVal EndPointY As Double, ByVal Mapping As MappingMode)
            Me.StartPointX = StartPointX
            Me.StartPointY = StartPointY
            Me.EndPointX = EndPointX
            Me.EndPointY = EndPointY
            Me.Mapping = Mapping
            Return
        End Sub

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''' <summary>
        ''' Sets anti-alias parameter
        ''' </summary>
        ''' <paramname="Value">Anti-alias true or false</param>
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Sub AntiAlias(ByVal Value As Boolean)
            Dictionary.AddBoolean("/AntiAlias", Value)
            Return
        End Sub

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''' <summary>
        ''' Extend shading beyond axis
        ''' </summary>
        ''' <paramname="Before">Before (true or false)</param>
        ''' <paramname="After">After (true or false)</param>
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Sub ExtendShading(ByVal Before As Boolean, ByVal After As Boolean)
            ExtendShadingBefore = Before
            ExtendShadingAfter = After
            Return
        End Sub

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Write object to PDF file
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Friend Overrides Sub WriteObjectToPdfFile()
            ' bounding box
            Dictionary.AddRectangle("/BBox", BBoxLeft, BBoxBottom, BBoxRight, BBoxTop)

            ' absolute axis direction
            If Mapping = MappingMode.Absolute Then
                ' relative axit direction
                Dictionary.AddRectangle("/Coords", StartPointX, StartPointY, EndPointX, EndPointY)
            Else
                Dim RelStartPointX = BBoxLeft * (1.0 - StartPointX) + BBoxRight * StartPointX
                Dim RelStartPointY = BBoxBottom * (1.0 - StartPointY) + BBoxTop * StartPointY
                Dim RelEndPointX = BBoxLeft * (1.0 - EndPointX) + BBoxRight * EndPointX
                Dim RelEndPointY = BBoxBottom * (1.0 - EndPointY) + BBoxTop * EndPointY
                Dictionary.AddRectangle("/Coords", RelStartPointX, RelStartPointY, RelEndPointX, RelEndPointY)
            End If

            ' extend shading
            Dictionary.AddFormat("/Extend", "[{0} {1}]", If(ExtendShadingBefore, "true", "false"), If(ExtendShadingAfter, "true", "false"))

            ' call PdfObject base routine
            MyBase.WriteObjectToPdfFile()

            ' exit
            Return
        End Sub
    End Class
End Namespace
