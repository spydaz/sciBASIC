Imports System.Windows
Imports System.Windows.Controls
'Imports Microsoft.VisualBasic.Windows.Forms

Namespace Pages

    ''' <summary>
    ''' Interaction logic for Home.xaml
    ''' </summary>
    Partial Public Class Home
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Admin_Rights(sender As Object, e As RoutedEventArgs)
            '   VistaSecurity.RestartElevated()
        End Sub

        Private Sub UAC_Initialized(sender As Object, e As EventArgs)
            'If VistaSecurity.IsAdmin() Then
            '    UAC.Visibility = System.Windows.Visibility.Collapsed
            '    UACNote.Visibility = System.Windows.Visibility.Collapsed
            'End If
        End Sub

        Private Sub license_brief_TextChanged(sender As Object, e As TextChangedEventArgs) Handles license_brief.TextChanged
            LicenseInfo.info.Brief = license_brief.Text
        End Sub

        Private Sub license_title_TextChanged(sender As Object, e As TextChangedEventArgs) Handles license_title.TextChanged
            LicenseInfo.info.Title = license_title.Text
        End Sub

        Private Sub copyright_TextChanged(sender As Object, e As TextChangedEventArgs) Handles copyright.TextChanged
            LicenseInfo.info.Copyright = copyright.Text
        End Sub
    End Class
End Namespace
