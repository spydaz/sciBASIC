﻿#Region "Microsoft.VisualBasic::8752f65dddaea2de0ffac65db6a7b855, sciBASIC#\Data_science\Graph\Model\GridNetwork\IPoint2D.vb"

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

'   Total Lines: 6
'    Code Lines: 4
' Comment Lines: 0
'   Blank Lines: 2
'     File Size: 116 B


' Interface IPoint2D
' 
'     Properties: X, Y
' 
' /********************************************************************************/

#End Region

Namespace GridGraph

    Public Interface IPoint2D

        ReadOnly Property X As Integer
        ReadOnly Property Y As Integer

    End Interface
End Namespace