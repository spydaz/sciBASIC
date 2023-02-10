﻿#Region "Microsoft.VisualBasic::b754aeb6cf72d6c69ce7aefe287edd35, sciBASIC#\Microsoft.VisualBasic.Core\src\ApplicationServices\Terminal\Utility\ProgressBar\ShellProgressBar\ShellProgressBar.Example\TestCases\UpdatesMaxTicksExample.vb"

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

    '   Total Lines: 25
    '    Code Lines: 24
    ' Comment Lines: 0
    '   Blank Lines: 1
    '     File Size: 935 B


    '     Class UpdatesMaxTicksExample
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
    Public Class UpdatesMaxTicksExample
        Implements IProgressBarExample
        Public Function Start(token As CancellationToken) As Task Implements IProgressBarExample.Start
            Dim ticks = 10
            Using pbar = New ProgressBar(ticks, "My operation that updates maxTicks", ConsoleColor.Cyan)
                Dim sleep = 1000
                For i = 0 To ticks - 1
                    pbar.Tick("Updating maximum ticks " & i)
                    If i = 5 Then
                        ticks = 120
                        pbar.MaxTicks = ticks
                        sleep = 50
                    End If
                    Thread.Sleep(sleep)
                Next
            End Using
            Return Task.FromResult(1)
        End Function
    End Class
End Namespace
