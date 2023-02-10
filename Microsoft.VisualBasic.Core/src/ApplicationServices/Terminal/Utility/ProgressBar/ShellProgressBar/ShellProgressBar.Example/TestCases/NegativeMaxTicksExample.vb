﻿#Region "Microsoft.VisualBasic::1615bda73659afef3bc11c603dadad2d, sciBASIC#\Microsoft.VisualBasic.Core\src\ApplicationServices\Terminal\Utility\ProgressBar\ShellProgressBar\ShellProgressBar.Example\TestCases\NegativeMaxTicksExample.vb"

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
    '    Code Lines: 18
    ' Comment Lines: 0
    '   Blank Lines: 1
    '     File Size: 698 B


    '     Class NegativeMaxTicksExample
    ' 
    '         Function: Start
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports System.Threading
Imports System.Threading.Tasks

Namespace ShellProgressBar.Example.TestCases
    Public Class NegativeMaxTicksExample
        Implements IProgressBarExample
        Public Function Start(token As CancellationToken) As Task Implements IProgressBarExample.Start
            Dim ticks = -100
            Using pbar = New ProgressBar(ticks, "my operation with negative ticks", ConsoleColor.Cyan)
                For i = 0 To ticks - 1
                    pbar.Tick("step " & i)
                    Thread.Sleep(50)
                Next
            End Using
            Return Task.FromResult(1)
        End Function
    End Class
End Namespace
