﻿#Region "Microsoft.VisualBasic::59bf99ac8b302584aeaf6579450981b0, sciBASIC#\mime\application%vnd.openxmlformats-officedocument.spreadsheetml.sheet\Excel\Model\Directory\Abstract.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 19
'    Code Lines: 14
' Comment Lines: 0
'   Blank Lines: 5
'     File Size: 489 B


'     Class Directory
' 
'         Properties: Folder
' 
'         Constructor: (+1 Overloads) Sub New
'         Function: ToString
' 
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace XLSX.Model.Directory

#If False Then
    An XLSX file is packaged using the Open Packaging Conventions (OPC/OOXML_2012, itself based on ZIP_6_2_0). 
    The package can be explored, by opening with ZIP software, typically by changing the file extension to .zip. 
    The top level of a minimal package will typically have three folders (_rels, docProps, and xl) and one file
    part ([Content_Types].xml). The xl folder holds the primary content of the document including the file part 
    workbook.xml and a worksheets folder containing a file for each worksheet, as well as other files and folders 
    that support functionality (such as controlling calculation order) and presentation (such as formatting 
    styles for cells) for the spreadsheet. Any embedded graphics are also stored in the xl folder as additional 
    parts. The other folders and parts at the top level of the package support efficient navigation and manipulation 
    of the package:

    + _rels is a Relationships folder, containing a single file .rels (which may be hidden from file listings, depending
      on operating system and settings). It lists and links to the key parts in the package, using URIs to identify the 
      type of relationship of each key part to the package. In particular it specifies a relationship to the primary 
      officeDocument (typically named /xl/workbook.xml ) and typically to parts within docProps as core and extended 
      properties.
    + docProps is a folder that contains properties for the document as a whole, typically including a set of core properties, 
      a set of extended or application-specific properties, and a thumbnail preview for the document.
    + [Content_Types].xml is a file part, a mandatory part in any OPC package, that lists the content types (using MIME 
      Internet Media Types as defined in RFC 6838) for parts within the package.
#End If

    Public MustInherit Class XlsxDirectoryPart

        Public ReadOnly Property folder As String

        Sub New(workdir$)
            folder = $"{workdir}/{_name()}"

            If Not workdir.StringEmpty Then
                Call _loadContents()
            End If
        End Sub

        Protected MustOverride Function _name() As String
        Protected MustOverride Sub _loadContents()

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Protected Function InternalFileName(name As String) As String
            Return $"{folder}/{name}".GetFullPath
        End Function

        Public Overrides Function ToString() As String
            Return folder
        End Function
    End Class
End Namespace
