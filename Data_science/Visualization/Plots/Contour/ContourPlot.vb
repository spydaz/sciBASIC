﻿Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.MarchingSquares
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace Contour

    Public Class ContourPlot : Inherits Plot

        ReadOnly contours As GeneralPath()

        Public Sub New(sample As IEnumerable(Of MeasureData), theme As Theme)
            MyBase.New(theme)

            contours = ContourLayer _
                .GetContours(sample) _
                .ToArray
        End Sub

        Sub New(sample As IEnumerable(Of GeneralPath), theme As Theme)
            MyBase.New(theme)

            contours = sample.OrderBy(Function(g) g.level).ToArray
        End Sub

        Sub New(sample As IEnumerable(Of ContourLayer), theme As Theme)
            MyBase.New(theme)

            contours = sample _
                .OrderBy(Function(layer) layer.threshold) _
                .Select(Function(layer) New GeneralPath(layer)) _
                .ToArray
        End Sub

        Protected Overrides Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)
            Dim level_cutoff As Double() = contours.Select(Function(c) c.level).ToArray
            Dim colors As SolidBrush() = Designer _
                .GetColors(theme.colorSet, level_cutoff.Length) _
                .Select(Function(c) New SolidBrush(c)) _
                .ToArray
            Dim i As i32 = Scan0
            Dim dims = contours(Scan0).dimension
            Dim rect = canvas.PlotRegion
            Dim scaleX = d3js.scale.linear.domain(New Double() {0, dims.Width}).range(New Double() {rect.Left, rect.Right})
            Dim scaleY = d3js.scale.linear(True).domain(New Double() {0, dims.Height}).range(New Double() {rect.Top, rect.Bottom})

            For Each polygon As GeneralPath In contours
                Dim color As SolidBrush = colors(++i)

                Call polygon.Fill(g, color, scaleX, scaleY)
                Call polygon.Draw(g, Pens.Black, scaleX, scaleY)
            Next

            Dim layout As New Rectangle(rect.Right + 10, rect.Top, canvas.Padding.Right / 3 * 2, rect.Height / 3 * 2)
            Dim legendTitleFont As Font = CSSFont.TryParse(theme.legendTitleCSS)
            Dim tickFont As Font = CSSFont.TryParse(theme.legendTickCSS)
            Dim tickStroke As Pen = Stroke.TryParse(theme.legendTickAxisStroke)

            Call g.ColorMapLegend(layout, colors, level_cutoff, legendTitleFont, title:=legendTitle, tickFont, tickStroke)
        End Sub
    End Class
End Namespace