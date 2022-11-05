Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Windows.Controls.Primitives
Imports System.Xml

Class MainWindow
    Dim bm As New BasicMethods
    Public Nlvl As Boolean = False
    Dim bol As Boolean = False
    Dim Copy As Boolean = False
    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If Nlvl Or bol Then Return
        If Copy = True Then
            bol = True
            Application.Current.Shutdown()
            Exit Sub
        End If
        bm.ClearTemp()
        If bm.ShowDeleteMSG("هل تريد الخروج من النظام؟") Then
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
        Else
            e.Cancel = True
            Me.BringIntoView()
        End If
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        FlowDirection = Windows.FlowDirection.RightToLeft
        'LoadResource()

        If Not LoadConnection() Then Return

        If Not MyProjectType = ProjectType.PCs Then
            If Md.Demo Then
                If Val(bm.ExecuteScalar("select (select count(*) from Attendance)+(select count(*) from BankCash)+(select count(*) from Cases)+(select count(*) from CustomerInvoices)+(select count(*) from Employees)+(select count(*) from Invoices)+(select count(*) from Entry)+(select count(*) from SalaryHistory)+(select count(*) from SalesMaster)+(select count(*) from Services)+(select count(*) from Reservations)+(select count(*) from Buildings)")) > 100 Then
                    bm.ShowMSG("عفوا .. انتهت فترة الاستخدام المجانى")
                    Application.Current.Shutdown()
                End If
            Else
                bm.TestProtection()
            End If
        End If

        Dim v As Integer = Val(bm.ExecuteScalar("select LastVersion from LastVersion"))
        If v > Md.LastVersion Or v = 0 Then
            bm.ShowMSG("MsgLastVersion")
            Application.Current.Shutdown()
        End If

        If Md.LastVersion > v Then
            bm.ExcuteNonQuery("delete from LastVersion insert into LastVersion (LastVersion) select " & Md.LastVersion)
        End If

        bm.ClearTemp()
        Md.CompanyName = bm.ExecuteScalar("select CompanyName from Statics")
        Md.CompanyTel = bm.ExecuteScalar("select CompanyTel from Statics")
        Dim L As New Login
        bm.SetImage(L.Img, "Login.jpg")
        LoadTabs(L)

    End Sub

    Public Sub LoadTabs(G As Object)
        Try
            MainGrid.Children.Clear()
            MainGrid.Children.Add(New Frame With {.Content = G})
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddTabOLD(ByVal M As MenuItem, ByVal L As UserControl)
        Dim Tab As New TabItem
        Tab.Header = M.Header
        Tab.Name = "Tab" & M.Name
        Tab.Content = L
        For Each it As TabItem In TabControl1.Items
            If it.Name = Tab.Name Then
                Tab = it
                TabControl1.SelectedItem = Tab
                Return
            End If
        Next
        TabControl1.Items.Add(Tab)
        TabControl1.SelectedItem = Tab
    End Sub

    'Add new tab --> mahmoud
    Public Sub AddTAB(ByVal M As MenuItem, ByVal UserCtrl As UserControl, Optional ByVal HaveClose As Boolean = True)
        Dim TabName As String = M.Name
        Dim TabHeader As String = M.Header
        Dim MW As MainWindow = Application.Current.MainWindow
        Dim TI As TabItem
        For I As Integer = 0 To MW.TabControl1.Items.Count - 1
            TI = MW.TabControl1.Items(I)
            If TI.Name = TabName Then
                TI.Focus()
                Exit Sub
            End If
        Next
        TI = New TabItem
        If HaveClose Then
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Visible}
        Else
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Hidden}
        End If
        Try
            CType(TI.Header, TabsHeader).Grid1.Children.Add(M.Icon)
        Catch ex As Exception
        End Try
        TI.Name = TabName
        TI.Content = UserCtrl
        MW.TabControl1.Items.Add(TI)
        TI.Focus()
    End Sub

    Function LoadConnection() As Boolean
        If con.State = ConnectionState.Open Then Return True
        Dim st As New StreamReader(Md.UdlName & ".udl")
        Dim s As String = ""
        st.ReadLine()
        st.ReadLine()
        s += st.ReadLine
        con.ConnectionString = s.Substring(20)
        Dim cb As New SqlClient.SqlConnectionStringBuilder(con.ConnectionString)
        Dim f As New Form1
        'con.ConnectionString = "Data Source=" & cb.DataSource & ";Initial Catalog=" & cb.InitialCatalog & ";Persist Security Info=True;User ID=" & cb.UserID & ";Password=" & cb.Password 'f.Password 
        con.ConnectionString = cb.ConnectionString
        Try
            con.Open()
        Catch ex As Exception
            bm.ShowMSG("Connection failed")
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
            Return False
        End Try
        cmd.Connection = con
        Return True
    End Function

    Public LogedIn As Boolean = False
    Public Flag As Integer = 1


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogout.Click
        Try
            If Not bm.ShowDeleteMSG("MsgExit") Then Return
            Forms.Application.Restart()
            Application.Current.Shutdown()
        Catch ex As Exception
        End Try
    End Sub

    
    Private Sub MainWindow_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = System.Windows.Input.Key.Enter Then
                'e.Handled = True
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Button) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Forms.Integration.WindowsFormsHost) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) Then
                    If CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then Return
                End If
                Dim c As Control = FocusManager.GetFocusedElement(Me)
                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})
                c.Focus()
                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})
                If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) AndAlso Not CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then CType(FocusManager.GetFocusedElement(Me), TextBox).SelectAll()
            End If
        Catch
        End Try
    End Sub

    Private Sub btnMinimize_Click(sender As Object, e As RoutedEventArgs) Handles btnMinimize.Click
        WindowState = Windows.WindowState.Minimized
    End Sub

End Class
