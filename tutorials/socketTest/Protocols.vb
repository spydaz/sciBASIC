﻿#Region "Microsoft.VisualBasic::da0fe537e502eaa234a58447f83a8cce, sciBASIC#\tutorials\socketTest\Protocols.vb"

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

    '   Total Lines: 12
    '    Code Lines: 9
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 249.00 B


    ' Module Protocols
    ' 
    ' 
    '     Enum Test
    ' 
    '         A, B, C
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: EntryPoint
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Net.Protocols.Reflection

Module Protocols

    Public Enum Test
        A
        B
        C
    End Enum

    Public ReadOnly Property EntryPoint As Long = New Protocol(GetType(Test)).EntryPoint
End Module
