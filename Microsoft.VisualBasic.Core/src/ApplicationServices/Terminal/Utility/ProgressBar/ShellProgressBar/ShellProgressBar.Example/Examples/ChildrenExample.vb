﻿Imports System
Imports System.Threading.Tasks

Namespace ShellProgressBar.Example.Examples
    Public Class ChildrenExample
        Inherits ExampleBase
        Protected Overrides Function StartAsync() As Task
            Const totalTicks = 10
            Dim options = New ProgressBarOptions With {
    .ForegroundColor = ConsoleColor.Yellow,
    .BackgroundColor = ConsoleColor.DarkGray,
    .ProgressCharacter = "─"c
}
            Dim childOptions = New ProgressBarOptions With {
    .ForegroundColor = ConsoleColor.Green,
    .BackgroundColor = ConsoleColor.DarkGray,
    .ProgressCharacter = "─"c
}
            Dim pbar = New ProgressBar(totalTicks, "main progressbar", options)
            TickToCompletion(pbar, totalTicks, sleep:=10, childAction:=Sub(i)
                                                                           Dim child = pbar.Spawn(totalTicks, "child actions", childOptions)
                                                                           TickToCompletion(child, totalTicks, sleep:=100)
                                                                       End Sub)
            Return Task.CompletedTask
        End Function
    End Class
End Namespace
