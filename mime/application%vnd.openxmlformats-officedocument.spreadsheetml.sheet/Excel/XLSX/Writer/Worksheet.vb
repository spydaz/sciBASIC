﻿' 
'  PicoXLSX is a small .NET library to generate XLSX (Microsoft Excel 2007 or newer) files in an easy and native way
'  Copyright Raphael Stoeckli © 2023
'  This library is licensed under the MIT License.
'  You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
' 

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports PicoXLSX.Cell

Namespace PicoXLSX

    ''' <summary>
    ''' Class representing a worksheet of a workbook
    ''' </summary>
    Public Class Worksheet
        ''' <summary>
        ''' Threshold, using when floats are compared
        ''' </summary>
        Private Const FLOAT_THRESHOLD As Single = 0.0001F

        ''' <summary>
        ''' Maximum number of characters a worksheet name can have
        ''' </summary>
        Public Shared ReadOnly MAX_WORKSHEET_NAME_LENGTH As Integer = 31

        ''' <summary>
        ''' Default column width as constant
        ''' </summary>
        Public Const DEFAULT_COLUMN_WIDTH As Single = 10F

        ''' <summary>
        ''' Default row height as constant
        ''' </summary>
        Public Const DEFAULT_ROW_HEIGHT As Single = 15F

        ''' <summary>
        ''' Maximum column number (zero-based) as constant
        ''' </summary>
        Public Const MAX_COLUMN_NUMBER As Integer = 16383

        ''' <summary>
        ''' Minimum column number (zero-based) as constant
        ''' </summary>
        Public Const MIN_COLUMN_NUMBER As Integer = 0

        ''' <summary>
        ''' Minimum column width as constant
        ''' </summary>
        Public Const MIN_COLUMN_WIDTH As Single = 0F

        ''' <summary>
        ''' Minimum row height as constant
        ''' </summary>
        Public Const MIN_ROW_HEIGHT As Single = 0F

        ''' <summary>
        ''' Maximum column width as constant
        ''' </summary>
        Public Const MAX_COLUMN_WIDTH As Single = 255F

        ''' <summary>
        ''' Maximum row number (zero-based) as constant
        ''' </summary>
        Public Const MAX_ROW_NUMBER As Integer = 1048575

        ''' <summary>
        ''' Minimum row number (zero-based) as constant
        ''' </summary>
        Public Const MIN_ROW_NUMBER As Integer = 0

        ''' <summary>
        ''' Maximum row height as constant
        ''' </summary>
        Public Const MAX_ROW_HEIGHT As Single = 409.5F

        ''' <summary>
        ''' Enum to define the direction when using AddNextCell method
        ''' </summary>
        Public Enum CellDirection
            ''' <summary>The next cell will be on the same row (A1,B1,C1...)</summary>
            ColumnToColumn
            ''' <summary>The next cell will be on the same column (A1,A2,A3...)</summary>
            RowToRow
            ''' <summary>The address of the next cell will be not changed when adding a cell (for manual definition of cell addresses)</summary>
            Disabled
        End Enum

        ''' <summary>
        ''' Enum to define the possible protection types when protecting a worksheet
        ''' </summary>
        Public Enum SheetProtectionValue
            ' sheet, // Is always on 1 if protected
            ''' <summary>If selected, the user can edit objects if the worksheets is protected</summary>
            objects
            ''' <summary>If selected, the user can edit scenarios if the worksheets is protected</summary>
            scenarios
            ''' <summary>If selected, the user can format cells if the worksheets is protected</summary>
            formatCells
            ''' <summary>If selected, the user can format columns if the worksheets is protected</summary>
            formatColumns
            ''' <summary>If selected, the user can format rows if the worksheets is protected</summary>
            formatRows
            ''' <summary>If selected, the user can insert columns if the worksheets is protected</summary>
            insertColumns
            ''' <summary>If selected, the user can insert rows if the worksheets is protected</summary>
            insertRows
            ''' <summary>If selected, the user can insert hyper links if the worksheets is protected</summary>
            insertHyperlinks
            ''' <summary>If selected, the user can delete columns if the worksheets is protected</summary>
            deleteColumns
            ''' <summary>If selected, the user can delete rows if the worksheets is protected</summary>
            deleteRows
            ''' <summary>If selected, the user can select locked cells if the worksheets is protected</summary>
            selectLockedCells
            ''' <summary>If selected, the user can sort cells if the worksheets is protected</summary>
            sort
            ''' <summary>If selected, the user can use auto filters if the worksheets is protected</summary>
            autoFilter
            ''' <summary>If selected, the user can use pivot tables if the worksheets is protected</summary>
            pivotTables
            ''' <summary>If selected, the user can select unlocked cells if the worksheets is protected</summary>
            selectUnlockedCells
        End Enum

        ''' <summary>
        ''' Enum to define the pane position or active pane in a slip worksheet
        ''' </summary>
        Public Enum WorksheetPane
            ''' <summary>The pane is located in the bottom right of the split worksheet</summary>
            bottomRight
            ''' <summary>The pane is located in the top right of the split worksheet</summary>
            topRight
            ''' <summary>The pane is located in the bottom left of the split worksheet</summary>
            bottomLeft
            ''' <summary>The pane is located in the top left of the split worksheet</summary>
            topLeft
        End Enum

        ''' <summary>
        ''' Defines the activeStyle
        ''' </summary>
        Private activeStyleField As Style

        ''' <summary>
        ''' Defines the autoFilterRange
        ''' </summary>
        Private autoFilterRangeField As Range?

        ''' <summary>
        ''' Defines the cells
        ''' </summary>
        Private ReadOnly cellsField As Dictionary(Of String, Cell)

        ''' <summary>
        ''' Defines the columns
        ''' </summary>
        Private ReadOnly columnsField As Dictionary(Of Integer, Column)

        ''' <summary>
        ''' Defines the sheetName
        ''' </summary>
        Private sheetNameField As String

        ''' <summary>
        ''' Defines the currentRowNumber
        ''' </summary>
        Private currentRowNumber As Integer

        ''' <summary>
        ''' Defines the currentColumnNumber
        ''' </summary>
        Private currentColumnNumber As Integer

        ''' <summary>
        ''' Defines the defaultRowHeight
        ''' </summary>
        Private defaultRowHeightField As Single

        ''' <summary>
        ''' Defines the defaultColumnWidth
        ''' </summary>
        Private defaultColumnWidthField As Single

        ''' <summary>
        ''' Defines the rowHeights
        ''' </summary>
        Private ReadOnly rowHeightsField As Dictionary(Of Integer, Single)

        ''' <summary>
        ''' Defines the hiddenRows
        ''' </summary>
        Private ReadOnly hiddenRowsField As Dictionary(Of Integer, Boolean)

        ''' <summary>
        ''' Defines the mergedCells
        ''' </summary>
        Private ReadOnly mergedCellsField As Dictionary(Of String, Range)

        ''' <summary>
        ''' Defines the sheetProtectionValues
        ''' </summary>
        Private ReadOnly sheetProtectionValuesField As List(Of SheetProtectionValue)

        ''' <summary>
        ''' Defines the useActiveStyle
        ''' </summary>
        Private useActiveStyle As Boolean

        ''' <summary>
        ''' Defines the hidden
        ''' </summary>
        Private hiddenField As Boolean

        ''' <summary>
        ''' Defines the workbookReference
        ''' </summary>
        Private workbookReferenceField As Workbook

        ''' <summary>
        ''' Defines the sheetProtectionPassword
        ''' </summary>
        Private sheetProtectionPasswordField As String = Nothing

        ''' <summary>
        ''' Defines the sheetProtectionPasswordHash
        ''' </summary>
        Private sheetProtectionPasswordHashField As String = Nothing

        ''' <summary>
        ''' Defines the selectedCells
        ''' </summary>
        Private selectedCellsField As List(Of Range)

        ''' <summary>
        ''' Defines the freezeSplitPanes
        ''' </summary>
        Private freezeSplitPanesField As Boolean?

        ''' <summary>
        ''' Defines the paneSplitLeftWidth
        ''' </summary>
        Private paneSplitLeftWidthField As Single?

        ''' <summary>
        ''' Defines the paneSplitTopHeight
        ''' </summary>
        Private paneSplitTopHeightField As Single?

        ''' <summary>
        ''' Defines the paneSplitTopLeftCell
        ''' </summary>
        Private paneSplitTopLeftCellField As Address?

        ''' <summary>
        ''' Defines the paneSplitAddress
        ''' </summary>
        Private paneSplitAddressField As Address?

        ''' <summary>
        ''' Defines the activePane
        ''' </summary>
        Private activePaneField As WorksheetPane?

        ''' <summary>
        ''' Defines the sheetID
        ''' </summary>
        Private sheetIDField As Integer

        ''' <summary>
        ''' Gets the range of the auto-filter. Wrapped to Nullable to provide null as value. If null, no auto-filter is applied
        ''' </summary>
        Public ReadOnly Property AutoFilterRange As Range?
            Get
                Return autoFilterRangeField
            End Get
        End Property

        ''' <summary>
        ''' Gets the cells of the worksheet as dictionary with the cell address as key and the cell object as value
        ''' </summary>
        Public ReadOnly Property Cells As Dictionary(Of String, Cell)
            Get
                Return cellsField
            End Get
        End Property

        ''' <summary>
        ''' Gets all columns with non-standard properties, like auto filter applied or a special width as dictionary with the zero-based column index as key and the column object as value
        ''' </summary>
        Public ReadOnly Property Columns As Dictionary(Of Integer, Column)
            Get
                Return columnsField
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the direction when using AddNextCell method
        ''' </summary>
        Public Property CurrentCellDirection As CellDirection

        ''' <summary>
        ''' Gets or sets the default column width
        ''' </summary>
        Public Property DefaultColumnWidth As Single
            Get
                Return defaultColumnWidthField
            End Get
            Set(ByVal value As Single)
                If value < MIN_COLUMN_WIDTH OrElse value > MAX_COLUMN_WIDTH Then
                    Throw New RangeException("OutOfRangeException", "The passed default column width is out of range (" & MIN_COLUMN_WIDTH.ToString() & " to " & MAX_COLUMN_WIDTH.ToString() & ")")
                End If
                defaultColumnWidthField = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the default Row height
        ''' </summary>
        Public Property DefaultRowHeight As Single
            Get
                Return defaultRowHeightField
            End Get
            Set(ByVal value As Single)
                If value < MIN_ROW_HEIGHT OrElse value > MAX_ROW_HEIGHT Then
                    Throw New RangeException("OutOfRangeException", "The passed default row height is out of range (" & MIN_ROW_HEIGHT.ToString() & " to " & MAX_ROW_HEIGHT.ToString() & ")")
                End If
                defaultRowHeightField = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the hidden rows as dictionary with the zero-based row number as key and a boolean as value. True indicates hidden, false visible.
        ''' </summary>
        Public ReadOnly Property HiddenRows As Dictionary(Of Integer, Boolean)
            Get
                Return hiddenRowsField
            End Get
        End Property

        ''' <summary>
        ''' Gets the merged cells (only references) as dictionary with the cell address as key and the range object as value
        ''' </summary>
        Public ReadOnly Property MergedCells As Dictionary(Of String, Range)
            Get
                Return mergedCellsField
            End Get
        End Property

        ''' <summary>
        ''' Gets defined row heights as dictionary with the zero-based row number as key and the height (float from 0 to 409.5) as value
        ''' </summary>
        Public ReadOnly Property RowHeights As Dictionary(Of Integer, Single)
            Get
                Return rowHeightsField
            End Get
        End Property

        ''' <summary>
        ''' Returns either null (if no cells are selected), or the first defined range of selected cells
        ''' </summary>
        ''' <remarks>Use <seecref="SelectedCellRanges"/> to get all defined ranges</remarks>
        <Obsolete("This method is a deprecated subset of the function SelectedCellRanges. SelectedCellRanges will get this function name in a future version. Therefore, the type will change")>
        Public ReadOnly Property SelectedCells As Range?
            Get
                If selectedCellsField.Count = 0 Then
                    Return Nothing
                Else
                    Return selectedCellsField(0)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets all ranges of selected cells of this worksheet. An empty list is returned if no cells are selected
        ''' </summary>
        Public ReadOnly Property SelectedCellRanges As List(Of Range)
            Get
                Return selectedCellsField
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the internal ID of the worksheet
        ''' </summary>
        Public Property SheetID As Integer
            Get
                Return sheetIDField
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Throw New FormatException("The ID " & value.ToString() & " is invalid. Worksheet IDs must be >0")
                End If
                sheetIDField = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the name of the worksheet
        ''' </summary>
        Public Property SheetName As String
            Get
                Return sheetNameField
            End Get
            Set(ByVal value As String)
                SetSheetName(value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the password used for sheet protection. See <seecref="SetSheetProtectionPassword"/> to set the password
        ''' </summary>
        Public ReadOnly Property SheetProtectionPassword As String
            Get
                Return sheetProtectionPasswordField
            End Get
        End Property

        ''' <summary>
        ''' gets the encrypted hash of the password, defined with <seecref="SheetProtectionPassword"/>. The value will be null, if no password is defined
        ''' </summary>
        Public ReadOnly Property SheetProtectionPasswordHash As String
            Get
                Return sheetProtectionPasswordHashField
            End Get
        End Property

        ''' <summary>
        ''' Gets the list of SheetProtectionValues. These values define the allowed actions if the worksheet is protected
        ''' </summary>
        Public ReadOnly Property SheetProtectionValues As List(Of SheetProtectionValue)
            Get
                Return sheetProtectionValuesField
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the worksheet is protected. If true, protection is enabled
        ''' </summary>
        Public Property UseSheetProtection As Boolean

        ''' <summary>
        ''' Gets or sets the Reference to the parent Workbook
        ''' </summary>
        Public Property WorkbookReference As Workbook
            Get
                Return workbookReferenceField
            End Get
            Set(ByVal value As Workbook)
                workbookReferenceField = value
                If value IsNot Nothing Then
                    workbookReferenceField.ValidateWorksheets()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the worksheet is hidden. If true, the worksheet is not listed as tab in the workbook's worksheet selection<br/>
        ''' If the worksheet is not part of a workbook, or the only one in the workbook, an exception will be thrown.<br/>
        ''' If the worksheet is the selected one, and attempted to set hidden, an exception will be thrown. Define another selected worksheet prior to this call, in this case.
        ''' </summary>
        Public Property Hidden As Boolean
            Get
                Return hiddenField
            End Get
            Set(ByVal value As Boolean)
                hiddenField = value
                If value AndAlso workbookReferenceField IsNot Nothing Then
                    workbookReferenceField.ValidateWorksheets()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the height of the upper, horizontal split pane, measured from the top of the window.<br/>
        ''' The value is nullable. If null, no horizontal split of the worksheet is applied.<br/>
        ''' The value is only applicable to split the worksheet into panes, but not to freeze them.<br/>
        ''' See also: <seecref="PaneSplitAddress"/>
        ''' </summary>
        Public ReadOnly Property PaneSplitTopHeight As Single?
            Get
                Return paneSplitTopHeightField
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the left, vertical split pane, measured from the left of the window.<br/>
        ''' The value is nullable. If null, no vertical split of the worksheet is applied<br/>
        ''' The value is only applicable to split the worksheet into panes, but not to freeze them.<br/>
        ''' See also: <seecref="PaneSplitAddress"/>
        ''' </summary>
        Public ReadOnly Property PaneSplitLeftWidth As Single?
            Get
                Return paneSplitLeftWidthField
            End Get
        End Property

        ''' <summary>
        ''' Gets the FreezeSplitPanes
        ''' Gets whether split panes are frozen.<br/>
        ''' The value is nullable. If null, no freezing is applied. This property also does not apply if <seecref="PaneSplitAddress"/> is null
        ''' </summary>
        Public ReadOnly Property FreezeSplitPanes As Boolean?
            Get
                Return freezeSplitPanesField
            End Get
        End Property

        ''' <summary>
        ''' Gets the Top Left cell address of the bottom right pane if applicable and splitting is applied.<br/>
        ''' The column is only relevant for vertical split, whereas the row component is only relevant for a horizontal split.<br/>
        ''' The value is nullable. If null, no splitting was defined.
        ''' </summary>
        Public ReadOnly Property PaneSplitTopLeftCell As Address?
            Get
                Return paneSplitTopLeftCellField
            End Get
        End Property

        ''' <summary>
        ''' Gets the split address for frozen panes or if pane split was defined in number of columns and / or rows.<br/> 
        ''' For vertical splits, only the column component is considered. For horizontal splits, only the row component is considered.<br/>
        ''' The value is nullable. If null, no frozen panes or split by columns / rows are applied to the worksheet. 
        ''' However, splitting can still be applied, if the value is defined in characters.<br/>
        ''' See also: <seecref="PaneSplitLeftWidth"/> and <seecref="PaneSplitTopHeight"/> for splitting in characters (without freezing)
        ''' </summary>
        Public ReadOnly Property PaneSplitAddress As Address?
            Get
                Return paneSplitAddressField
            End Get
        End Property

        ''' <summary>
        ''' Gets the active Pane is splitting is applied.<br/>
        ''' The value is nullable. If null, no splitting was defined
        ''' </summary>
        Public ReadOnly Property ActivePane As WorksheetPane?
            Get
                Return activePaneField
            End Get
        End Property

        ''' <summary>
        ''' Gets the active Style of the worksheet. If null, no style is defined as active
        ''' </summary>
        Public ReadOnly Property ActiveStyle As Style
            Get
                Return activeStyleField
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Worksheet"/> class
        ''' </summary>
        Public Sub New()
            CurrentCellDirection = CellDirection.ColumnToColumn
            cellsField = New Dictionary(Of String, Cell)()
            currentRowNumber = 0
            currentColumnNumber = 0
            defaultColumnWidthField = DEFAULT_COLUMN_WIDTH
            defaultRowHeightField = DEFAULT_ROW_HEIGHT
            rowHeightsField = New Dictionary(Of Integer, Single)()
            mergedCellsField = New Dictionary(Of String, Range)()
            sheetProtectionValuesField = New List(Of SheetProtectionValue)()
            hiddenRowsField = New Dictionary(Of Integer, Boolean)()
            columnsField = New Dictionary(Of Integer, Column)()
            selectedCellsField = New List(Of Range)()
            activeStyleField = Nothing
            workbookReferenceField = Nothing
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Worksheet"/> class
        ''' </summary>
        ''' <paramname="name">The name<seecref="String"/>.</param>
        Public Sub New(ByVal name As String)
            Me.New()
            SetSheetName(name)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Worksheet"/> class
        ''' </summary>
        ''' <paramname="name">Name of the worksheet.</param>
        ''' <paramname="id">ID of the worksheet (for internal use).</param>
        ''' <paramname="reference">Reference to the parent Workbook.</param>
        Public Sub New(ByVal name As String, ByVal id As Integer, ByVal reference As Workbook)
            Me.New()
            SetSheetName(name)
            SheetID = id
            workbookReferenceField = reference
        End Sub

        ''' <summary>
        ''' Adds an object to the next cell position. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        Public Sub AddNextCell(ByVal value As Object)
            AddNextCell(CastValue(value, currentColumnNumber, currentRowNumber), True, Nothing)
        End Sub

        ''' <summary>
        ''' Adds an object to the next cell position. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        ''' <paramname="style">Style object to apply on this cell.</param>
        Public Sub AddNextCell(ByVal value As Object, ByVal style As Style)
            AddNextCell(CastValue(value, currentColumnNumber, currentRowNumber), True, style)
        End Sub

        ''' <summary>
        ''' Method to insert a generic cell to the next cell position
        ''' </summary>
        ''' <paramname="cell">Cell object to insert.</param>
        ''' <paramname="incremental">If true, the address value (row or column) will be incremented, otherwise not.</param>
        ''' <paramname="style">If not null, the defined style will be applied to the cell, otherwise no style or the default style will be applied.</param>
        Private Sub AddNextCell(ByVal cell As Cell, ByVal incremental As Boolean, ByVal style As Style)
            ' date and time styles are already defined by the passed cell object
            If style IsNot Nothing OrElse activeStyleField IsNot Nothing AndAlso useActiveStyle Then

                If cell.CellStyle Is Nothing AndAlso useActiveStyle Then
                    cell.SetStyle(activeStyleField)
                ElseIf cell.CellStyle Is Nothing AndAlso style IsNot Nothing Then
                    cell.SetStyle(style)
                ElseIf cell.CellStyle IsNot Nothing AndAlso useActiveStyle Then
                    Dim mixedStyle As Style = CType(cell.CellStyle.Copy(), Style)
                    mixedStyle.Append(activeStyleField)
                    cell.SetStyle(mixedStyle)
                ElseIf cell.CellStyle IsNot Nothing AndAlso style IsNot Nothing Then
                    Dim mixedStyle As Style = CType(cell.CellStyle.Copy(), Style)
                    mixedStyle.Append(style)
                    cell.SetStyle(mixedStyle)
                End If
            End If
            Dim address = cell.CellAddress
            If cellsField.ContainsKey(address) Then
                cellsField(address) = cell
            Else
                cellsField.Add(address, cell)
            End If
            If incremental Then
                If CurrentCellDirection = CellDirection.ColumnToColumn Then
                    currentColumnNumber += 1
                ElseIf CurrentCellDirection = CellDirection.RowToRow Then
                    currentRowNumber += 1
                    ' else = disabled
                End If
            Else
                If CurrentCellDirection = CellDirection.ColumnToColumn Then
                    currentColumnNumber = cell.ColumnNumber + 1
                    currentRowNumber = cell.RowNumber
                ElseIf CurrentCellDirection = CellDirection.RowToRow Then
                    currentColumnNumber = cell.ColumnNumber
                    currentRowNumber = cell.RowNumber + 1
                End If
                ' else = Disabled
            End If
        End Sub

        ''' <summary>
        ''' Method to cast a value or align an object of the type Cell to the context of the worksheet
        ''' </summary>
        ''' <paramname="value">Unspecified value or object of the type Cell.</param>
        ''' <paramname="column">Column index.</param>
        ''' <paramname="row">Row index.</param>
        ''' <returns>Cell object.</returns>
        Private Function CastValue(ByVal value As Object, ByVal column As Integer, ByVal row As Integer) As Cell
            Dim c As Cell
            If value IsNot Nothing AndAlso value.GetType() Is GetType(Cell) Then
                c = CType(value, Cell)
                c.CellAddress2 = New Address(column, row)
            Else
                c = New Cell(value, CellType.DEFAULT, column, row)
            End If
            Return c
        End Function

        ''' <summary>
        ''' Adds an object to the defined cell address. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        Public Sub AddCell(ByVal value As Object, ByVal columnNumber As Integer, ByVal rowNumber As Integer)
            AddNextCell(CastValue(value, columnNumber, rowNumber), False, Nothing)
        End Sub

        ''' <summary>
        ''' Adds an object to the defined cell address. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        ''' <paramname="style">Style to apply on the cell.</param>
        Public Sub AddCell(ByVal value As Object, ByVal columnNumber As Integer, ByVal rowNumber As Integer, ByVal style As Style)
            AddNextCell(CastValue(value, columnNumber, rowNumber), False, style)
        End Sub

        ''' <summary>
        ''' Adds an object to the defined cell address. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        Public Sub AddCell(ByVal value As Object, ByVal address As String)
            Dim column, row As Integer
            ResolveCellCoordinate(address, column, row)
            AddCell(value, column, row)
        End Sub

        ''' <summary>
        ''' Adds an object to the defined cell address. If the type of the value does not match with one of the supported data types, it will be casted to a String. A prepared object of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="value">Unspecified value to insert.</param>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        ''' <paramname="style">Style to apply on the cell.</param>
        Public Sub AddCell(ByVal value As Object, ByVal address As String, ByVal style As Style)
            Dim column, row As Integer
            ResolveCellCoordinate(address, column, row)
            AddCell(value, column, row, style)
        End Sub

        ''' <summary>
        ''' Adds a cell formula as string to the defined cell address
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        Public Sub AddCellFormula(ByVal formula As String, ByVal address As String)
            Dim column, row As Integer
            ResolveCellCoordinate(address, column, row)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, column, row)
            AddNextCell(c, False, Nothing)
        End Sub

        ''' <summary>
        ''' Adds a cell formula as string to the defined cell address
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        ''' <paramname="style">Style to apply on the cell.</param>
        Public Sub AddCellFormula(ByVal formula As String, ByVal address As String, ByVal style As Style)
            Dim column, row As Integer
            ResolveCellCoordinate(address, column, row)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, column, row)
            AddNextCell(c, False, style)
        End Sub

        ''' <summary>
        ''' Adds a cell formula as string to the defined cell address
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        Public Sub AddCellFormula(ByVal formula As String, ByVal columnNumber As Integer, ByVal rowNumber As Integer)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, columnNumber, rowNumber)
            AddNextCell(c, False, Nothing)
        End Sub

        ''' <summary>
        ''' Adds a cell formula as string to the defined cell address
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        ''' <paramname="style">Style to apply on the cell.</param>
        Public Sub AddCellFormula(ByVal formula As String, ByVal columnNumber As Integer, ByVal rowNumber As Integer, ByVal style As Style)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, columnNumber, rowNumber)
            AddNextCell(c, False, style)
        End Sub

        ''' <summary>
        ''' Adds a formula as string to the next cell position
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        Public Sub AddNextCellFormula(ByVal formula As String)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, currentColumnNumber, currentRowNumber)
            AddNextCell(c, True, Nothing)
        End Sub

        ''' <summary>
        ''' Adds a formula as string to the next cell position
        ''' </summary>
        ''' <paramname="formula">Formula to insert.</param>
        ''' <paramname="style">Style to apply on the cell.</param>
        Public Sub AddNextCellFormula(ByVal formula As String, ByVal style As Style)
            Dim c As Cell = New Cell(formula, CellType.FORMULA, currentColumnNumber, currentRowNumber)
            AddNextCell(c, True, style)
        End Sub

        ''' <summary>
        ''' Adds a list of object values to a defined cell range. If the type of the a particular value does not match with one of the supported data types, it will be casted to a String. Prepared objects of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="values">List of unspecified objects to insert.</param>
        ''' <paramname="startAddress">Start address.</param>
        ''' <paramname="endAddress">End address.</param>
        Public Sub AddCellRange(ByVal values As IReadOnlyList(Of Object), ByVal startAddress As Address, ByVal endAddress As Address)
            AddCellRangeInternal(values, startAddress, endAddress, Nothing)
        End Sub

        ''' <summary>
        ''' Adds a list of object values to a defined cell range. If the type of the a particular value does not match with one of the supported data types, it will be casted to a String. Prepared objects of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="values">List of unspecified objects to insert.</param>
        ''' <paramname="startAddress">Start address.</param>
        ''' <paramname="endAddress">End address.</param>
        ''' <paramname="style">Style to apply on the all cells of the range.</param>
        Public Sub AddCellRange(ByVal values As IReadOnlyList(Of Object), ByVal startAddress As Address, ByVal endAddress As Address, ByVal style As Style)
            AddCellRangeInternal(values, startAddress, endAddress, style)
        End Sub

        ''' <summary>
        ''' Adds a list of object values to a defined cell range. If the type of the a particular value does not match with one of the supported data types, it will be casted to a String. Prepared objects of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="values">List of unspecified objects to insert.</param>
        ''' <paramname="cellRange">Cell range as string in the format like A1:D1 or X10:X22.</param>
        Public Sub AddCellRange(ByVal values As IReadOnlyList(Of Object), ByVal cellRange As String)
            Dim range = ResolveCellRange(cellRange)
            AddCellRangeInternal(values, range.StartAddress, range.EndAddress, Nothing)
        End Sub

        ''' <summary>
        ''' Adds a list of object values to a defined cell range. If the type of the a particular value does not match with one of the supported data types, it will be casted to a String. Prepared objects of the type Cell will not be casted but adjusted
        ''' </summary>
        ''' <paramname="values">List of unspecified objects to insert.</param>
        ''' <paramname="cellRange">Cell range as string in the format like A1:D1 or X10:X22.</param>
        ''' <paramname="style">Style to apply on the all cells of the range.</param>
        Public Sub AddCellRange(ByVal values As IReadOnlyList(Of Object), ByVal cellRange As String, ByVal style As Style)
            Dim range = ResolveCellRange(cellRange)
            AddCellRangeInternal(values, range.StartAddress, range.EndAddress, style)
        End Sub

        ''' <summary>
        ''' Internal function to add a generic list of value to the defined cell range
        ''' </summary>
        ''' <typeparamname="T">Data type of the generic value list.</typeparam>
        ''' <paramname="values">List of values.</param>
        ''' <paramname="startAddress">Start address.</param>
        ''' <paramname="endAddress">End address.</param>
        ''' <paramname="style">Style to apply on the all cells of the range.</param>
        Private Sub AddCellRangeInternal(Of T)(ByVal values As IReadOnlyList(Of T), ByVal startAddress As Address, ByVal endAddress As Address, ByVal style As Style)
            Dim addresses As List(Of Address) = TryCast(GetCellRange(startAddress, endAddress), List(Of Address))
            If values.Count <> addresses.Count Then
                Throw New RangeException("OutOfRangeException", "The number of passed values (" & values.Count.ToString() & ") differs from the number of cells within the range (" & addresses.Count.ToString() & ")")
            End If
            Dim list As List(Of Cell) = TryCast(ConvertArray(values), List(Of Cell))
            Dim len = values.Count
            For i = 0 To len - 1
                list(i).RowNumber = addresses(i).Row
                list(i).ColumnNumber = addresses(i).Column
                AddNextCell(list(i), False, style)
            Next
        End Sub

        ''' <summary>
        ''' Removes a previous inserted cell at the defined address
        ''' </summary>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        ''' <returns>Returns true if the cell could be removed (existed), otherwise false (did not exist).</returns>
        Public Function RemoveCell(ByVal columnNumber As Integer, ByVal rowNumber As Integer) As Boolean
            Dim address = ResolveCellAddress(columnNumber, rowNumber)
            Return cellsField.Remove(address)
        End Function

        ''' <summary>
        ''' Removes a previous inserted cell at the defined address
        ''' </summary>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        ''' <returns>Returns true if the cell could be removed (existed), otherwise false (did not exist).</returns>
        Public Function RemoveCell(ByVal address As String) As Boolean
            Dim row, column As Integer
            ResolveCellCoordinate(address, column, row)
            Return RemoveCell(column, row)
        End Function

        ''' <summary>
        ''' Sets the passed style on the passed cell range. If cells are already existing, the style will be added or replaced
        ''' </summary>
        ''' <paramname="cellRange">Cell range to apply the style.</param>
        ''' <paramname="style">Style to apply.</param>
        Public Sub SetStyle(ByVal cellRange As Range, ByVal style As Style)
            Dim addresses As IReadOnlyList(Of Address) = cellRange.ResolveEnclosedAddresses()
            For Each address In addresses
                Dim key As String = address.GetAddress()
                If cellsField.ContainsKey(key) Then
                    If style Is Nothing Then
                        cellsField(key).RemoveStyle()
                    Else
                        cellsField(key).SetStyle(style)
                    End If
                Else
                    If style IsNot Nothing Then
                        AddCell(Nothing, address.Column, address.Row, style)
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Sets the passed style on the passed cell range, derived from a start and end address. If cells are already existing, the style will be added or replaced
        ''' Sets the passed style on the passed cell range, derived from a start and end address. If cells are already existing, the style will be added or replaced
        ''' </summary>
        ''' <paramname="startAddress">Start address of the cell range.</param>
        ''' <paramname="endAddress">End address of the cell range.</param>
        ''' <paramname="style">Style to apply or null to clear the range.</param>
        Public Sub SetStyle(ByVal startAddress As Address, ByVal endAddress As Address, ByVal style As Style)
            SetStyle(New Range(startAddress, endAddress), style)
        End Sub

        ''' <summary>
        ''' Sets the passed style on the passed (singular) cell address. If the cell is already existing, the style will be added or replaced
        ''' Sets the passed style on the passed (singular) cell address. If the cell is already existing, the style will be added or replaced
        ''' </summary>
        ''' <paramname="address">Cell address to apply the style.</param>
        ''' <paramname="style">Style to apply or null to clear the range.</param>
        Public Sub SetStyle(ByVal address As Address, ByVal style As Style)
            SetStyle(address, address, style)
        End Sub

        ''' <summary>
        ''' Sets the passed style on the passed address expression. Such an expression may be a single cell or a cell range
        ''' Sets the passed style on the passed address expression. Such an expression may be a single cell or a cell range
        ''' Sets the passed style on the passed address expression. Such an expression may be a single cell or a cell range
        ''' </summary>
        ''' <paramname="addressExpression">Expression of a cell address or range of addresses.</param>
        ''' <paramname="style">Style to apply or null to clear the range.</param>
        Public Sub SetStyle(ByVal addressExpression As String, ByVal style As Style)
            Dim scope = GetAddressScope(addressExpression)
            If scope = AddressScope.SingleAddress Then
                Dim address As Address = New Address(addressExpression)
                SetStyle(address, style)
            ElseIf scope = AddressScope.Range Then
                Dim range As Range = New Range(addressExpression)
                SetStyle(range, style)
            Else
                Throw New FormatException("The passed address'" & addressExpression & "' is neither a cell address, nor a range")
            End If
        End Sub

        ''' <summary>
        ''' Method to add allowed actions if the worksheet is protected. If one or more values are added, UseSheetProtection will be set to true
        ''' </summary>
        ''' <paramname="typeOfProtection">Allowed action on the worksheet or cells.</param>
        Public Sub AddAllowedActionOnSheetProtection(ByVal typeOfProtection As SheetProtectionValue)
            If Not sheetProtectionValuesField.Contains(typeOfProtection) Then
                If typeOfProtection = SheetProtectionValue.selectLockedCells AndAlso Not sheetProtectionValuesField.Contains(SheetProtectionValue.selectUnlockedCells) Then
                    sheetProtectionValuesField.Add(SheetProtectionValue.selectUnlockedCells)
                End If
                sheetProtectionValuesField.Add(typeOfProtection)
                UseSheetProtection = True
            End If
        End Sub

        ''' <summary>
        ''' Sets the defined column as hidden
        ''' </summary>
        ''' <paramname="columnNumber">Column number to hide on the worksheet.</param>
        Public Sub AddHiddenColumn(ByVal columnNumber As Integer)
            SetColumnHiddenState(columnNumber, True)
        End Sub

        ''' <summary>
        ''' Sets the defined column as hidden
        ''' </summary>
        ''' <paramname="columnAddress">Column address to hide on the worksheet.</param>
        Public Sub AddHiddenColumn(ByVal columnAddress As String)
            Dim columnNumber = ResolveColumn(columnAddress)
            SetColumnHiddenState(columnNumber, True)
        End Sub

        ''' <summary>
        ''' Sets the defined row as hidden
        ''' </summary>
        ''' <paramname="rowNumber">Row number to hide on the worksheet.</param>
        Public Sub AddHiddenRow(ByVal rowNumber As Integer)
            SetRowHiddenState(rowNumber, True)
        End Sub

        ''' <summary>
        ''' Clears the active style of the worksheet. All later added calls will contain no style unless another active style is set
        ''' </summary>
        Public Sub ClearActiveStyle()
            useActiveStyle = False
            activeStyleField = Nothing
        End Sub

        ''' <summary>
        ''' Gets the cell of the specified address
        ''' </summary>
        ''' <paramname="address">Address of the cell.</param>
        ''' <returns>Cell object.</returns>
        Public Function GetCell(ByVal address As Address) As Cell
            If Not cellsField.ContainsKey(address.GetAddress()) Then
                Throw New WorksheetException("The cell with the address " & address.GetAddress() & " does not exist in this worksheet")
            End If
            Return cellsField(address.GetAddress())
        End Function

        ''' <summary>
        ''' Gets the cell of the specified column and row number (zero-based)
        ''' </summary>
        ''' <paramname="columnNumber">Column number of the cell.</param>
        ''' <paramname="rowNumber">Row number of the cell.</param>
        ''' <returns>Cell object.</returns>
        Public Function GetCell(ByVal columnNumber As Integer, ByVal rowNumber As Integer) As Cell
            Return GetCell(New Address(columnNumber, rowNumber))
        End Function

        ''' <summary>
        ''' Gets whether the specified address exists in the worksheet. Existing means that a value was stored at the address
        ''' </summary>
        ''' <paramname="address">Address to check.</param>
        ''' <returns>The <seecref="Boolean"/>.</returns>
        Public Function HasCell(ByVal address As Address) As Boolean
            Return cellsField.ContainsKey(address.GetAddress())
        End Function

        ''' <summary>
        ''' Gets whether the specified address exists in the worksheet. Existing means that a value was stored at the address
        ''' </summary>
        ''' <paramname="columnNumber">Column number of the cell to check (zero-based).</param>
        ''' <paramname="rowNumber">Row number of the cell to check (zero-based).</param>
        ''' <returns>The <seecref="Boolean"/>.</returns>
        Public Function HasCell(ByVal columnNumber As Integer, ByVal rowNumber As Integer) As Boolean
            Return HasCell(New Address(columnNumber, rowNumber))
        End Function

        ''' <summary>
        ''' Resets the defined column, if existing. The corresponding instance will be removed from <seecref="Columns"/>
        ''' </summary>
        ''' <paramname="columnNumber">Column number to reset (zero-based).</param>
        Public Sub ResetColumn(ByVal columnNumber As Integer)
            If columnsField.ContainsKey(columnNumber) AndAlso Not columnsField(columnNumber).HasAutoFilter Then ' AutoFilters cannot have gaps 
                columnsField.Remove(columnNumber)
            ElseIf columnsField.ContainsKey(columnNumber) Then
                columnsField(columnNumber).IsHidden = False
                columnsField(columnNumber).Width = DEFAULT_COLUMN_WIDTH
            End If
        End Sub

        ''' <summary>
        ''' Gets the first existing column number in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based column number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetFirstColumnNumber() As Integer
            Return GetBoundaryNumber(False, True)
        End Function

        ''' <summary>
        ''' Gets the first existing column number with data in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based column number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetFirstDataColumnNumber() As Integer
            Return GetBoundaryDataNumber(False, True, True)
        End Function

        ''' <summary>
        ''' Gets the first existing row number in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based row number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetFirstRowNumber() As Integer
            Return GetBoundaryNumber(True, True)
        End Function

        ''' <summary>
        ''' Gets the first existing row number with data in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based row number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetFirstDataRowNumber() As Integer
            Return GetBoundaryDataNumber(True, True, True)
        End Function

        ''' <summary>
        ''' Gets the last existing column number in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based column number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetLastColumnNumber() As Integer
            Return GetBoundaryNumber(False, False)
        End Function

        ''' <summary>
        ''' Gets the last existing column number with data in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based column number. in case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetLastDataColumnNumber() As Integer
            Return GetBoundaryDataNumber(False, False, True)
        End Function

        ''' <summary>
        ''' Gets the last existing row number in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based row number. In case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetLastRowNumber() As Integer
            Return GetBoundaryNumber(True, False)
        End Function

        ''' <summary>
        ''' Gets the last existing row number with data in the current worksheet (zero-based)
        ''' </summary>
        ''' <returns>Zero-based row number. in case of an empty worksheet, -1 will be returned.</returns>
        Public Function GetLastDataRowNumber() As Integer
            Return GetBoundaryDataNumber(True, False, True)
        End Function

        ''' <summary>
        ''' Gets the last existing cell in the current worksheet (bottom right)
        ''' </summary>
        ''' <returns>Nullable Cell Address. If no cell address could be determined, null will be returned.</returns>
        Public Function GetLastCellAddress() As Address?
            Dim lastRow As Integer = GetLastRowNumber()
            Dim lastColumn As Integer = GetLastColumnNumber()
            If lastRow < 0 OrElse lastColumn < 0 Then
                Return Nothing
            End If
            Return New Address(lastColumn, lastRow)
        End Function

        ''' <summary>
        ''' Gets the last existing cell with data in the current worksheet (bottom right)
        ''' </summary>
        ''' <returns>Nullable Cell Address. If no cell address could be determined, null will be returned.</returns>
        Public Function GetLastDataCellAddress() As Address?
            Dim lastRow As Integer = GetLastDataRowNumber()
            Dim lastColumn As Integer = GetLastDataColumnNumber()
            If lastRow < 0 OrElse lastColumn < 0 Then
                Return Nothing
            End If
            Return New Address(lastColumn, lastRow)
        End Function

        ''' <summary>
        ''' Gets the first existing cell in the current worksheet (bottom right)
        ''' </summary>
        ''' <returns>Nullable Cell Address. If no cell address could be determined, null will be returned.</returns>
        Public Function GetFirstCellAddress() As Address?
            Dim firstRow As Integer = GetFirstRowNumber()
            Dim firstColumn As Integer = GetFirstColumnNumber()
            If firstRow < 0 OrElse firstColumn < 0 Then
                Return Nothing
            End If
            Return New Address(firstColumn, firstRow)
        End Function

        ''' <summary>
        ''' Gets the first existing cell with data in the current worksheet (bottom right)
        ''' </summary>
        ''' <returns>Nullable Cell Address. If no cell address could be determined, null will be returned.</returns>
        Public Function GetFirstDataCellAddress() As Address?
            Dim firstRow As Integer = GetFirstDataRowNumber()
            Dim firstColumn As Integer = GetLastDataColumnNumber()
            If firstRow < 0 OrElse firstColumn < 0 Then
                Return Nothing
            End If
            Return New Address(firstColumn, firstRow)
        End Function

        ''' <summary>
        ''' Gets either the minimum or maximum row or column number, considering only calls with data
        ''' </summary>
        ''' <paramname="row">If true, the min or max row is returned, otherwise the column.</param>
        ''' <paramname="min">If true, the min value of the row or column is defined, otherwise the max value.</param>
        ''' <paramname="ignoreEmpty">If true, empty cell values are ignored, otherwise considered without checking the content.</param>
        ''' <returns>Min or max number, or -1 if not defined.</returns>
        Private Function GetBoundaryDataNumber(ByVal row As Boolean, ByVal min As Boolean, ByVal ignoreEmpty As Boolean) As Integer
            If cellsField.Count = 0 Then
                Return -1
            End If
            If Not ignoreEmpty Then
                If row AndAlso min Then
                    Return cellsField.Min(Function(x) x.Value.RowNumber)
                ElseIf row Then
                    Return cellsField.Max(Function(x) x.Value.RowNumber)
                ElseIf min Then
                    Return cellsField.Min(Function(x) x.Value.ColumnNumber)
                Else
                    Return cellsField.Max(Function(x) x.Value.ColumnNumber)
                End If
            End If
            Dim nonEmptyCells As List(Of Cell) = cellsField.Values.Where(Function(x) x.Value IsNot Nothing).ToList()
            If nonEmptyCells.Count = 0 Then
                Return -1
            End If
            If row AndAlso min Then
                Return nonEmptyCells.Where(Function(x) Not Equals(x.Value.ToString(), String.Empty)).Min(Function(x) x.RowNumber)
            ElseIf row Then
                Return nonEmptyCells.Where(Function(x) Not Equals(x.Value.ToString(), String.Empty)).Max(Function(x) x.RowNumber)
            ElseIf min Then
                Return nonEmptyCells.Where(Function(x) Not Equals(x.Value.ToString(), String.Empty)).Max(Function(x) x.ColumnNumber)
            Else
                Return nonEmptyCells.Where(Function(x) Not Equals(x.Value.ToString(), String.Empty)).Min(Function(x) x.ColumnNumber)
            End If
        End Function

        ''' <summary>
        ''' Gets either the minimum or maximum row or column number, considering all available data
        ''' </summary>
        ''' <paramname="row">If true, the min or max row is returned, otherwise the column.</param>
        ''' <paramname="min">If true, the min value of the row or column is defined, otherwise the max value.</param>
        ''' <returns>Min or max number, or -1 if not defined.</returns>
        Private Function GetBoundaryNumber(ByVal row As Boolean, ByVal min As Boolean) As Integer
            Dim cellBoundary = GetBoundaryDataNumber(row, min, False)
            If row Then
                Dim heightBoundary = -1
                If rowHeightsField.Count > 0 Then
                    heightBoundary = If(min, RowHeights.Min(Function(x) x.Key), RowHeights.Max(Function(x) x.Key))
                End If
                Dim hiddenBoundary = -1
                If hiddenRowsField.Count > 0 Then
                    hiddenBoundary = If(min, HiddenRows.Min(Function(x) x.Key), HiddenRows.Max(Function(x) x.Key))
                End If
                Return If(min, GetMinRow(cellBoundary, heightBoundary, hiddenBoundary), GetMaxRow(cellBoundary, heightBoundary, hiddenBoundary))
            Else
                Dim columnDefBoundary = -1
                If columnsField.Count > 0 Then
                    columnDefBoundary = If(min, Columns.Min(Function(x) x.Key), Columns.Max(Function(x) x.Key))
                End If
                If min Then
                    Return If(cellBoundary > 0 AndAlso cellBoundary < columnDefBoundary, cellBoundary, columnDefBoundary)
                Else
                    Return If(cellBoundary > 0 AndAlso cellBoundary > columnDefBoundary, cellBoundary, columnDefBoundary)
                End If
            End If
        End Function

        ''' <summary>
        ''' Gets the maximum row coordinate either from cell data, height definitions or hidden rows
        ''' </summary>
        ''' <paramname="cellBoundary">Row number of max cell data.</param>
        ''' <paramname="heightBoundary">Row number of max defined row height.</param>
        ''' <paramname="hiddenBoundary">Row number of max defined hidden row.</param>
        ''' <returns>Max row number or -1 if nothing valid defined.</returns>
        Private Function GetMaxRow(ByVal cellBoundary As Integer, ByVal heightBoundary As Integer, ByVal hiddenBoundary As Integer) As Integer
            Dim highest = -1
            If cellBoundary >= 0 Then
                highest = cellBoundary
            End If
            If heightBoundary >= 0 AndAlso heightBoundary > highest Then
                highest = heightBoundary
            End If
            If hiddenBoundary >= 0 AndAlso hiddenBoundary > highest Then
                highest = hiddenBoundary
            End If
            Return highest
        End Function

        ''' <summary>
        ''' Gets the minimum row coordinate either from cell data, height definitions or hidden rows
        ''' </summary>
        ''' <paramname="cellBoundary">Row number of min cell data.</param>
        ''' <paramname="heightBoundary">Row number of min defined row height.</param>
        ''' <paramname="hiddenBoundary">Row number of min defined hidden row.</param>
        ''' <returns>Min row number or -1 if nothing valid defined.</returns>
        Private Function GetMinRow(ByVal cellBoundary As Integer, ByVal heightBoundary As Integer, ByVal hiddenBoundary As Integer) As Integer
            Dim lowest = Integer.MaxValue
            If cellBoundary >= 0 Then
                lowest = cellBoundary
            End If
            If heightBoundary >= 0 AndAlso heightBoundary < lowest Then
                lowest = heightBoundary
            End If
            If hiddenBoundary >= 0 AndAlso hiddenBoundary < lowest Then
                lowest = hiddenBoundary
            End If
            Return If(lowest = Integer.MaxValue, -1, lowest)
        End Function

        ''' <summary>
        ''' Gets the current column number (zero based)
        ''' </summary>
        ''' <returns>Column number (zero-based).</returns>
        Public Function GetCurrentColumnNumber() As Integer
            Return currentColumnNumber
        End Function

        ''' <summary>
        ''' Gets the current row number (zero based)
        ''' </summary>
        ''' <returns>Row number (zero-based).</returns>
        Public Function GetCurrentRowNumber() As Integer
            Return currentRowNumber
        End Function

        ''' <summary>
        ''' Moves the current position to the next column
        ''' </summary>
        Public Sub GoToNextColumn()
            currentColumnNumber += 1
            currentRowNumber = 0
            ValidateColumnNumber(currentColumnNumber)
        End Sub

        ''' <summary>
        ''' Moves the current position to the next column with the number of cells to move
        ''' </summary>
        ''' <paramname="numberOfColumns">Number of columns to move.</param>
        ''' <paramname="keepRowPosition">If true, the row position is preserved, otherwise set to 0.</param>
        Public Sub GoToNextColumn(ByVal numberOfColumns As Integer, ByVal Optional keepRowPosition As Boolean = False)
            currentColumnNumber += numberOfColumns
            If Not keepRowPosition Then
                currentRowNumber = 0
            End If
            ValidateColumnNumber(currentColumnNumber)
        End Sub

        ''' <summary>
        ''' Moves the current position to the next row (use for a new line)
        ''' </summary>
        Public Sub GoToNextRow()
            currentRowNumber += 1
            currentColumnNumber = 0
            ValidateRowNumber(currentRowNumber)
        End Sub

        ''' <summary>
        ''' Moves the current position to the next row with the number of cells to move (use for a new line)
        ''' </summary>
        ''' <paramname="numberOfRows">Number of rows to move.</param>
        ''' <paramname="keepColumnPosition">If true, the column position is preserved, otherwise set to 0.</param>
        Public Sub GoToNextRow(ByVal numberOfRows As Integer, ByVal Optional keepColumnPosition As Boolean = False)
            currentRowNumber += numberOfRows
            If Not keepColumnPosition Then
                currentColumnNumber = 0
            End If
            ValidateRowNumber(currentRowNumber)
        End Sub

        ''' <summary>
        ''' Merges the defined cell range
        ''' </summary>
        ''' <paramname="cellRange">Range to merge.</param>
        ''' <returns>Returns the validated range of the merged cells (e.g. 'A1:B12').</returns>
        Public Function MergeCells(ByVal cellRange As Range) As String
            Return MergeCells(cellRange.StartAddress, cellRange.EndAddress)
        End Function

        ''' <summary>
        ''' Merges the defined cell range
        ''' </summary>
        ''' <paramname="cellRange">Range to merge (e.g. 'A1:B12').</param>
        ''' <returns>Returns the validated range of the merged cells (e.g. 'A1:B12').</returns>
        Public Function MergeCells(ByVal cellRange As String) As String
            Dim range = ResolveCellRange(cellRange)
            Return MergeCells(range.StartAddress, range.EndAddress)
        End Function

        ''' <summary>
        ''' Merges the defined cell range
        ''' </summary>
        ''' <paramname="startAddress">Start address of the merged cell range.</param>
        ''' <paramname="endAddress">End address of the merged cell range.</param>
        ''' <returns>Returns the validated range of the merged cells (e.g. 'A1:B12').</returns>
        Public Function MergeCells(ByVal startAddress As Address, ByVal endAddress As Address) As String
            Dim key As String = startAddress.ToString() & ":" & endAddress.ToString()
            Dim value As Range = New Range(startAddress, endAddress)
            Dim enclosedAddress As IReadOnlyList(Of Address) = value.ResolveEnclosedAddresses()
            For Each item In mergedCellsField
                If Enumerable.ToList(Enumerable.Intersect(item.Value.ResolveEnclosedAddresses(), enclosedAddress)).Count > 0 Then
                    Throw New RangeException("ConflictingRangeException", "The passed range: " & value.ToString() & " contains cells that are already in the defined merge range: " & item.Key)
                End If
            Next
            mergedCellsField.Add(key, value)
            Return key
        End Function

        ''' <summary>
        ''' Method to recalculate the auto filter (columns) of this worksheet. This is an internal method. There is no need to use it
        ''' </summary>
        Friend Sub RecalculateAutoFilter()
            If autoFilterRangeField Is Nothing Then
                Return
            End If
            Dim start = autoFilterRangeField.Value.StartAddress.Column
            Dim [end] = autoFilterRangeField.Value.EndAddress.Column
            Dim endRow = 0
            For Each item In Cells
                If item.Value.ColumnNumber < start OrElse item.Value.ColumnNumber > [end] Then
                    Continue For
                End If
                If item.Value.RowNumber > endRow Then
                    endRow = item.Value.RowNumber
                End If
            Next
            Dim c As Column
            For i = start To [end]
                If Not columnsField.ContainsKey(i) Then
                    c = New Column(i)
                    c.HasAutoFilter = True
                    columnsField.Add(i, c)
                Else
                    columnsField(i).HasAutoFilter = True
                End If
            Next
            Dim temp As Range = New Range()
            temp.StartAddress = New Address(start, 0)
            temp.EndAddress = New Address([end], endRow)
            autoFilterRangeField = temp
        End Sub

        ''' <summary>
        ''' Method to recalculate the collection of columns of this worksheet. This is an internal method. There is no need to use it
        ''' </summary>
        Friend Sub RecalculateColumns()
            Dim columnsToDelete As List(Of Integer) = New List(Of Integer)()
            For Each col In columnsField
                If Not col.Value.HasAutoFilter AndAlso Not col.Value.IsHidden AndAlso Math.Abs(col.Value.Width - DEFAULT_COLUMN_WIDTH) <= FLOAT_THRESHOLD Then
                    columnsToDelete.Add(col.Key)
                End If
                If Not col.Value.HasAutoFilter AndAlso Not col.Value.IsHidden AndAlso Math.Abs(col.Value.Width - DEFAULT_COLUMN_WIDTH) <= FLOAT_THRESHOLD Then
                    columnsToDelete.Add(col.Key)
                End If
            Next
            For Each index In columnsToDelete
                columnsField.Remove(index)
            Next
        End Sub

        ''' <summary>
        ''' Method to resolve all merged cells of the worksheet. Only the value of the very first cell of the locked cells range will be visible. The other values are still present (set to EMPTY) but will not be stored in the worksheet.<br/>
        ''' This is an internal method. There is no need to use it
        ''' </summary>
        Friend Sub ResolveMergedCells()
            Dim mergeStyle = Style.BasicStyles.MergeCellStyle
            Dim cell As Cell
            For Each range In MergedCells
                Dim pos = 0
                Dim addresses As List(Of Address) = TryCast(GetCellRange(range.Value.StartAddress, range.Value.EndAddress), List(Of Address))
                For Each address In addresses
                    If Not Cells.ContainsKey(address.GetAddress()) Then
                        cell = New Cell()
                        cell.DataType = CellType.EMPTY
                        cell.RowNumber = address.Row
                        cell.ColumnNumber = address.Column
                        AddCell(cell, cell.ColumnNumber, cell.RowNumber)
                    Else
                        cell = Cells(address.GetAddress())
                    End If
                    If pos <> 0 Then
                        If cell.CellStyle Is Nothing Then
                            cell.SetStyle(mergeStyle)
                        Else
                            Dim mixedMergeStyle = cell.CellStyle
                            ' TODO: There should be a better possibility to identify particular style elements that deviates
                            mixedMergeStyle.CurrentCellXf.ForceApplyAlignment = mergeStyle.CurrentCellXf.ForceApplyAlignment
                            cell.SetStyle(mixedMergeStyle)
                        End If
                    End If
                    pos += 1
                Next
            Next
        End Sub

        ''' <summary>
        ''' Removes auto filters from the worksheet
        ''' </summary>
        Public Sub RemoveAutoFilter()
            autoFilterRangeField = Nothing
        End Sub

        ''' <summary>
        ''' Sets a previously defined, hidden column as visible again
        ''' </summary>
        ''' <paramname="columnNumber">Column number to make visible again.</param>
        Public Sub RemoveHiddenColumn(ByVal columnNumber As Integer)
            SetColumnHiddenState(columnNumber, False)
        End Sub

        ''' <summary>
        ''' Sets a previously defined, hidden column as visible again
        ''' </summary>
        ''' <paramname="columnAddress">Column address to make visible again.</param>
        Public Sub RemoveHiddenColumn(ByVal columnAddress As String)
            Dim columnNumber = ResolveColumn(columnAddress)
            SetColumnHiddenState(columnNumber, False)
        End Sub

        ''' <summary>
        ''' Sets a previously defined, hidden row as visible again
        ''' </summary>
        ''' <paramname="rowNumber">Row number to hide on the worksheet.</param>
        Public Sub RemoveHiddenRow(ByVal rowNumber As Integer)
            SetRowHiddenState(rowNumber, False)
        End Sub

        ''' <summary>
        ''' Removes the defined merged cell range
        ''' </summary>
        ''' <paramname="range">Cell range to remove the merging.</param>
        Public Sub RemoveMergedCells(ByVal range As String)
            If Not Equals(range, Nothing) Then
                range = range.ToUpper()
            End If
            If Equals(range, Nothing) OrElse Not mergedCellsField.ContainsKey(range) Then
                Throw New RangeException("UnknownRangeException", "The cell range " & range & " was not found in the list of merged cell ranges")
            End If

            Dim addresses As List(Of Address) = TryCast(GetCellRange(range), List(Of Address))
            For Each address In addresses
                If cellsField.ContainsKey(address.GetAddress()) Then
                    Dim cell As Cell = cellsField(address.ToString())
                    If Style.BasicStyles.MergeCellStyle.Equals(cell.CellStyle) Then
                        cell.RemoveStyle()
                    End If
                    cell.ResolveCellType() ' resets the type
                End If
            Next
            mergedCellsField.Remove(range)
        End Sub

        ''' <summary>
        ''' Removes the cell selection of this worksheet
        ''' </summary>
        Public Sub RemoveSelectedCells()
            selectedCellsField.Clear()
        End Sub

        ''' <summary>
        ''' Removes the defined, non-standard row height
        ''' </summary>
        ''' <paramname="rowNumber">Row number (zero-based).</param>
        Public Sub RemoveRowHeight(ByVal rowNumber As Integer)
            If rowHeightsField.ContainsKey(rowNumber) Then
                rowHeightsField.Remove(rowNumber)
            End If
        End Sub

        ''' <summary>
        ''' Removes an allowed action on the current worksheet or its cells
        ''' </summary>
        ''' <paramname="value">Allowed action on the worksheet or cells.</param>
        Public Sub RemoveAllowedActionOnSheetProtection(ByVal value As SheetProtectionValue)
            If sheetProtectionValuesField.Contains(value) Then
                sheetProtectionValuesField.Remove(value)
            End If
        End Sub

        ''' <summary>
        ''' Sets the active style of the worksheet. This style will be assigned to all later added cells
        ''' </summary>
        ''' <paramname="style">Style to set as active style.</param>
        Public Sub SetActiveStyle(ByVal style As Style)
            If style Is Nothing Then
                useActiveStyle = False
            Else
                useActiveStyle = True
            End If
            activeStyleField = style
        End Sub

        ''' <summary>
        ''' Sets the column auto filter within the defined column range
        ''' </summary>
        ''' <paramname="startColumn">Column number with the first appearance of an auto filter drop down.</param>
        ''' <paramname="endColumn">Column number with the last appearance of an auto filter drop down.</param>
        Public Sub SetAutoFilter(ByVal startColumn As Integer, ByVal endColumn As Integer)
            Dim start = ResolveCellAddress(startColumn, 0)
            Dim [end] = ResolveCellAddress(endColumn, 0)
            If endColumn < startColumn Then
                SetAutoFilter([end] & ":" & start)
            Else
                SetAutoFilter(start & ":" & [end])
            End If
        End Sub

        ''' <summary>
        ''' Sets the column auto filter within the defined column range
        ''' </summary>
        ''' <paramname="range">Range to apply auto filter on. The range could be 'A1:C10' for instance. The end row will be recalculated automatically when saving the file.</param>
        Public Sub SetAutoFilter(ByVal range As String)
            autoFilterRangeField = ResolveCellRange(range)
            RecalculateAutoFilter()
            RecalculateColumns()
        End Sub

        ''' <summary>
        ''' Sets the defined column as hidden or visible
        ''' </summary>
        ''' <paramname="columnNumber">Column number to hide on the worksheet.</param>
        ''' <paramname="state">If true, the column will be hidden, otherwise be visible.</param>
        Private Sub SetColumnHiddenState(ByVal columnNumber As Integer, ByVal state As Boolean)
            ValidateColumnNumber(columnNumber)
            If columnsField.ContainsKey(columnNumber) Then
                columnsField(columnNumber).IsHidden = state
            ElseIf state Then
                Dim c As Column = New Column(columnNumber)
                c.IsHidden = True
                columnsField.Add(columnNumber, c)
            End If
            If Not columnsField(columnNumber).IsHidden AndAlso Math.Abs(columnsField(columnNumber).Width - DEFAULT_COLUMN_WIDTH) <= FLOAT_THRESHOLD AndAlso Not columnsField(columnNumber).HasAutoFilter Then
                columnsField.Remove(columnNumber)
            End If
        End Sub

        ''' <summary>
        ''' Sets the width of the passed column address
        ''' </summary>
        ''' <paramname="columnAddress">Column address (A - XFD).</param>
        ''' <paramname="width">Width from 0 to 255.0.</param>
        Public Sub SetColumnWidth(ByVal columnAddress As String, ByVal width As Single)
            Dim columnNumber = ResolveColumn(columnAddress)
            SetColumnWidth(columnNumber, width)
        End Sub

        ''' <summary>
        ''' Sets the width of the passed column number (zero-based)
        ''' </summary>
        ''' <paramname="columnNumber">Column number (zero-based, from 0 to 16383).</param>
        ''' <paramname="width">Width from 0 to 255.0.</param>
        Public Sub SetColumnWidth(ByVal columnNumber As Integer, ByVal width As Single)
            ValidateColumnNumber(columnNumber)
            If width < MIN_COLUMN_WIDTH OrElse width > MAX_COLUMN_WIDTH Then
                Throw New RangeException("OutOfRangeException", "The column width (" & width.ToString() & ") is out of range. Range is from " & MIN_COLUMN_WIDTH.ToString() & " to " & MAX_COLUMN_WIDTH.ToString() & " (chars).")
            End If
            If columnsField.ContainsKey(columnNumber) Then
                columnsField(columnNumber).Width = width
            Else
                Dim c As Column = New Column(columnNumber)
                c.Width = width
                columnsField.Add(columnNumber, c)
            End If
        End Sub

        ''' <summary>
        ''' Set the current cell address
        ''' </summary>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        Public Sub SetCurrentCellAddress(ByVal columnNumber As Integer, ByVal rowNumber As Integer)
            SetCurrentColumnNumber(columnNumber)
            SetCurrentRowNumber(rowNumber)
        End Sub

        ''' <summary>
        ''' Set the current cell address
        ''' </summary>
        ''' <paramname="address">Cell address in the format A1 - XFD1048576.</param>
        Public Sub SetCurrentCellAddress(ByVal address As String)
            Dim row, column As Integer
            ResolveCellCoordinate(address, column, row)
            SetCurrentCellAddress(column, row)
        End Sub

        ''' <summary>
        ''' Sets the current column number (zero based)
        ''' </summary>
        ''' <paramname="columnNumber">Column number (zero based).</param>
        Public Sub SetCurrentColumnNumber(ByVal columnNumber As Integer)
            ValidateColumnNumber(columnNumber)
            currentColumnNumber = columnNumber
        End Sub

        ''' <summary>
        ''' Sets the current row number (zero based)
        ''' </summary>
        ''' <paramname="rowNumber">Row number (zero based).</param>
        Public Sub SetCurrentRowNumber(ByVal rowNumber As Integer)
            ValidateRowNumber(rowNumber)
            currentRowNumber = rowNumber
        End Sub

        ''' <summary>
        ''' Sets a single range of selected cells on this worksheet. All existing ranges will be removed
        ''' </summary>
        ''' <paramname="range">Range to set as single cell range for selected cells</param>
        <Obsolete("This method is a deprecated subset of the function AddSelectedCells. It will be removed in a future version")>
        Public Sub SetSelectedCells(ByVal range As Range)
            RemoveSelectedCells()
            AddSelectedCells(range)
        End Sub

        ''' <summary>
        ''' Sets the selected cells on this worksheet
        ''' </summary>
        ''' <paramname="startAddress">Start address of the range to set as single cell range for selected cells</param>
        ''' <paramname="endAddress">End address of the range to set as single cell range for selected cells</param>
        <Obsolete("This method is a deprecated subset of the function AddSelectedCells. It will be removed in a future version")>
        Public Sub SetSelectedCells(ByVal startAddress As Address, ByVal endAddress As Address)
            SetSelectedCells(New Range(startAddress, endAddress))
        End Sub

        ''' <summary>
        ''' Sets a single range of selected cells on this worksheet. All existing ranges will be removed. Null will remove all selected cells
        ''' </summary>
        ''' <paramname="range">Range as string to set as single cell range for selected cells, or null to remove the selected cells</param>
        <Obsolete("This method is a deprecated subset of the function AddSelectedCells. It will be removed in a future version")>
        Public Sub SetSelectedCells(ByVal range As String)
            If Equals(range, Nothing) Then
                selectedCellsField.Clear()
                Return
            Else
                SetSelectedCells(New Range(range))
            End If
        End Sub

        ''' <summary>
        ''' Adds a range to the selected cells on this worksheet
        ''' </summary>
        ''' <paramname="range">Cell range to be added as selected cells</param>
        Public Sub AddSelectedCells(ByVal range As Range)
            selectedCellsField.Add(range)
        End Sub

        ''' <summary>
        ''' Adds a range to the selected cells on this worksheet
        ''' </summary>
        ''' <paramname="startAddress">Start address of the range to add</param>
        ''' <paramname="endAddress">End address of the range to add</param>
        Public Sub AddSelectedCells(ByVal startAddress As Address, ByVal endAddress As Address)
            selectedCellsField.Add(New Range(startAddress, endAddress))
        End Sub

        ''' <summary>
        ''' Adds a range to the selected cells on this worksheet. Null or empty as value will be ignored
        ''' </summary>
        ''' <paramname="range">Cell range to add as selected cells</param>
        Public Sub AddSelectedCells(ByVal range As String)
            If Not Equals(range, Nothing) Then
                selectedCellsField.Add(ResolveCellRange(range))
            End If
        End Sub

        ''' <summary>
        ''' Sets or removes the password for worksheet protection. If set, UseSheetProtection will be also set to true
        ''' </summary>
        ''' <paramname="password">Password (UTF-8) to protect the worksheet. If the password is null or empty, no password will be used.</param>
        Public Sub SetSheetProtectionPassword(ByVal password As String)
            If String.IsNullOrEmpty(password) Then
                sheetProtectionPasswordField = Nothing
                sheetProtectionPasswordHashField = Nothing
                UseSheetProtection = False
            Else
                sheetProtectionPasswordField = password
                sheetProtectionPasswordHashField = LowLevel.GeneratePasswordHash(password)
                UseSheetProtection = True
            End If
        End Sub

        ''' <summary>
        ''' Sets the height of the passed row number (zero-based)
        ''' </summary>
        ''' <paramname="rowNumber">Row number (zero-based, 0 to 1048575).</param>
        ''' <paramname="height">Height from 0 to 409.5.</param>
        Public Sub SetRowHeight(ByVal rowNumber As Integer, ByVal height As Single)
            ValidateRowNumber(rowNumber)
            If height < MIN_ROW_HEIGHT OrElse height > MAX_ROW_HEIGHT Then
                Throw New RangeException("OutOfRangeException", "The row height (" & height.ToString() & ") is out of range. Range is from " & MIN_ROW_HEIGHT.ToString() & " to " & MAX_ROW_HEIGHT.ToString() & " (equals 546px).")
            End If
            If rowHeightsField.ContainsKey(rowNumber) Then
                rowHeightsField(rowNumber) = height
            Else
                rowHeightsField.Add(rowNumber, height)
            End If
        End Sub

        ''' <summary>
        ''' Sets the defined row as hidden or visible
        ''' </summary>
        ''' <paramname="rowNumber">Row number to make visible again.</param>
        ''' <paramname="state">If true, the row will be hidden, otherwise visible.</param>
        Private Sub SetRowHiddenState(ByVal rowNumber As Integer, ByVal state As Boolean)
            ValidateRowNumber(rowNumber)
            If hiddenRowsField.ContainsKey(rowNumber) Then
                If state Then
                    hiddenRowsField(rowNumber) = True
                Else
                    hiddenRowsField.Remove(rowNumber)
                End If
            ElseIf state Then
                hiddenRowsField.Add(rowNumber, True)
            End If
        End Sub

        ''' <summary>
        ''' Validates and sets the worksheet name
        ''' </summary>
        ''' <paramname="name">Name to set.</param>
        Public Sub SetSheetName(ByVal name As String)
            If String.IsNullOrEmpty(name) Then
                Throw New FormatException("the worksheet name must be between 1 and " & MAX_WORKSHEET_NAME_LENGTH.ToString() & " characters")
            End If
            If name.Length > MAX_WORKSHEET_NAME_LENGTH Then
                Throw New FormatException("the worksheet name must be between 1 and " & MAX_WORKSHEET_NAME_LENGTH.ToString() & " characters")
            End If
            Dim regex As Regex = New Regex("[\[\]\*\?/\\]")
            Dim match = regex.Match(name)
            If match.Captures.Count > 0 Then
                Throw New FormatException("the worksheet name must not contain the characters [  ]  * ? / \ ")
            End If
            sheetNameField = name
        End Sub

        ''' <summary>
        ''' Sets the name of the worksheet
        ''' </summary>
        ''' <paramname="name">Name of the worksheet.</param>
        ''' <paramname="sanitize">If true, the filename will be sanitized automatically according to the specifications of Excel.</param>
        Public Sub SetSheetName(ByVal name As String, ByVal sanitize As Boolean)
            If sanitize Then
                sheetNameField = "" ' Empty name (temporary) to prevent conflicts during sanitizing
                sheetNameField = SanitizeWorksheetName(name, workbookReferenceField)
            Else
                SetSheetName(name)
            End If
        End Sub

        ''' <summary>
        ''' Sets the horizontal split of the worksheet into two panes. The measurement in characters cannot be used to freeze panes
        ''' </summary>
        ''' <paramname="topPaneHeight">Height (similar to row height) from top of the worksheet to the split line in characters.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable). Only the row component is important in a horizontal split.</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetHorizontalSplit(ByVal topPaneHeight As Single, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            SetSplit(Nothing, topPaneHeight, topLeftCell, activePane)
        End Sub

        ''' <summary>
        ''' Sets the horizontal split of the worksheet into two panes. The measurement in rows can be used to split and freeze panes
        ''' </summary>
        ''' <paramname="numberOfRowsFromTop">Number of rows from top of the worksheet to the split line. The particular row heights are considered.</param>
        ''' <paramname="freeze">If true, all panes are frozen, otherwise remains movable.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable). Only the row component is important in a horizontal split.</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetHorizontalSplit(ByVal numberOfRowsFromTop As Integer, ByVal freeze As Boolean, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            SetSplit(Nothing, numberOfRowsFromTop, freeze, topLeftCell, activePane)
        End Sub

        ''' <summary>
        ''' Sets the vertical split of the worksheet into two panes. The measurement in characters cannot be used to freeze panes
        ''' </summary>
        ''' <paramname="leftPaneWidth">Width (similar to column width) from left of the worksheet to the split line in characters.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable). Only the column component is important in a vertical split.</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetVerticalSplit(ByVal leftPaneWidth As Single, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            SetSplit(leftPaneWidth, Nothing, topLeftCell, activePane)
        End Sub

        ''' <summary>
        ''' Sets the vertical split of the worksheet into two panes. The measurement in columns can be used to split and freeze panes
        ''' </summary>
        ''' <paramname="numberOfColumnsFromLeft">Number of columns from left of the worksheet to the split line. The particular column widths are considered.</param>
        ''' <paramname="freeze">If true, all panes are frozen, otherwise remains movable.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable). Only the column component is important in a vertical split.</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetVerticalSplit(ByVal numberOfColumnsFromLeft As Integer, ByVal freeze As Boolean, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            SetSplit(numberOfColumnsFromLeft, Nothing, freeze, topLeftCell, activePane)
        End Sub

        ''' <summary>
        ''' Sets the horizontal and vertical split of the worksheet into four panes. The measurement in rows and columns can be used to split and freeze panes
        ''' </summary>
        ''' <paramname="numberOfColumnsFromLeft">The numberOfColumnsFromLeft<seecref="Integer."/>.</param>
        ''' <paramname="numberOfRowsFromTop">The numberOfRowsFromTop<seecref="Integer."/>.</param>
        ''' <paramname="freeze">If true, all panes are frozen, otherwise remains movable.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable).</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetSplit(ByVal numberOfColumnsFromLeft As Integer?, ByVal numberOfRowsFromTop As Integer?, ByVal freeze As Boolean, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            If freeze Then
                If numberOfColumnsFromLeft IsNot Nothing AndAlso topLeftCell.Column < numberOfColumnsFromLeft.Value Then
                    Throw New WorksheetException("The column number " & topLeftCell.Column.ToString() & " is not valid for a frozen, vertical split with the split pane column number " & numberOfColumnsFromLeft.Value.ToString())
                End If
                If numberOfRowsFromTop IsNot Nothing AndAlso topLeftCell.Row < numberOfRowsFromTop.Value Then
                    Throw New WorksheetException("The row number " & topLeftCell.Row.ToString() & " is not valid for a frozen, horizontal split height the split pane row number " & numberOfRowsFromTop.Value.ToString())
                End If
            End If
            paneSplitLeftWidthField = Nothing
            paneSplitTopHeightField = Nothing
            freezeSplitPanesField = freeze
            Dim row = If(numberOfRowsFromTop IsNot Nothing, numberOfRowsFromTop.Value, 0)
            Dim column = If(numberOfColumnsFromLeft IsNot Nothing, numberOfColumnsFromLeft.Value, 0)
            paneSplitAddressField = New Address(column, row)
            paneSplitTopLeftCellField = topLeftCell
            activePaneField = activePane
        End Sub

        ''' <summary>
        ''' Sets the horizontal and vertical split of the worksheet into four panes. The measurement in characters cannot be used to freeze panes
        ''' </summary>
        ''' <paramname="leftPaneWidth">The leftPaneWidth<seecref="Single."/>.</param>
        ''' <paramname="topPaneHeight">The topPaneHeight<seecref="Single."/>.</param>
        ''' <paramname="topLeftCell">Top Left cell address of the bottom right pane (if applicable).</param>
        ''' <paramname="activePane">Active pane in the split window.</param>
        Public Sub SetSplit(ByVal leftPaneWidth As Single?, ByVal topPaneHeight As Single?, ByVal topLeftCell As Address, ByVal activePane As WorksheetPane)
            paneSplitLeftWidthField = leftPaneWidth
            paneSplitTopHeightField = topPaneHeight
            freezeSplitPanesField = Nothing
            paneSplitAddressField = Nothing
            paneSplitTopLeftCellField = topLeftCell
            activePaneField = activePane
        End Sub

        ''' <summary>
        ''' Resets splitting of the worksheet into panes, as well as their freezing
        ''' </summary>
        Public Sub ResetSplit()
            paneSplitLeftWidthField = Nothing
            paneSplitTopHeightField = Nothing
            freezeSplitPanesField = Nothing
            paneSplitAddressField = Nothing
            paneSplitTopLeftCellField = Nothing
            activePaneField = Nothing
        End Sub

        ''' <summary>
        ''' Creates a (dereferenced) deep copy of this worksheet
        ''' </summary>
        ''' <returns>The <seecref="Worksheet"/>.</returns>
        Public Function Copy() As Worksheet
            Dim lCopy As Worksheet = New Worksheet()
            For Each cell In cellsField
                lCopy.AddCell(cell.Value.Copy(), cell.Key)
            Next
            lCopy.activePaneField = activePaneField
            lCopy.activeStyleField = activeStyleField
            If autoFilterRangeField.HasValue Then
                lCopy.autoFilterRangeField = autoFilterRangeField.Value.Copy()
            End If
            For Each column In columnsField
                lCopy.columnsField.Add(column.Key, column.Value.Copy())
            Next
            lCopy.CurrentCellDirection = CurrentCellDirection
            lCopy.currentColumnNumber = currentColumnNumber
            lCopy.currentRowNumber = currentRowNumber
            lCopy.defaultColumnWidthField = defaultColumnWidthField
            lCopy.defaultRowHeightField = defaultRowHeightField
            lCopy.freezeSplitPanesField = freezeSplitPanesField
            lCopy.hiddenField = hiddenField
            For Each row In hiddenRowsField
                lCopy.hiddenRowsField.Add(row.Key, row.Value)
            Next
            For Each cell In mergedCellsField
                lCopy.mergedCellsField.Add(cell.Key, cell.Value.Copy())
            Next
            If paneSplitAddressField.HasValue Then
                lCopy.paneSplitAddressField = paneSplitAddressField.Value.Copy()
            End If
            lCopy.paneSplitLeftWidthField = paneSplitLeftWidthField
            lCopy.paneSplitTopHeightField = paneSplitTopHeightField
            If paneSplitTopLeftCellField.HasValue Then
                lCopy.paneSplitTopLeftCellField = paneSplitTopLeftCellField.Value.Copy()
            End If
            For Each row In rowHeightsField
                lCopy.rowHeightsField.Add(row.Key, row.Value)
            Next
            If selectedCellsField.Count > 0 Then
                For Each selectedCellRange In selectedCellsField
                    lCopy.AddSelectedCells(selectedCellRange.Copy())
                Next
            End If
            lCopy.sheetProtectionPasswordField = sheetProtectionPasswordField
            lCopy.sheetProtectionPasswordHashField = sheetProtectionPasswordHashField
            For Each value As SheetProtectionValue In sheetProtectionValuesField
                lCopy.sheetProtectionValuesField.Add(value)
            Next
            lCopy.useActiveStyle = useActiveStyle
            lCopy.UseSheetProtection = UseSheetProtection
            Return lCopy
        End Function

        ''' <summary>
        ''' Sanitizes a worksheet name
        ''' </summary>
        ''' <paramname="input">Name to sanitize.</param>
        ''' <paramname="workbook">Workbook reference.</param>
        ''' <returns>Name of the sanitized worksheet.</returns>
        Public Shared Function SanitizeWorksheetName(ByVal input As String, ByVal workbook As Workbook) As String
            If Equals(input, Nothing) Then
                input = "Sheet1"
            End If
            Dim len = input.Length
            If len > 31 Then
                len = 31
            ElseIf len = 0 Then
                input = "Sheet1"
            End If
            Dim sb As StringBuilder = New StringBuilder(31)
            Dim c As Char
            For i = 0 To len - 1
                c = input(i)
                If c = "["c OrElse c = "]"c OrElse c = "*"c OrElse c = "?"c OrElse c = "\"c OrElse c = "/"c Then
                    sb.Append("_"c)
                Else
                    sb.Append(c)
                End If
            Next
            Return GetUnusedWorksheetName(sb.ToString(), workbook)
        End Function

        ''' <summary>
        ''' Determines the next unused worksheet name in the passed workbook
        ''' </summary>
        ''' <paramname="name">Original name to start the check.</param>
        ''' <paramname="workbook">Workbook to look for existing worksheets.</param>
        ''' <returns>Not yet used worksheet name.</returns>
        Private Shared Function GetUnusedWorksheetName(ByVal name As String, ByVal workbook As Workbook) As String
            If workbook Is Nothing Then
                Throw New WorksheetException("The workbook reference is null")
            End If
            If Not WorksheetExists(name, workbook) Then
                Return name
            End If
            Dim regex As Regex = New Regex("^(.*?)(\d{1,31})$")
            Dim match = regex.Match(name)
            Dim prefix = name
            Dim number = 1
            If match.Groups.Count > 1 Then
                prefix = match.Groups(1).Value
                Integer.TryParse(match.Groups(2).Value, number)
                ' if this failed, the start number is 0 (parsed number was >max. int32)
            End If
            While True
                Dim numberString = number.ToString("G", CultureInfo.InvariantCulture)
                If numberString.Length + prefix.Length > MAX_WORKSHEET_NAME_LENGTH Then
                    Dim endIndex = prefix.Length - (numberString.Length + prefix.Length - MAX_WORKSHEET_NAME_LENGTH)
                    prefix = prefix.Substring(0, endIndex)
                End If
                Dim newName = prefix & numberString
                If Not WorksheetExists(newName, workbook) Then
                    Return newName
                End If
                number += 1
            End While
        End Function

        ''' <summary>
        ''' Checks whether a worksheet with the given name exists
        ''' </summary>
        ''' <paramname="name">Name to check.</param>
        ''' <paramname="workbook">Workbook reference.</param>
        ''' <returns>True if the name exits, otherwise false.</returns>
        Private Shared Function WorksheetExists(ByVal name As String, ByVal workbook As Workbook) As Boolean
            If workbook Is Nothing Then
                Throw New WorksheetException("The workbook reference is null")
            End If
            Dim len = workbook.Worksheets.Count
            For i = 0 To len - 1
                If Equals(workbook.Worksheets(i).SheetName, name) Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Class representing a column of a worksheet
        ''' </summary>
        Public Class Column
            ''' <summary>
            ''' Defines the number
            ''' </summary>
            Private numberField As Integer

            ''' <summary>
            ''' Defines the columnAddress
            ''' </summary>
            Private columnAddressField As String

            ''' <summary>
            ''' Defines the width
            ''' </summary>
            Private widthField As Single

            ''' <summary>
            ''' Gets or sets the ColumnAddress
            ''' Column address (A to XFD)
            ''' </summary>
            Public Property ColumnAddress As String
                Get
                    Return columnAddressField
                End Get
                Set(ByVal value As String)
                    If String.IsNullOrEmpty(value) Then
                        Throw New RangeException("A general range exception occurred", "The passed address was null or empty")
                    End If
                    numberField = ResolveColumn(value)
                    columnAddressField = value.ToUpper()
                End Set
            End Property

            ''' <summary>
            ''' Gets or sets a value indicating whether HasAutoFilter
            ''' If true, the column has auto filter applied, otherwise not
            ''' </summary>
            Public Property HasAutoFilter As Boolean

            ''' <summary>
            ''' Gets or sets a value indicating whether IsHidden
            ''' If true, the column is hidden, otherwise visible
            ''' </summary>
            Public Property IsHidden As Boolean

            ''' <summary>
            ''' Gets or sets the Number
            ''' Column number (0 to 16383)
            ''' </summary>
            Public Property Number As Integer
                Get
                    Return numberField
                End Get
                Set(ByVal value As Integer)
                    columnAddressField = ResolveColumnAddress(value)
                    numberField = value
                End Set
            End Property

            ''' <summary>
            ''' Gets or sets the Width
            ''' Width of the column
            ''' </summary>
            Public Property Width As Single
                Get
                    Return widthField
                End Get
                Set(ByVal value As Single)
                    If value < MIN_COLUMN_WIDTH OrElse value > MAX_COLUMN_WIDTH Then
                        Throw New RangeException("A general range exception occurred", "The passed column width is out of range (" & MIN_COLUMN_WIDTH.ToString() & " to " & MAX_COLUMN_WIDTH.ToString() & ")")
                    End If
                    widthField = value
                End Set
            End Property

            ''' <summary>
            ''' Prevents a default instance of the <seecref="Column"/> class from being created
            ''' </summary>
            Private Sub New()
                Width = DEFAULT_COLUMN_WIDTH
            End Sub

            ''' <summary>
            ''' Initializes a new instance of the <seecref="Column"/> class
            ''' </summary>
            ''' <paramname="columnCoordinate">Column number (zero-based, 0 to 16383).</param>
            Public Sub New(ByVal columnCoordinate As Integer)
                Me.New()
                Number = columnCoordinate
            End Sub

            ''' <summary>
            ''' Initializes a new instance of the <seecref="Column"/> class
            ''' </summary>
            ''' <paramname="columnAddress">Column address (A to XFD).</param>
            Public Sub New(ByVal columnAddress As String)
                Me.New()
                Me.ColumnAddress = columnAddress
            End Sub

            ''' <summary>
            ''' Creates a deep copy of this column
            ''' </summary>
            ''' <returns>Copy of this column.</returns>
            Friend Function Copy() As Column
                Dim lCopy As Column = New Column()
                lCopy.IsHidden = IsHidden
                lCopy.Width = widthField
                lCopy.HasAutoFilter = HasAutoFilter
                lCopy.columnAddressField = columnAddressField
                lCopy.numberField = numberField
                Return lCopy
            End Function
        End Class
    End Class
End Namespace
