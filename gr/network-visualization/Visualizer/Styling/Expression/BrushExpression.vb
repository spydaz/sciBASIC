﻿
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS.Parser
Imports Microsoft.VisualBasic.Scripting.Runtime

Namespace Styling

    Module BrushExpression

        ''' <summary>
        ''' 通过这个函数来进行颜色填充以及图案填充的画笔对象的统一构建
        ''' </summary>
        ''' <param name="expression">
        ''' #### 使用图片作为填充材质
        ''' 
        ''' + url(filepath/data uri) 所有节点都使用统一的图案
        ''' + map(propertyName, val1=url1, val2=url2, val3=url3)  离散映射
        ''' + rgb(x,x,x,x)|#xxxxx|xxxxx 颜色表达式，所有的节点都使用相同的颜色
        ''' + map(propertyName, val1=color1, val2=color2) 离散映射
        ''' + map(propertyName, [patternName, n]) 区间映射
        ''' </param>
        ''' <returns></returns>
        Public Function Evaluate(expression As String) As GetBrush
            If UrlEvaluator.IsURLPattern(expression) Then
                Return expression.unifyImage
            ElseIf IsMapExpression(expression) Then
                ' 可能是离散图案映射，颜色映射
                Return expression.brushMapper
            ElseIf expression.IsColorExpression Then
                ' 可能是使用相同的颜色
                Return expression.unifyColor
            Else
                ' passthrough映射，即expression为属性名称，属性值就是颜色值
                Return expression.passthroughMapper
            End If
        End Function

        ''' <summary>
        ''' 只能够映射颜色
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        <Extension>
        Private Function passthroughMapper(expression As String) As GetBrush
            ' 使用单词进行直接映射
            Dim selector = expression.SelectNodeValue
            Dim color As Color
            Dim brush As Brush

            Return Iterator Function(nodes)
                       For Each n As Node In nodes
                           color = CStrSafe(selector(n)).TranslateColor
                           brush = New SolidBrush(color)

                           Yield New Map(Of Node, Brush) With {
                               .Key = n,
                               .Maps = brush
                           }
                       Next
                   End Function
        End Function

        ''' <summary>
        ''' 在这里处理图案映射和颜色映射，对于图案映射而言，是没有区间映射的
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        <Extension>
        Private Function brushMapper(expression As String) As GetBrush
            Dim model As MapExpression = expression.MapExpressionParser

            If model.type = MapperTypes.Continuous Then
                ' 区间映射只能够是颜色映射了
                Return model.colorRangeMapper
            Else
                ' 颜色和图案都可以具有离散映射
                Return model.discreteMapper
            End If
        End Function

        ''' <summary>
        ''' 在离散映射之中，颜色和图案可能会进行混用
        ''' </summary>
        ''' <param name="model"></param>
        ''' <returns></returns>
        <Extension>
        Private Function discreteMapper(model As MapExpression) As GetBrush
            Dim brushList As New Dictionary(Of String, Brush)
            Dim brush As Brush
            Dim selector = model.propertyName.SelectNodeValue

            For Each val As NamedValue(Of String) In model.values.Select(Function(s) s.GetTagValue("=", trim:=True))
                If val.Value.IsColorExpression Then
                    brush = New SolidBrush(val.Value.TranslateColor)
                Else
                    brush = New TextureBrush(UrlEvaluator.EvaluateAsImage(val.Value))
                End If

                brushList.Add(val.Name, brush)
            Next

            Return Iterator Function(nodes)
                       Dim key As String

                       For Each node As Node In nodes
                           key = CStrSafe(selector(node))
                           brush = brushList.TryGetValue(key, default:=Brushes.Black)

                           Yield New Map(Of Node, Brush) With {
                               .Key = node,
                               .Maps = brush
                           }
                       Next
                   End Function
        End Function

        <Extension>
        Private Function colorRangeMapper(model As MapExpression) As GetBrush
            Dim patternName$ = model.values(0)
            Dim n% = model.values(1)
            Dim colors As Brush() = Designer _
                .GetColors(patternName, n) _
                .Select(Function(c) DirectCast(New SolidBrush(c), Brush)) _
                .ToArray
            Dim range As DoubleRange = {0, n - 1}
            Dim selector = model.propertyName.SelectNodeValue
            Dim getValue = Function(node As Node) Val(selector(node))

            Return Iterator Function(nodes)
                       ' {value, index}
                       Dim nodesArray = nodes.ToArray
                       Dim nodeValues = nodesArray.RangeTransform(getValue, range)
                       Dim index%
                       Dim brush As Brush

                       For i As Integer = 0 To nodesArray.Length - 1
                           index = CInt(nodeValues(i).Maps)
                           brush = colors(index)

                           Yield New Map(Of Node, Brush) With {
                               .Key = nodesArray(i),
                               .Maps = brush
                           }
                       Next
                   End Function
        End Function

        ''' <summary>
        ''' 全部都使用统一的颜色进行填充
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Private Function unifyColor(expression As String) As GetBrush
            Dim color As Color = expression.TranslateColor
            Dim brush As New SolidBrush(color)

            Return Iterator Function(nodes As IEnumerable(Of Node)) As IEnumerable(Of Map(Of Node, Brush))
                       For Each n As Node In nodes
                           Yield New Map(Of Node, Brush) With {
                               .Key = n,
                               .Maps = brush
                           }
                       Next
                   End Function
        End Function

        ''' <summary>
        ''' 全部都使用统一的图案
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Private Function unifyImage(expression As String) As GetBrush
            Dim image As Image = UrlEvaluator.EvaluateAsImage(expression)
            Dim brush As New TextureBrush(image)

            Return Iterator Function(nodes As IEnumerable(Of Node)) As IEnumerable(Of Map(Of Node, Brush))
                       For Each n As Node In nodes
                           Yield New Map(Of Node, Brush) With {
                               .Key = n,
                               .Maps = brush
                           }
                       Next
                   End Function
        End Function
    End Module
End Namespace