﻿Imports System
Imports System.Runtime.CompilerServices

Namespace UMAP
    Friend Module ThreadSafeFastRandom
        Private ReadOnly _global As Random = New Random()
        <ThreadStatic>
        Private _local As UMAP.FastRandom

        Private Function GetGlobalSeed() As Integer
            Dim seed As Integer

            SyncLock UMAP.ThreadSafeFastRandom._global
                seed = UMAP.ThreadSafeFastRandom._global.Next()
            End SyncLock

            Return seed
        End Function

        ''' <summary>
        ''' Returns a non-negative random integer.
        ''' </summary>
        ''' <returns>A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function [Next]() As Integer
            Dim inst = UMAP.ThreadSafeFastRandom._local

            If inst Is Nothing Then
                Dim seed As Integer
                seed = UMAP.ThreadSafeFastRandom.GetGlobalSeed()
                UMAP.ThreadSafeFastRandom._local = CSharpImpl.__Assign(inst, New UMAP.FastRandom(seed))
            End If

            Return inst.Next()
        End Function

        ''' <summary>
        ''' Returns a non-negative random integer that is less than the specified maximum.
        ''' </summary>
        ''' <paramname="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        ''' <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue; that is, the range of return values ordinarily includes 0 but not maxValue. However,</returns>        '  if maxValue equals 0, maxValue is returned.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function [Next](ByVal maxValue As Integer) As Integer
            Dim inst = UMAP.ThreadSafeFastRandom._local

            If inst Is Nothing Then
                Dim seed As Integer
                seed = UMAP.ThreadSafeFastRandom.GetGlobalSeed()
                UMAP.ThreadSafeFastRandom._local = CSharpImpl.__Assign(inst, New UMAP.FastRandom(seed))
            End If

            Dim ans As Integer

            Do
                ans = inst.Next(maxValue)
            Loop While ans = maxValue

            Return ans
        End Function

        ''' <summary>
        ''' Returns a random integer that is within a specified range.
        ''' </summary>
        ''' <paramname="minValue">The inclusive lower bound of the random number returned.</param>
        ''' <paramname="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        ''' <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue</returns>        '  equals maxValue, minValue is returned.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function [Next](ByVal minValue As Integer, ByVal maxValue As Integer) As Integer
            Dim inst = UMAP.ThreadSafeFastRandom._local

            If inst Is Nothing Then
                Dim seed As Integer
                seed = UMAP.ThreadSafeFastRandom.GetGlobalSeed()
                UMAP.ThreadSafeFastRandom._local = CSharpImpl.__Assign(inst, New UMAP.FastRandom(seed))
            End If

            Return inst.Next(minValue, maxValue)
        End Function

        ''' <summary>
        ''' Generates a random float. Values returned are from 0.0 up to but not including 1.0.
        ''' </summary>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function NextFloat() As Single
            Dim inst = UMAP.ThreadSafeFastRandom._local

            If inst Is Nothing Then
                Dim seed As Integer
                seed = UMAP.ThreadSafeFastRandom.GetGlobalSeed()
                UMAP.ThreadSafeFastRandom._local = CSharpImpl.__Assign(inst, New UMAP.FastRandom(seed))
            End If

            Return inst.NextFloat()
        End Function

        ''' <summary>
        ''' Fills the elements of a specified array of bytes with random numbers.
        ''' </summary>
        ''' <paramname="buffer">An array of bytes to contain random numbers.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub NextFloats(ByVal buffer As Span(Of Single))
            Dim inst = UMAP.ThreadSafeFastRandom._local

            If inst Is Nothing Then
                Dim seed As Integer
                seed = UMAP.ThreadSafeFastRandom.GetGlobalSeed()
                UMAP.ThreadSafeFastRandom._local = CSharpImpl.__Assign(inst, New UMAP.FastRandom(seed))
            End If

            inst.NextFloats(buffer)
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Module
End Namespace
