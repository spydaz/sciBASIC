﻿#Region "Microsoft.VisualBasic::f087e1ad425d0328d441e37a262a51b0, sciBASIC#\gr\Microsoft.VisualBasic.Imaging\My Project\Resources.Designer.vb"

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

    '   Total Lines: 377
    '    Code Lines: 165
    ' Comment Lines: 178
    '   Blank Lines: 34
    '     File Size: 31.38 KB


    '     Module Resources
    ' 
    '         Properties: colorbrewer, Culture, Default_Aspect, Default_Blue, Default_Blue2
    '                     Default_BlueGreen, Default_BlueWarm, Default_GrayScale, Default_Green, Default_GreenYellow
    '                     Default_Marquee, Default_Median, Default_Office, Default_Office2007_2010, Default_Orange
    '                     Default_OrangeRed, Default_Paper, Default_Red, Default_RedOrange, Default_RedViolet
    '                     Default_Slipstream, Default_Violet, Default_Violet2, Default_Yellow, Default_YellowOrange
    '                     DefaultTexture, designer_colors, ResourceManager
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Microsoft.VisualBasic.Imaging.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property colorbrewer() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("colorbrewer", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Aspect&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;323232&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E3DED1&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;F07F09&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;9F2936&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;1B587C&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srg [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Aspect() As String
            Get
                Return ResourceManager.GetString("Default_Aspect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Blue&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;17406D&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;DBEFF9&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;0F6FC6&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;009DD9&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;0BD0D9&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgbC [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Blue() As String
            Get
                Return ResourceManager.GetString("Default_Blue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Blue2&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;335B74&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;DFE3E5&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;1CADE4&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;2683C6&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;27CED7&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgb [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Blue2() As String
            Get
                Return ResourceManager.GetString("Default_Blue2", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_BlueGreen&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;373545&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;CEDBE6&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;3494BA&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;58B6C0&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;75BDA7&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a: [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_BlueGreen() As String
            Get
                Return ResourceManager.GetString("Default_BlueGreen", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_BlueWarm&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;242852&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;ACCBF9&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;4A66AC&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;629DD1&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;297FD5&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:s [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_BlueWarm() As String
            Get
                Return ResourceManager.GetString("Default_BlueWarm", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_GrayScale&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;000000&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;F8F8F8&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;DDDDDD&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;B2B2B2&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;969696&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a: [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_GrayScale() As String
            Get
                Return ResourceManager.GetString("Default_GrayScale", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Green&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;455F51&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E3DED1&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;549E39&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;8AB833&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;C0CF3A&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgb [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Green() As String
            Get
                Return ResourceManager.GetString("Default_Green", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_GreenYellow&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;455F51&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E2DFCC&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;99CB38&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;63A537&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;37A76F&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt; [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_GreenYellow() As String
            Get
                Return ResourceManager.GetString("Default_GreenYellow", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Marquee&quot;&gt;&lt;a:dk1&gt;&lt;a:srgbClr val=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;5E5E5E&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;DDDDDD&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;418AB3&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;A6B727&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;F69200&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgbClr val=&quot;838383&quot;/&gt; [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Marquee() As String
            Get
                Return ResourceManager.GetString("Default_Marquee", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Median&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;775F55&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;EBDDC3&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;94B6D2&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;DD8047&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;A5AB81&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srg [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Median() As String
            Get
                Return ResourceManager.GetString("Default_Median", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Office&quot;&gt;
        '''  &lt;a:dk1&gt;
        '''    &lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;
        '''  &lt;/a:dk1&gt;
        '''  &lt;a:lt1&gt;
        '''    &lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;
        '''  &lt;/a:lt1&gt;
        '''  &lt;a:dk2&gt;
        '''    &lt;a:srgbClr val=&quot;44546A&quot;/&gt;
        '''  &lt;/a:dk2&gt;
        '''  &lt;a:lt2&gt;
        '''    &lt;a:srgbClr val=&quot;E7E6E6&quot;/&gt;
        '''  &lt;/a:lt2&gt;
        '''  &lt;a:accent1&gt;
        '''    &lt;a:srgbClr val=&quot;5B9BD5&quot;/&gt;
        '''  &lt;/a:accent1&gt;
        '''  &lt;a:accent2&gt;
        '''    &lt;a:srgbClr val=&quot;ED7D31&quot;/&gt;
        '''  &lt;/a:accent2 [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Office() As String
            Get
                Return ResourceManager.GetString("Default_Office", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Office2007-2010&quot;&gt;
        '''  &lt;a:dk1&gt;
        '''    &lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;
        '''  &lt;/a:dk1&gt;
        '''  &lt;a:lt1&gt;
        '''    &lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;
        '''  &lt;/a:lt1&gt;
        '''  &lt;a:dk2&gt;
        '''    &lt;a:srgbClr val=&quot;1F497D&quot;/&gt;
        '''  &lt;/a:dk2&gt;
        '''  &lt;a:lt2&gt;
        '''    &lt;a:srgbClr val=&quot;EEECE1&quot;/&gt;
        '''  &lt;/a:lt2&gt;
        '''  &lt;a:accent1&gt;
        '''    &lt;a:srgbClr val=&quot;4F81BD&quot;/&gt;
        '''  &lt;/a:accent1&gt;
        '''  &lt;a:accent2&gt;
        '''    &lt;a:srgbClr val=&quot;C0504D&quot;/&gt;
        '''  &lt;/ [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Office2007_2010() As String
            Get
                Return ResourceManager.GetString("Default_Office2007_2010", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Orange&quot;&gt;&lt;a:dk1&gt;&lt;a:srgbClr val=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;637052&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;CCDDEA&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;E48312&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;BD582C&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;865640&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgbClr val=&quot;9B8357&quot;/&gt;&lt; [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Orange() As String
            Get
                Return ResourceManager.GetString("Default_Orange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_OrangeRed&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;696464&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E9E5DC&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;D34817&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;9B2D1F&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;A28E6A&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a: [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_OrangeRed() As String
            Get
                Return ResourceManager.GetString("Default_OrangeRed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Paper&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;444D26&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;FEFAC9&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;A5B592&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;F3A447&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;E7BC29&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgb [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Paper() As String
            Get
                Return ResourceManager.GetString("Default_Paper", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Red&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;323232&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E5C243&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;A5300F&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;D55816&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;E19825&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srgbCl [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Red() As String
            Get
                Return ResourceManager.GetString("Default_Red", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_RedOrange&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;505046&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;EEECE1&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;E84C22&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;FFBD47&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;B64926&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a: [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_RedOrange() As String
            Get
                Return ResourceManager.GetString("Default_RedOrange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_RedViolet&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;454551&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;D8D9DC&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;E32D91&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;C830CC&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;4EA6DC&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a: [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_RedViolet() As String
            Get
                Return ResourceManager.GetString("Default_RedViolet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Slipstream&quot;&gt;
        '''  &lt;a:dk1&gt;
        '''    &lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;
        '''  &lt;/a:dk1&gt;
        '''  &lt;a:lt1&gt;
        '''    &lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;
        '''  &lt;/a:lt1&gt;
        '''  &lt;a:dk2&gt;
        '''    &lt;a:srgbClr val=&quot;212745&quot;/&gt;
        '''  &lt;/a:dk2&gt;
        '''  &lt;a:lt2&gt;
        '''    &lt;a:srgbClr val=&quot;B4DCFA&quot;/&gt;
        '''  &lt;/a:lt2&gt;
        '''  &lt;a:accent1&gt;
        '''    &lt;a:srgbClr val=&quot;4E67C8&quot;/&gt;
        '''  &lt;/a:accent1&gt;
        '''  &lt;a:accent2&gt;
        '''    &lt;a:srgbClr val=&quot;5ECCF3&quot;/&gt;
        '''  &lt;/a:acc [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Slipstream() As String
            Get
                Return ResourceManager.GetString("Default_Slipstream", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Violet&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;373545&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;DCD8DC&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;AD84C6&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;8784C7&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;5D739A&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srg [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Violet() As String
            Get
                Return ResourceManager.GetString("Default_Violet", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Violet2&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;632E62&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;EAE5EB&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;92278F&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;9B57D3&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;755DD9&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:sr [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Violet2() As String
            Get
                Return ResourceManager.GetString("Default_Violet2", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_Yellow&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;39302A&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;E5DEDB&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;FFCA08&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;F8931D&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;CE8D3E&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt;&lt;a:srg [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_Yellow() As String
            Get
                Return ResourceManager.GetString("Default_Yellow", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        '''&lt;a:clrScheme xmlns:a=&quot;http://schemas.openxmlformats.org/drawingml/2006/main&quot; name=&quot;Default_YellowOrange&quot;&gt;&lt;a:dk1&gt;&lt;a:sysClr val=&quot;windowText&quot; lastClr=&quot;000000&quot;/&gt;&lt;/a:dk1&gt;&lt;a:lt1&gt;&lt;a:sysClr val=&quot;window&quot; lastClr=&quot;FFFFFF&quot;/&gt;&lt;/a:lt1&gt;&lt;a:dk2&gt;&lt;a:srgbClr val=&quot;4E3B30&quot;/&gt;&lt;/a:dk2&gt;&lt;a:lt2&gt;&lt;a:srgbClr val=&quot;FBEEC9&quot;/&gt;&lt;/a:lt2&gt;&lt;a:accent1&gt;&lt;a:srgbClr val=&quot;F0A22E&quot;/&gt;&lt;/a:accent1&gt;&lt;a:accent2&gt;&lt;a:srgbClr val=&quot;A5644E&quot;/&gt;&lt;/a:accent2&gt;&lt;a:accent3&gt;&lt;a:srgbClr val=&quot;B58B80&quot;/&gt;&lt;/a:accent3&gt;&lt;a:accent4&gt; [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Default_YellowOrange() As String
            Get
                Return ResourceManager.GetString("Default_YellowOrange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property DefaultTexture() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("DefaultTexture", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property designer_colors() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("designer_colors", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
    End Module
End Namespace
