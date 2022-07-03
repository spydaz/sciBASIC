﻿Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.FileIO.Path
Imports Microsoft.VisualBasic.Net.Protocols.ContentTypes

Namespace FileSystem

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StreamBlock : Inherits StreamObject

        Public Property offset As Long
        Public Property size As Long

        Public ReadOnly Property mimeType As ContentType
            Get
                Return referencePath.ToString.FileMimeType
            End Get
        End Property

        ''' <summary>
        ''' tostring of the <see cref="referencePath"/> object.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property fullName As String
            Get
                Return referencePath.ToString
            End Get
        End Property

        Sub New()
        End Sub

        Sub New(filepath As FilePath)
            Call MyBase.New(filepath)
        End Sub

        Public Overrides Function ToString() As String
            Return $"{MyBase.ToString} [offset={offset}, size={StringFormats.Lanudry(size)}] ({mimeType.ToString})"
        End Function

    End Class
End Namespace