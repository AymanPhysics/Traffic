' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}


    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

        'If Md.MyProject = Client.ClothesRed Then
        '    bm.SetColor(SampleDisplayBorder)
        '    btnBack.Background = System.Windows.Media.Brushes.White
        'End If
    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)

        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)
            Dim theFrame As Page = CType(theButton.Tag, Page)

            theButton.IsTabStop = False

            SampleDisplayFrame.Content = theButton.Tag
            SampleDisplayBorder.Visibility = Visibility.Visible

            CType(args.Source, RadioButton).IsChecked = False

            Try
                'If Md.MyProject = Client.ClothesRed Then
                '    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateText).RealText.Tag)
                '    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateText).RealText.Tag
                'Else
                theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                'End If

            Catch ex As Exception
            End Try
            sampleDisplayFrameLoaded(theFrame, args)

        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        Try
            If Not CType(CType(sender, Frame).Content, Page).Tag Is Nothing Then
                CType(CType(sender, Frame).Content, Page).Title = CType(CType(sender, Frame).Content, Page).Tag
                Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
            End If
        Catch ex As Exception
        End Try
        Try
            CType(sender, Page).Title = CType(sender, Page).Tag
            Md.Currentpage = CType(sender, Page).Title
        Catch ex As Exception
        End Try

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DesignDt As New DataTable
    Sub LoadLabel(ByVal G As WrapPanel, Ttl As String)
        CurrentMenuitem += 1

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .Height = ActualHeight, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .Margin = New Windows.Thickness(24, 0, 0, 0), .VerticalContentAlignment = Windows.VerticalAlignment.Top, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1)}
        'lbl.SetResourceReference(Label.ContentProperty, Ttl)
        lbl.Content = Ttl
        G.Children.Add(lbl)

    End Sub
    Function LoadRadio(ByVal G As WrapPanel, ByVal frm As UserControl, ByVal Ttl As String) As RadioButton
        CurrentMenuitem += 1
        While Ttl.Length < 16
            Ttl = " " & Ttl & " "
        End While


        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle")}
        Dim AnimatingAlongAPathExampleFrame As New Page
        AnimatingAlongAPathExampleFrame.Content = frm
        r.Tag = AnimatingAlongAPathExampleFrame

        Dim t As New UserControl
        t = New TranslateTextAnimationExample
        CType(t, TranslateTextAnimationExample).RealText.Text = Ttl
        CType(t, TranslateTextAnimationExample).RealText.Tag = Ttl
        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.Width = 180
        r.Height = 90
        r.ToolTip = Ttl

        Return r
    End Function

    Private Sub LoadRadio(ByVal frm As TabItem, ByVal Ttl As String)


        Dim r As New RadioButton With {.Style = Application.Current.FindResource("GlassRadioButtonStyle")}
        Dim AnimatingAlongAPathExampleFrame As New Frame
        AnimatingAlongAPathExampleFrame.Source = New Uri(frm.GetType().ToString.Split(".").Last & ".Xaml", UriKind.RelativeOrAbsolute)
        AnimatingAlongAPathExampleFrame.Background = System.Windows.Media.Brushes.White
        AnimatingAlongAPathExampleFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden
        r.Content = AnimatingAlongAPathExampleFrame
        'SampleGrid.Children.Add(r)
        r.Width = 100
        r.Height = 60
        r.ToolTip = Ttl
    End Sub


    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
    End Sub

    Public Lvl As Boolean = False
    Dim CurrentTab As Integer = 0
    Dim CurrentMenuitem As Integer = 0
    Public Sub Load()

        DesignDt = bm.ExcuteAdapter("select * from PLevels where id='" & Md.UserName & "'")

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs()
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            Dim dt As DataTable = bm.ExcuteAdapter("select * from nlevels where id=" & Md.LevelId)
            If dt.Rows.Count = 0 Then Return

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dt.Rows(0)(CType(tab.Items(i), TabItem).Name).ToString = "" Then
                    item.Visibility = Windows.Visibility.Collapsed
                Else
                    item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dt.Rows(0)(t.Name).ToString = "" Then
                            t.Visibility = Windows.Visibility.Collapsed
                        Else
                            t.Visibility = IIf(dt.Rows(0)(t.Name), Visibility.Visible, Visibility.Collapsed)
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Windows.Visibility.Visible
                        ElseIf dt.Rows(0)(t.Name).ToString = "" Then
                            t.Visibility = Windows.Visibility.Collapsed
                        Else
                            t.Visibility = IIf(dt.Rows(0)(t.Name), Visibility.Visible, Visibility.Collapsed)
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Windows.Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Private Sub PrintTbl(ByVal Header As String, ByVal tbl As String, Optional ByVal maintbl As String = "", Optional ByVal mainfield As String = "")
        Dim frm As New ReportViewer
        frm.Rpt = IIf(maintbl = "", "PrintTbl.rpt", "PrintTbl2.rpt")
        frm.paraname = {"Header", "@tbl", "@maintbl", "@mainfield"}
        frm.paravalue = {Header, tbl, maintbl, mainfield}
        frm.ShowDialog()
    End Sub

    Function MakePanel(MyHeader As String, ImagePath As String) As WrapPanel
        While MyHeader.Length < 16
            MyHeader = " " & MyHeader & " "
        End While
        CurrentTab += 1
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12"
        t.Style = FindResource("MyTabItem")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel

        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))
        ResizeHeader(G)
        t.Header = t.Header
        Return G
    End Function

    Private Sub LoadGPCs()
        Dim G As WrapPanel = MakePanel("File", "Traffic.jpg")

        Dim frm As New BasicForm With {.TableName = "PCs"}
        bm.SetImage(CType(frm, BasicForm).Img, "password.jpg")
        frm.txtName.MaxLength = 1000
        m.TabControl1.Items.Clear()
        LoadRadio(G, frm, "PCs")

    End Sub

    Private Sub LoadGFile()
        Dim s As String = "MainTraffic.jpg"

        Dim G As WrapPanel = MakePanel("ملف", s)
        Dim frm As UserControl

        LoadRadio(G, New Employees, "المستخدمين")

        LoadLabel(G, "")

        frm = New BasicForm With {.TableName = "LabelTypes"}
        LoadRadio(G, frm, "أنواع اللوحات")

        frm = New BasicForm With {.TableName = "CarTypes"}
        LoadRadio(G, frm, "أنواع السيارات")

        frm = New ViolationTypes
        LoadRadio(G, frm, "أنواع مخالفات السيارات")

        frm = New BasicForm With {.TableName = "AttachmentTypes"}
        LoadRadio(G, frm, "أنواع المرفقات")

        LoadLabel(G, "")


    End Sub


    Sub LoadDailyMotion()
        Dim s As String = "MainTraffic.jpg"
        Dim G As WrapPanel = MakePanel("إدارة المرور", s)
        Dim frm As UserControl

        frm = New Invoices
        LoadRadio(G, frm, "تسجيل المخالفات")

        frm = New Payment
        LoadRadio(G, frm, "تسجيل سداد المخالفات")

        frm = New InvoicesAttachments
        LoadRadio(G, frm, "تسجيل مرفقات السيارات")

        frm = New Invoices2
        LoadRadio(G, frm, "شهادة براءة الذمة")

        LoadLabel(G, "")

    End Sub

    Private Sub LoadGSecurity()
        Dim s As String = "MainTraffic.jpg"

        Dim G As WrapPanel = MakePanel("خيارات", s)
        Dim frm As UserControl

        frm = New ChangePassword
        LoadRadio(G, frm, "تغيير كلمة المرور")

        frm = New Levels
        LoadRadio(G, frm, "صلاحيات المستخدمين")

        frm = New Attachments
        LoadRadio(G, frm, "المرفقات")


    End Sub
     

    Private Sub LoadDailyMotionReports()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("تقارير إدارة المرور", s)
        Dim frm As UserControl

        AddHandler LoadRadio(G, frm, "المخالفات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT2 With {.Flag = 1}
                                                               sender.Tag = New Page With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(G, frm, "تعديلات المخالفات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT2 With {.Flag = 2}
                                                                       sender.Tag = New Page With {.Content = frm}
                                                                   End Sub


        AddHandler LoadRadio(G, frm, "شهادة براءة الذمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT2 With {.Flag = 3}
                                                                       sender.Tag = New Page With {.Content = frm}
                                                                   End Sub

        'AddHandler LoadRadio(G, frm, "شهادة براءة الذمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
        'frm = New RPT3 With {.Flag = 1}
        'sender.Tag = New Page With {.Content = frm}
        'End Sub
    End Sub

    Private Sub LoadTabs()

        LoadGFile()

        LoadDailyMotion()

        LoadGSecurity()

        LoadDailyMotionReports()
          

    End Sub


End Class

