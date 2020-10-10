﻿''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'	PdfFileWriter
'	PDF File Write C# Class Library.
'
'	PdfInfo
'	PDF document information dictionary.
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


Namespace PdfFileWriter
    ''' <summary>
    ''' PDF document information dictionary
    ''' </summary>
    Public Class PdfInfo
        Inherits PdfObject
        ''' <summary>
        ''' Constructor for PdfInfo class
        ''' </summary>
        ''' <paramname="Document">Main document class</param>
        ''' <returns>PdfInfo object</returns>
        ''' <remarks>
        ''' <para>The constructor initialize the /Info dictionary with 4 key value pairs. </para>
        ''' <listtype="table">
        ''' <item><description>Creation date set to current local system date</description></item>
        ''' <item><description>Modification date set to current local system date</description></item>
        ''' <item><description>Creator is PdfFileWriter C# Class Library Version No</description></item>
        ''' <item><description>Producer is PdfFileWriter C# Class Library Version No</description></item>
        ''' </list>
        ''' </remarks>
        Public Shared Function CreatePdfInfo(ByVal Document As PdfDocument) As PdfInfo
            ' create a new default info object
            If Document.InfoObject Is Nothing Then
                ' create and add info object to trailer dictionary
                Document.InfoObject = New PdfInfo(Document)
                Document.TrailerDict.AddIndirectReference("/Info", Document.InfoObject)
            End If

            ' exit with either existing object or a new one
            Return Document.InfoObject
        End Function

        ''' <summary>
        ''' Protected constructor
        ''' </summary>
        ''' <paramname="Document">Main document object</param>
        Protected Sub New(ByVal Document As PdfDocument)
            MyBase.New(Document, ObjectType.Dictionary)
            ' set creation and modify dates
            Dim LocalTime = Date.Now
            CreationDate(LocalTime)
            ModDate(LocalTime)

            ' set creator and producer
            Creator("PdfFileWriter C# Class Library Version " & PdfDocument.RevisionNumber)
            Producer("PdfFileWriter C# Class Library Version " & PdfDocument.RevisionNumber)
            Return
        End Sub

        ''' <summary>
        ''' Sets document creation date and time
        ''' </summary>
        ''' <paramname="Date">Creation date and time</param>
        Public Sub CreationDate(ByVal [Date] As Date)
            Dictionary.AddPdfString("/CreationDate", String.Format("D:{0}", [Date].ToString("yyyyMMddHHmmss")))
            Return
        End Sub

        ''' <summary>
        ''' Sets document last modify date and time
        ''' </summary>
        ''' <paramname="Date">Modify date and time</param>
        Public Sub ModDate(ByVal [Date] As Date)
            Dictionary.AddPdfString("/ModDate", String.Format("D:{0}", [Date].ToString("yyyyMMddHHmmss")))
            Return
        End Sub

        ''' <summary>
        ''' Sets document title
        ''' </summary>
        ''' <paramname="pTitle">Title</param>
        Public Sub Title(ByVal pTitle As String)
            Dictionary.AddPdfString("/Title", pTitle)
            Return
        End Sub

        ''' <summary>
        ''' Sets document author 
        ''' </summary>
        ''' <paramname="pAuthor">Author</param>
        Public Sub Author(ByVal pAuthor As String)
            Dictionary.AddPdfString("/Author", pAuthor)
            Return
        End Sub

        ''' <summary>
        ''' Sets document subject
        ''' </summary>
        ''' <paramname="pSubject">Subject</param>
        Public Sub Subject(ByVal pSubject As String)
            Dictionary.AddPdfString("/Subject", pSubject)
            Return
        End Sub

        ''' <summary>
        ''' Sets keywords associated with the document
        ''' </summary>
        ''' <paramname="pKeywords">Keywords list</param>
        Public Sub Keywords(ByVal pKeywords As String)
            Dictionary.AddPdfString("/Keywords", pKeywords)
            Return
        End Sub

        ''' <summary>
        ''' Sets the name of the application that created the document
        ''' </summary>
        ''' <paramname="pCreator">Creator</param>
        Public Sub Creator(ByVal pCreator As String)
            Dictionary.AddPdfString("/Creator", pCreator)
            Return
        End Sub

        ''' <summary>
        ''' Sets the name of the application that produced the document
        ''' </summary>
        ''' <paramname="pProducer">Producer</param>
        Public Sub Producer(ByVal pProducer As String)
            Dictionary.AddPdfString("/Producer", pProducer)
            Return
        End Sub
    End Class
End Namespace
