﻿Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Analysis
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Serialization.JSON

Module LouvainTest

    Sub Main()

        Dim links As String()() = "E:\GCModeller\src\runtime\sciBASIC#\Data_science\algorithms\Louvain\facebook_combined.txt".ReadAllLines.Skip(1).Select(Function(str) Strings.Trim(str).StringSplit("\s+")).ToArray
        Dim g As New NetworkGraph

        ' build network via links
        For Each line As String() In links
            If g.GetElementByID(line(0)) Is Nothing Then
                g.CreateNode(line(0))
            End If
            If g.GetElementByID(line(1)) Is Nothing Then
                g.CreateNode(line(1))
            End If

            g.CreateEdge(g.GetElementByID(line(0)), g.GetElementByID(line(1)), 1)
        Next

        ' the original network with communities labeled
        Dim clusters As NetworkGraph = Communities.Analysis(g)

        Call Console.WriteLine(Communities.Modularity(clusters))
        Call Console.WriteLine(Communities.Community(g).GetJson(indent:=True))

        Call clusters _
            .Tabular _
            .Save("E:\GCModeller\src\runtime\sciBASIC#\Data_science\algorithms\Louvain\facebook_combined_graph")

        Pause()
    End Sub

    Sub RunAnalysis(filepath As String)

    End Sub

End Module