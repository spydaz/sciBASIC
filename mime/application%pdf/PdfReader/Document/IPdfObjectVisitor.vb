﻿
Namespace PdfReader
    Public Interface IPdfObjectVisitor
        Sub Visit(ByVal array As PdfArray)
        Sub Visit(ByVal [boolean] As PdfBoolean)
        Sub Visit(ByVal contents As PdfCatalog)
        Sub Visit(ByVal contents As PdfContents)
        Sub Visit(ByVal dateTime As PdfDateTime)
        Sub Visit(ByVal dateTime As PdfDictionary)
        Sub Visit(ByVal document As PdfDocument)
        Sub Visit(ByVal identifier As PdfIdentifier)
        Sub Visit(ByVal [integer] As PdfInteger)
        Sub Visit(ByVal info As PdfInfo)
        Sub Visit(ByVal indirectObject As PdfIndirectObject)
        Sub Visit(ByVal name As PdfName)
        Sub Visit(ByVal nameTree As PdfNameTree)
        Sub Visit(ByVal nul As PdfNull)
        Sub Visit(ByVal numberTree As PdfNumberTree)
        Sub Visit(ByVal obj As PdfObject)
        Sub Visit(ByVal reference As PdfObjectReference)
        Sub Visit(ByVal outlineItem As PdfOutlineItem)
        Sub Visit(ByVal outlineLevel As PdfOutlineLevel)
        Sub Visit(ByVal page As PdfPage)
        Sub Visit(ByVal pages As PdfPages)
        Sub Visit(ByVal real As PdfReal)
        Sub Visit(ByVal rectangle As PdfRectangle)
        Sub Visit(ByVal stream As PdfStream)
        Sub Visit(ByVal str As PdfString)
        Sub Visit(ByVal version As PdfVersion)
    End Interface
End Namespace
