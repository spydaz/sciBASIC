﻿#Region "Microsoft.VisualBasic::5012630bde3f5b220cbf4fe5af78d49d, Microsoft.VisualBasic.Core\src\Text\Xml\XmlDocumentTree.vb"

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

    '     Interface IXmlDocumentTree
    ' 
    '         Function: GetAllChilds, GetAllChildsByNodeName
    ' 
    '     Interface IXmlNode
    ' 
    '         Function: GetInnerText
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Text.Xml

    Public Interface IXmlDocumentTree : Inherits IXmlNode

        ''' <summary>
        ''' 获取当前节点内的所有直接的子节点
        ''' </summary>
        ''' <returns></returns>
        Function GetAllChilds() As IXmlNode()

        ''' <summary>
        ''' 得到当前节点内所有的目标名称的子节点
        ''' </summary>
        ''' <param name="nodename"></param>
        ''' <returns></returns>
        Function GetAllChildsByNodeName(nodename As String) As IXmlDocumentTree()

    End Interface

    ''' <summary>
    ''' includes tree node and text node
    ''' </summary>
    Public Interface IXmlNode

        ReadOnly Property nodeName As String

        Function GetInnerText() As String

    End Interface
End Namespace
