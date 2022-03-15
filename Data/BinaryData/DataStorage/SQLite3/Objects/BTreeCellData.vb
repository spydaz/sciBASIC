﻿#Region "Microsoft.VisualBasic::1beaf146e17036c13e16fcba9bd80608, sciBASIC#\Data\BinaryData\DataStorage\SQLite3\Objects\BTreeCellData.vb"

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

    '   Total Lines: 13
    '    Code Lines: 9
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 363.00 B


    '     Class BTreeCellData
    ' 
    '         Properties: Cell, CellOffset, Page
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Diagnostics

Namespace ManagedSqlite.Core.Objects

    <DebuggerDisplay("Page {Page}, Size {Cell.DataSizeInCell} / {Cell.DataSize}")>
    Friend Class BTreeCellData

        Friend Property Page As UInteger
        Friend Property Cell As BTreeLeafTablePage.Cell
        Friend Property CellOffset As UShort

    End Class
End Namespace
