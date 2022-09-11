﻿#Region "Microsoft.VisualBasic::5fa080914fb7045953b17c401ce9aa69, sciBASIC#\Microsoft.VisualBasic.Core\src\CommandLine\InteropService\Pipeline\RunSlavePipeline.vb"

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

    '   Total Lines: 81
    '    Code Lines: 61
    ' Comment Lines: 2
    '   Blank Lines: 18
    '     File Size: 2.70 KB


    '     Class RunSlavePipeline
    ' 
    '         Properties: CommandLine
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: Run, ToString
    ' 
    '         Sub: HookProgress, ProcessMessage, SendMessage, SendProgress
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace CommandLine.InteropService.Pipeline

    Public Class RunSlavePipeline

        Public Event SetProgress(percentage As Integer, details As String)
        Public Event SetMessage(message As String)
        Public Event Finish(exitCode As Integer)

        ReadOnly app As String
        ReadOnly arguments As String
        ReadOnly workdir As String

        Public ReadOnly Property CommandLine As String
            Get
                Return $"{app} {arguments}"
            End Get
        End Property

        Sub New(app$, arguments$, Optional workdir As String = Nothing)
            Me.app = app
            Me.arguments = arguments
            Me.workdir = workdir
        End Sub

        Public Function Run() As Integer
            Dim code As Integer = PipelineProcess.ExecSub(
                app:=app,
                args:=arguments,
                onReadLine:=AddressOf ProcessMessage,
                workdir:=workdir
            )

            RaiseEvent Finish(code)

            Return code
        End Function

        Public Overrides Function ToString() As String
            Return CommandLine
        End Function

        Private Sub ProcessMessage(line As String)
            If line.StringEmpty Then
                Return
            End If

            If line.StartsWith("[SET_MESSAGE]") Then
                ' [SET_MESSAGE] message text
                RaiseEvent SetMessage(line.GetTagValue(" ", trim:=True).Value)
            ElseIf line.StartsWith("[SET_PROGRESS]") Then
                ' [SET_PROGRESS] percentage message text
                Dim data = line.GetTagValue(" ", trim:=True).Value.GetTagValue(" ", trim:=True)
                Dim percentage As Double = Val(data.Name)
                Dim message As String = data.Value

                RaiseEvent SetProgress(percentage, message)
            End If
        End Sub

        Public Shared Sub SendMessage(message As String)
            Call VBDebugger.WaitOutput()
            Call Console.WriteLine($"[SET_MESSAGE] {message}")
        End Sub

        Shared m_hookProgress As SetProgressEventHandler

        Public Shared Sub HookProgress(progress As SetProgressEventHandler)
            m_hookProgress = progress
        End Sub

        Public Shared Sub SendProgress(percentage As Double, message As String)
            Call VBDebugger.WaitOutput()
            Call Console.WriteLine($"[SET_PROGRESS] {percentage} {message}")

            If Not m_hookProgress Is Nothing Then
                Call m_hookProgress(percentage, message)
            End If
        End Sub

    End Class
End Namespace
