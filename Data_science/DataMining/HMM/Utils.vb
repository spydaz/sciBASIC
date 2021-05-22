﻿#Region "Microsoft.VisualBasic::b260dbf3893d21e8948055e54e23228c, Data_science\DataMining\HMM\Utils.vb"

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

    ' Module Utils
    ' 
    '     Function: findSequence, gamma, xi
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.DataMining.HiddenMarkovChain.Models
Imports Microsoft.VisualBasic.My.JavaScript

Module Utils

    <Extension>
    Public Function findSequence(sequence As Chain, states As StatesObject()) As IEnumerable(Of Integer)
        Return sequence.obSequence _
            .reduce(Function(all, curr)
                        all.Add(states.findIndex(Function(x) sequence.equalsTo(x.state, curr)))
                        Return all
                    End Function, New List(Of Integer))
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function gamma(alpha#, beta#, forward#) As Double
        Return (alpha * beta) / forward
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function xi(alpha#, trans#, emiss#, beta#, forward#) As Double
        Return (alpha * trans * emiss * beta) / forward
    End Function
End Module


