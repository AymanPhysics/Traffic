﻿Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Controls
Imports System.Diagnostics
Imports System.Management
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.Net

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Windows.Controls.Primitives
'Imports System.Runtime.integereropServices
Imports System.Reflection
Imports Microsoft.Office.Interop


Public Class BasicMethods
    '''''''''''''''''''''''''''''''''''''''''''''''''''
    Public stat As String = ""
    Dim da As New SqlDataAdapter()

    Public AppendWhere As String = ""
    Public DiscountKeyFiels As Integer
    
    Public Table_Name As String = ""
    Public Fields() As String = {}
    Public GeneralString As String = ""
    Public KeyFields() As String = {}
    Public control() As Control = {}
    Public ReturnedValues(,) As String = {}
    Public Values() As String = {}
    Public IsLoaded As Boolean = False
    Public SelectedRow As System.Data.DataRowView
    Dim Path As String = System.Windows.Forms.Application.StartupPath & "\Temp\"


    Sub AccNoLostFocus(ByVal txtId As TextBox, ByVal txtName As TextBox, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0, Optional HideLinkedAccs As Boolean = False)
        If txtId.Visibility = Visibility.Hidden Then Return
        If txtId.Text.Trim = "" Or txtId.Text.Trim = "0" Or txtId.Visibility = Visibility.Hidden Then
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter("select Name from Chart where Id='" & txtId.Text.Trim() & "'")
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        Dim s As String = IIf(HideLinkedAccs, -1, 0)
        dt = ExcuteAdapter("select Name from Chart where Id='" & txtId.Text.Trim() & "' and (SubType=" & SubType & " or " & SubType & "=-1) and (LinkFile=" & LinkFile & " or " & LinkFile & "=" & s & ")")
        If dt.Rows.Count = 0 Then
            If SubType = 1 Then
                ShowMSG("Please, Select a Valid Account")
            ElseIf SubType = 0 Then
                ShowMSG("This isn't a General Account")
            End If
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        txtName.Text = dt.Rows(0)(0).ToString
    End Sub

    Public Function AccNoShowHelp(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0, Optional HideLinkedAccs As Boolean = False) As Boolean
        Dim s As String = IIf(HideLinkedAccs, -1, 0)
        Return ShowHelp("Accounts", txtId, txtName, e, "select cast(Id as varchar(100)) ID,Name from Chart where (SubType=" & SubType & " or " & SubType & "=-1) and (LinkFile=" & LinkFile & " or " & LinkFile & "=" & s & ")")
    End Function


    Sub AccNoLostFocusGrid(ByVal Cell As Forms.DataGridViewCell, ByVal txtName As Label, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0)
        If Val(Cell.Value) = 0 Or Cell.ReadOnly Then
            Cell.Value = ""
            txtName.Content = ""
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter("select Name from Chart where Id='" & Cell.Value.ToString & "'")
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            Cell.Value = ""
            txtName.Content = ""
            Return
        End If

        dt = ExcuteAdapter("select Name from Chart where Id='" & Cell.Value.ToString & "' and (SubType=" & SubType & " or " & SubType & "=-1) and (LinkFile=" & LinkFile & " or " & LinkFile & "=0)")
        If dt.Rows.Count = 0 Then
            If SubType = 1 Then
                ShowMSG("Please, Select a Valid Account")
            ElseIf SubType = 0 Then
                ShowMSG("This isn't a General Account")
            End If
            Cell.Value = ""
            txtName.Content = ""
            Return
        End If
        txtName.Content = dt.Rows(0)(0).ToString
    End Sub


    Public Function AccNoShowHelpGrid(ByVal Column1 As Forms.DataGridViewCell, txtName As Label, ByVal e As System.Windows.Forms.KeyEventArgs, Optional ByVal SubType As Integer = 1, Optional ByVal LinkFile As Integer = 0, Optional HideLinkedAccs As Boolean = False) As Boolean
        Dim s As String = IIf(HideLinkedAccs, -1, 0)
        Return ShowHelpGrid("Accounts", Column1, txtName, e, "select cast(Id as varchar(100)) ID,Name from Chart where (SubType=" & SubType & " or " & SubType & "=-1) and (LinkFile=" & LinkFile & " or " & LinkFile & "=" & s & ")")
    End Function

    Sub CostCenterIdLostFocus(ByVal txtId As TextBox, ByVal txtName As TextBox, Optional ByVal SubType As Integer = 1)
        If txtId.Text.Trim = "" Or txtId.Text.Trim = "0" Or txtId.Visibility = Visibility.Hidden Then
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter("select Name from CostCenters where Id=" & txtId.Text.Trim())
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        dt = ExcuteAdapter("select Name from CostCenters where Id=" & txtId.Text.Trim() & " and SubType=" & SubType)
        If dt.Rows.Count = 0 Then
            If SubType = 1 Then
                ShowMSG("Please, Select a Valid Cost Center")
            ElseIf SubType = 0 Then
                ShowMSG("This isn't a General Cost Center")
            End If
            txtId.Clear()
            txtName.Clear()
            Return
        End If

        txtName.Text = dt.Rows(0)(0).ToString
    End Sub

    Sub CostCenterIdLostFocusGrid(ByVal Column1 As Forms.DataGridViewCell, ByVal txtName As Label, Optional ByVal SubType As Integer = 1)
        If Val(Column1.Value) = 0 Or Column1.ReadOnly Then
            Column1.Value = ""
            txtName.Content = ""
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter("select Name from CostCenters where Id=" & Val(Column1.Value))
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            Column1.Value = ""
            txtName.Content = ""
            Return
        End If

        dt = ExcuteAdapter("select Name from CostCenters where Id=" & Val(Column1.Value) & " and SubType=" & SubType)
        If dt.Rows.Count = 0 Then
            If SubType = 1 Then
                ShowMSG("Please, Select a Valid Cost Center")
            ElseIf SubType = 0 Then
                ShowMSG("This isn't a General Cost Center")
            End If
            Column1.Value = ""
            txtName.Content = ""
            Return
        End If
        txtName.Content = dt.Rows(0)(0).ToString
    End Sub

    Public Function CostCenterIdShowHelp(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs, Optional ByVal SubType As Integer = 1) As Boolean
        Return ShowHelp("CostCenters", txtId, txtName, e, "select cast(Id as varchar(100)) ID,Name from CostCenters where SubType=" & SubType)
    End Function


    Public Function TestIsLoaded(M As Object, Optional Fource As Boolean = False) As Boolean

        If Not M Is Nothing Then
            M.FlowDirection = Application.Current.MainWindow.FlowDirection
        End If

        'If Md.MyProject = Client.ClothesRed Then
        '    SetColor(M.Content)
        'End If

        If IsLoaded Then Return True
        If Fource Then IsLoaded = True
        Return False
    End Function

    Public Function ShowHelp(ByVal Header As String, ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs, ByVal Statement As String, Optional ByVal TableName As String = "", Optional MyFields() As String = Nothing, Optional MyValues() As String = Nothing, Optional ByVal FirstColumn As String = "", Optional ByVal SecondColumn As String = "") As Boolean
        If e Is Nothing OrElse e.Key = System.Windows.Input.Key.F1 Then
            Dim hh As New Help
            SelectedRow = Nothing
            hh.Header = Header
            If FirstColumn <> "" Then hh.FirstColumn = FirstColumn
            If SecondColumn <> "" Then hh.SecondColumn = SecondColumn
            If hh.Header = "" Then hh.Header = Header
            hh.MyFields = MyFields
            hh.MyValues = MyValues
            hh.Statement = Statement
            hh.TableName = TableName
            hh.ShowDialog()
            SelectedRow = hh.SelectedRow
            If hh.SelectedId = 0 Then Return False
            txtId.Text = hh.SelectedId
            If Not txtId Is txtName Then txtName.Text = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function


    Public Function ShowHelpGrid(ByVal Header As String, ByVal Column1 As Forms.DataGridViewCell, ByVal Column2 As Forms.DataGridViewCell, ByVal e As Forms.KeyEventArgs, ByVal Statement As String, Optional ByVal TableName As String = "", Optional ByVal FirstColumn As String = "", Optional ByVal SecondColumn As String = "") As Boolean
        If e.KeyCode = Forms.Keys.F1 Then
            Dim hh As New Help
            hh.Header = Header
            If FirstColumn <> "" Then hh.FirstColumn = FirstColumn
            If SecondColumn <> "" Then hh.SecondColumn = SecondColumn
            hh.Statement = Statement
            hh.TableName = TableName
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            Column1.Value = hh.SelectedId
            If Not Column1 Is Column2 Then Column2.Value = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ShowHelpGridItemBal(ByVal Column1 As Forms.DataGridViewCell, ByVal Column2 As Forms.DataGridViewCell, ByVal e As Forms.KeyEventArgs, ByVal Statement As String, Optional ByVal TableName As String = "") As Boolean
        If e.KeyCode = Forms.Keys.F12 Then
            Dim hh As New Help
            hh.Header = "الأرصدة"
            hh.Statement = Statement
            hh.TableName = TableName
            hh.txtID.Visibility = Visibility.Hidden
            hh.txtName.Visibility = Visibility.Hidden
            hh.Show()
            hh.DataGridView1.Columns(0).Visibility = Visibility.Hidden
            hh.DataGridView1.Columns(2).Visibility = Visibility.Hidden
            hh.DataGridView1.Columns(4).Visibility = Visibility.Hidden
            hh.Hide()
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            'If ShowDeleteMSG("هل تريد عمل طلب تحويل؟") Then
            '    ExcuteNonQuery("" & hh.SelectedRow(0) & hh.SelectedRow(2))
            '    Column1.Value = hh.SelectedId
            '    Column2.Value = hh.SelectedName
            'End If


            Return True
        Else
            Return False
        End If
    End Function


    Public Function ShowHelpCustomers(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal e As System.Windows.Input.KeyEventArgs) As Boolean
        If e.Key = System.Windows.Input.Key.F1 Then
            Dim hh As New HelpCustomers
            hh.Header = Application.Current.MainWindow.Resources.Item("Customers")
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            txtId.Text = hh.SelectedId
            txtName.Text = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function


    Public Function ShowHelpGrid(ByVal Header As String, ByVal Column1 As Forms.DataGridViewCell, ByVal txtName As Label, ByVal e As Forms.KeyEventArgs, ByVal Statement As String, Optional ByVal TableName As String = "") As Boolean
        If e.KeyCode = Forms.Keys.F1 Then
            Dim hh As New Help
            hh.Header = Header
            hh.Statement = Statement
            hh.TableName = TableName
            hh.ShowDialog()
            If hh.SelectedId = 0 Then Return True
            Column1.Value = hh.SelectedId
            txtName.Content = hh.SelectedName
            Return True
        Else
            Return False
        End If
    End Function



    Sub LostFocus(ByVal txtId As TextBox, ByVal txtName As TextBox, ByVal Statement As String, Optional IgnoreVisibility As Boolean = False)
        If txtId.Text.Trim = "" Or txtId.Text.Trim = "0" Or (Not txtId.Visibility = Visibility.Visible And Not IgnoreVisibility) Then
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter(Statement)
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            txtId.Clear()
            txtName.Clear()
            Return
        End If
        txtName.Text = dt.Rows(0)(0).ToString
    End Sub

    Sub LostFocusGrid(ByVal Column1 As Forms.DataGridViewCell, ByVal txtName As Label, ByVal Statement As String, Optional IgnoreVisibility As Boolean = False)
        If Val(Column1.Value) = 0 Or Column1.ReadOnly Then
            Column1.Value = ""
            txtName.Content = ""
            Return
        End If
        Dim dt As DataTable = ExcuteAdapter(Statement)
        If dt.Rows.Count = 0 Then
            ShowMSG("هذا الرقم غير صحيح")
            Column1.Value = ""
            txtName.Content = ""
            Return
        End If
        txtName.Content = dt.Rows(0)(0).ToString
    End Sub

    Public Sub ApplyOpenCombobox(ByVal cc As ComboBox())
        For Each c As ComboBox In cc
            'AddHandler c.KeyDown, AddressOf MyOpenCombobox
            AddHandler c.KeyUp, AddressOf MyFilterCombobox
            AddHandler c.GotFocus, AddressOf MyGotFocus
        Next
    End Sub

    Public Sub MyKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = System.Windows.Input.Key.Enter Then
            e.Handled = True
            'CType(sender, Control).MoveFocus(New System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next))

            InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})

        End If
    End Sub



    Private Sub MyGetFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            If sender.GetType().ToString() = "System.Windows.Controls.PasswordBox" Then
                CType(sender, PasswordBox).SelectAll()
            ElseIf sender.GetType().ToString() = "System.Windows.Controls.TextBox" Then
                CType(sender, TextBox).SelectAll()
            End If
        Catch ex As Exception
        End Try
    End Sub


    Public Sub MyKeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs, Optional ByVal IsDecimal As Boolean = False)
        If e.Key = System.Windows.Input.Key.Enter Or e.Key = System.Windows.Input.Key.Tab Then Return
        If IsDecimal Then
            Dim s As Controls.TextBox = sender
            Dim ddd = Chr(e.Key)
            If (e.Key = Windows.Input.Key.OemPeriod Or e.Key = Windows.Input.Key.Decimal) AndAlso Not s.Text.Contains(".") Then
                Return
            End If
            If (e.Key = Windows.Input.Key.OemMinus Or e.Key = Windows.Input.Key.Subtract) Then
                s.Text = -Val(s.Text)
                e.Handled = True
                Return
            End If
        End If
        If Not e.Key = Key.F1 AndAlso Not IsNumeric(e.Key.ToString.Replace("NumPad", "").Replace("D", "")) AndAlso Not e.Key = Key.Space Then
            e.Handled = True
            'CType(sender, TextBox).Undo()
        End If
    End Sub



    Public Structure SYSTEMTIME
        Public wYear As UInt16
        Public wMonth As UInt16
        Public wDayOfWeek As UInt16
        Public wDay As UInt16
        Public wHour As UInt16
        Public wMinute As UInt16
        Public wSecond As UInt16
        Public wMilliseconds As UInt16
    End Structure

    Public Sub SetTime()
        Try
            Dim dd As DateTime = CType(ExecuteScalar("select getdate()"), DateTime)
            Today = dd
            TimeOfDay = dd
        Catch ex As Exception
            ShowMSG("Please, Run As Administrator" & vbCrLf & "to enable Time Updating")
        End Try
    End Sub

    Public Function MyGetDate() As DateTime
        Return CType(ExecuteScalar("select dbo.MyGetDate()"), DateTime)
    End Function



    Public Function Physics_processorId() As String
        Dim s As String = ""
        Dim search As New ManagementObjectSearcher(New SelectQuery("Win32_processor"))
        For Each info As ManagementObject In search.Get()
            Try
                s &= info("processorId").ToString()
            Catch ex As Exception
            End Try
        Next
        Return s.ToUpper
    End Function

    Public Function EnName(ByVal ArName As String, ByVal dtName As DataTable) As String
        ArName = ArName.Trim
        While ArName.Contains("  ")
            ArName = ArName.Replace("  ", " ")
        End While
        Dim s() As String
        s = ArName.Split(" ")
        Dim En As String = ""
        For i As Integer = 0 To s.Length - 1

            'Dim a As String = ExecuteScalar("select top 1 English_Name from Names where Arabic_Name='" & s(i) & "'")
            Dim a As String = ""
            Try
                a = (dtName.Select("Arabic_Name='" & s(i) & "'")(0))(1)
            Catch ex As Exception
                a = s(i)
            End Try
            If a = "" Then
                En &= s(i)
            Else
                En &= a
            End If
            En &= " "
        Next
        Return En.Trim
    End Function


    Public Function Physics_SerialNumber() As String
        Dim s As String = ""
        Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia")
        For Each wmi_HD As ManagementObject In searcher.Get()
            If wmi_HD("SerialNumber") <> Nothing Then
                s &= wmi_HD("SerialNumber").ToString()
                Exit For
            End If
        Next
        Return s.ToUpper
    End Function


    Public Function Physics_BaseBoard() As String
        Dim s As String = ""
        Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard")
        For Each wmi_HD As ManagementObject In searcher.Get()
            If wmi_HD("SerialNumber") <> Nothing Then
                s &= wmi_HD.Properties("SerialNumber").ToString()
            End If
            If wmi_HD("Product") <> Nothing Then
                s &= wmi_HD.Properties("Product").Value.ToString().Trim()
            End If
        Next

        searcher.Dispose()

        Return s.ToUpper
    End Function

    Public Function Physics_VolumeSerialNumber(ByVal Volume As String) As String
        Dim s As String = ""
        Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", Volume))
        Try
            disk.Get()
        Catch ex As Exception
        End Try
        Try
            s &= disk("VolumeSerialNumber").ToString()
        Catch ex As Exception
        End Try
        Return s
    End Function

    Public Function Physics_MACAddress() As String
        Dim s As String = ""
        Dim mo2 As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration")
        For Each mac As ManagementObject In mo2.Get()
            Try
                s &= mac("MACAddress").ToString.Replace(":", "")
            Catch ex As Exception
            End Try
            If s.Length > 0 Then Exit For
        Next
        Return s
    End Function

    Public Function Physics_MacAddress2() As String
        Dim s As String = ""
        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim MACAddress As String = ""
        For Each mo As ManagementObject In moc
            If (MACAddress.Equals(String.Empty)) Then
                If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()
                mo.Dispose()
            End If
            Try
                s &= MACAddress.Replace(":", String.Empty)
            Catch ex As Exception
            End Try
            If s.Length > 0 Then Exit For
        Next
        Return s
    End Function

    Public Function Protect(ByVal s As String) As String
        Return s.Replace(" ", "").Replace("0", "P").Replace("1", "K").Replace("2", "U").Replace("3", "G").Replace("4", "R").Replace("5", "D").Replace("6", "W").Replace("7", "Q").Replace("8", "A").Replace("9", "Z").ToUpper()
    End Function

    Public Function ProtectionSerial() As String
        Return (Physics_BaseBoard() & Physics_processorId()).Trim() '& Physics_MACAddress() & Physics_SerialNumber()
    End Function
    Public Sub TestProtection()
        Dim frm As New Form1
        frm.BackgroundWorker1.RunWorkerAsync()
    End Sub


    Enum CloseState
        Yes
        No
        Cancel
    End Enum

    Public Function RequestDelete() As CloseState
        If Md.FourceExit Then Return CloseState.No
        Dim frm As New CloseForm
        frm.ShowDialog()
        Return frm.State
    End Function


    Public Function ShowForm(ByVal parent As Window, ByVal frm As Window, ByVal Caption As String, ByVal p As Point)
        Return frm

    End Function

    Public Sub CloseTab(ByVal TabName)
        Dim MW As MainWindow = Application.Current.MainWindow
        Dim TI As TabItem
        For I As Integer = 0 To MW.TabControl1.Items.Count - 1
            TI = MW.TabControl1.Items(I)
            If TI.Name = TabName Then
                MW.TabControl1.Items.RemoveAt(I)
                Exit Sub
            End If
        Next
    End Sub

    Sub HandleKeyDown(ByVal frm As Window)
        'For Each C As Control In frm.Controls
        '    If C.GetType Is GetType(TextBox) Then
        '        Dim cc As TextBox = C
        '        If cc.Multiline Then
        '            Continue For
        '        End If
        '    End If
        '    AddHandler C.KeyPress, AddressOf _KeyPress
        'Next
    End Sub


    Public Sub _KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then e.Handled = True
    End Sub
    Public Sub DefineValues()
        ReDim Values(control.Length)
        Dim type As String = "", nn As String = ""
        For i As Integer = 0 To control.Length - 1
            'val = CType(control(i), Control).Text.Trim()
            type = control(i).GetType().ToString().Trim
            nn = control(i).Name
            If (type = "System.Windows.Controls.ComboBox") Then
                Dim cbo As New ComboBox
                cbo = control(i)
                If cbo.SelectedValuePath = "" Then
                    Values(i) = cbo.SelectedIndex.ToString().Trim
                Else
                    If IsNothing(cbo.SelectedValue) Then cbo.SelectedValue = 0
                    Values(i) = cbo.SelectedValue.ToString().Trim
                End If
            ElseIf (type = "System.Windows.Controls.CheckBox") Then
                Dim chk As New CheckBox()
                chk = control(i)
                If (chk.IsChecked) Then
                    Values(i) = "1"
                Else
                    Values(i) = "0"
                End If

            ElseIf (type = "System.Windows.Controls.RadioButton") Then
                Dim rbd As New RadioButton()
                rbd = control(i)
                If (rbd.IsChecked) Then
                    Values(i) = "1"
                Else
                    Values(i) = "0"
                End If

            ElseIf type = "System.Windows.Controls.DatePicker" Then

                Dim dd As DatePicker = control(i)
                If dd.SelectedDate Is Nothing Then
                    Values(i) = Nothing
                Else
                    Values(i) = ToStrDate(dd.SelectedDate)
                End If

                
            ElseIf type = "System.Windows.Controls.PasswordBox" Then
                Values(i) = Encrypt(CType(control(i), PasswordBox).Password.Trim())
            ElseIf type = "System.Windows.Controls.RichTextBox" Then
                Values(i) = GetString(CType(control(i), RichTextBox))
            ElseIf Table_Name = "PCs" And control(i).Name = "txtName" Then
                Values(i) = Encrypt(CType(control(i), TextBox).Text.Trim())
            Else
                Values(i) = CType(control(i), TextBox).Text.Trim()
            End If


            Try
                Values(i) = Values(i).Replace("'", "''")
            Catch
            End Try
        Next

    End Sub

    Enum SaveState
        All
        Insert
        Update
        Print
        Close
    End Enum

    Function GetString(rtb As RichTextBox) As String

        Dim txt As New TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd)
        Return txt.Text
    End Function


    Public Function Save(ByVal ID() As String, ByVal IDValue() As String, Optional ByVal State As SaveState = SaveState.All) As Boolean
        ' DefineValues()
        If Not StopPro() Then Return False
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            'main.sqlConnection1.Open()
            MyCmd.CommandType = CommandType.Text
            MyCmd.Parameters.Clear()
            MyCmd.CommandText = ""
            If State <> SaveState.Insert Then
                MyCmd.CommandText = "IF Exists(Select *  From " & Table_Name & " T Where " & ID(0) & "='" & IDValue(0) & "'"
                For i As Integer = 1 To ID.Length - 1
                    MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
                Next

                MyCmd.CommandText &= " " & AppendWhere & ")"
                MyCmd.CommandText &= " Update " & Table_Name & "  Set UserName='" & Md.UserName & "',MyGetDate=GetDate(),"
                For i As Integer = 0 To Fields.Length - 1
                    MyCmd.CommandText &= Fields(i) & "='" & Values(i) & "'"
                    If Not i = Fields.Length - 1 Then
                        MyCmd.CommandText &= ","
                    End If
                Next
                MyCmd.CommandText &= " Where " & ID(0) & "='" & IDValue(0) & "'"

                For i As Integer = 1 To ID.Length - 1
                    MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
                Next
                MyCmd.CommandText &= AppendWhere
            End If

            If State = SaveState.All Then
                MyCmd.CommandText &= " Else "
            End If

            If State <> SaveState.Update Then
                MyCmd.CommandText &= "Insert into " & Table_Name & "(UserName,MyGetDate," & Table_Fields() & " ) Values ('" & Md.UserName & "',GetDate(),'"
                For i As Integer = 0 To Fields.Length - 1

                    MyCmd.CommandText &= Values(i)
                    If Not i = Fields.Length - 1 Then
                        MyCmd.CommandText &= "', '"
                    Else : MyCmd.CommandText &= "')"
                    End If
                Next
            End If

            MyCmd.ExecuteNonQuery()
            MyCmd.Connection.Close()
            Return True

        Catch ee As Exception

            Dim ss As String = ee.Message.ToString().Trim
            ShowMSG(ss)
            Return False

        Finally
            c = Nothing
        End Try
    End Function

    Public Sub SearchTable(ByVal ID() As String, ByVal IDValue() As String, Cbo As ComboBox, Optional SpName As String = "", Optional UsedControls() As Control = Nothing)
        Try
            Dim str As String = ""

            str = "Select " & ID(ID.Length - 1) & " Id,cast(" & IIf(SpName <> "", SpName, ID(ID.Length - 1)) & " as varchar(100)) Name  From " & Table_Name & " T Where 1=1 "
            For i As Integer = KeyFields.Length To Fields.Length - 1
                If Values(i) = "" OrElse (IsDate(Values(i)) AndAlso Len(Values(i)) = 10) Then Continue For
                If (UsedControls Is Nothing OrElse UsedControls.Contains(control(i))) AndAlso control(i).Visibility = Visibility.Visible Then
                    str &= " and " & Fields(i) & "='" & Values(i) & "'"
                End If
            Next

            For i As Integer = 0 To ID.Length - 2
                str &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next
            str &= AppendWhere & " union select 0 Id,'-' Name"

            FillCombo(str, Cbo)
        Catch ee As Exception
            Dim ss As String = ee.Message.ToString().Trim
            ShowMSG(ss)
        End Try
    End Sub

    Public Function SaveGrid(ByVal Grid As Forms.DataGridView, ByVal TableName As String, ByVal KeysFields() As String, ByVal KeysValues() As String, ByVal Fields() As String, ByVal ColumnsNames() As String, ByVal Type() As VariantType, ByVal ColumnsKeys() As String) As Boolean
        'SaveGrid(New DataGridView, "", New Integer() {}, New VariantType() {VariantType.String})
        Dim Statement As String = "Delete From " & TableName & " where 1=1" & vbCrLf
        For i As Integer = 0 To KeysFields.Length - 1
            Statement &= " and " & KeysFields(i) & "='" & KeysValues(i) & "'" & vbCrLf
        Next

        Dim Statement2 As String = ""
        Statement2 &= " Insert " & TableName & "(UserName,MyGetDate,"
        For i As Integer = 0 To KeysFields.Length - 1
            Statement2 &= KeysFields(i) & ","
        Next
        For i As Integer = 0 To Fields.Length - 1
            Statement2 &= Fields(i) & ","
        Next
        Statement2 = Mid(Statement2, 1, Len(Statement2) - 1)
        Statement2 &= ") values " & vbCrLf

        Dim Statement3 As String = ""
        For i As Integer = 0 To Grid.Rows.Count - 1 - IIf(Grid.AllowUserToAddRows, 1, 0)

            For x As Integer = 0 To ColumnsKeys.Length - 1
                If Grid.Rows(i).Cells(ColumnsKeys(x)).Value Is Nothing OrElse Grid.Rows(i).Cells(ColumnsKeys(x)).Value.ToString = "" Then GoTo EndFor
            Next

            Statement3 &= IIf(Statement3 = "", "", ",")

            Statement3 &= "('" & Md.UserName & "',GetDate(),"
            For x As Integer = 0 To KeysValues.Length - 1
                Statement3 &= "'" & KeysValues(x) & "',"
            Next
            For x As Integer = 0 To ColumnsNames.Length - 1
                Statement3 &= GetCellValue(Type(x), Grid.Rows(i).Cells(ColumnsNames(x)).Value)
                Statement3 &= IIf(x = ColumnsNames.Length - 1, "", ",")
            Next
            Statement3 &= ")"

EndFor:
        Next

        If Statement3 = "" Then Statement2 = ""
        Return ExcuteNonQuery(Statement & Statement2 & Statement3)
    End Function

    Function GetCellValue(ByVal Type As VariantType, ByVal Value As String) As String
        Try
            Value = Value.Replace("'", "''")
        Catch ex As Exception
        End Try
        Select Case Type
            Case VariantType.Date
                Try
                    If Value Is Nothing Then Return "null"
                    Return "'" & ToStrDate(DateTime.Parse(Value)) & "'"
                Catch ex As Exception
                    Return "null"
                End Try
            Case VariantType.Integer
                Return Val(Value)
            Case VariantType.Decimal
                Return Val(Value)
            Case VariantType.Boolean
                Return IIf(Value, 1, 0)
            Case VariantType.String
                Return "'" & Value & "'"
            Case Else
                Return "''"
        End Select
    End Function

    '___________________________Check IF Record Whith Condition is Exist__________________
    Public Function IF_Exists() As Boolean

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim dt As New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandText = ""
            MyCmd.CommandText = " IF Exists ( Select * From " & Table_Name & " " & WhereKeyFields() & " ) select '1' else select '0'"

            da.SelectCommand = MyCmd
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            If dt.Rows(0)(0).ToString().Trim = "1" Then
                Return True
            Else : Return False
            End If

        Catch
            Return False
        End Try
    End Function
    Public Function IF_Exists(ByVal SqlStatment As String) As Boolean

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim dt As New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = ""
            MyCmd.CommandText = " IF Exists (" & SqlStatment & ") select '1' else select '0'"

            da.SelectCommand = MyCmd
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            If dt.Rows(0)(0).ToString().Trim = "1" Then

                Return True
            Else : Return False
            End If

        Catch
            Return False
        Finally
            c = Nothing
        End Try
    End Function

    Public Sub FillControls()

        Try

            Dim Type As String = ""
            For i As Integer = 0 To Fields.Length - 1
                Type = control(i).GetType().ToString().Trim
                Dim nn As String = control(i).Name
                If Type = "System.Windows.Controls.ComboBox" Then
                    Dim cbo As ComboBox = control(i)
                    If (cbo.ItemsSource Is Nothing) Then    'for save integereger no. in DB. from combox
                        Dim index As String = ReturnedValues(0, i)
                        If Not (ReturnedValues(0, i).Trim() = "") Then
                            Try
                                cbo.SelectedIndex = Integer.Parse(ReturnedValues(0, i))
                            Catch ex As Exception
                            End Try
                        End If
                    Else
                        Try
                            cbo.SelectedIndex = 0
                            cbo.SelectedValue = ReturnedValues(0, i)
                        Catch ex As Exception
                            cbo.SelectedIndex = -1
                        End Try
                    End If
                ElseIf (Type = "System.Windows.Controls.CheckBox") Then

                    Dim chk As CheckBox = control(i)
                    Dim val As String = ReturnedValues(0, i)
                    If (ReturnedValues(0, i) = "1" Or ReturnedValues(0, i) = "True") Then
                        chk.IsChecked = True
                    Else : chk.IsChecked = False
                    End If
                ElseIf (Type = "System.Windows.Controls.RadioButton") Then
                    Dim rbt As RadioButton = control(i)
                    If (ReturnedValues(0, i) = "1") Then
                        rbt.IsChecked = True

                    Else : rbt.IsChecked = False

                    End If
                ElseIf (Type = "System.Windows.Controls.PasswordBox") Then
                    Dim txt As PasswordBox = control(i)
                    Try
                        CType(control(i), PasswordBox).Password = Decrypt(ReturnedValues(0, i))
                    Catch
                        CType(control(i), PasswordBox).Password = ""
                    End Try
                ElseIf (Type = "System.Windows.Controls.DatePicker") Then
                    Dim txt As DatePicker = control(i)
                    Try
                        If DateTime.Parse(ReturnedValues(0, i)) = DateTime.Parse("01/01/1900") Then
                            CType(control(i), DatePicker).SelectedDate = Nothing
                        Else
                            Try
                                CType(control(i), DatePicker).SelectedDate = ReturnedValues(0, i)
                            Catch ex As Exception
                                CType(control(i), DatePicker).SelectedDate = Nothing
                            End Try
                        End If
                    Catch ex As Exception

                    End Try
                ElseIf (Table_Name = "PCs" And control(i).Name = "txtName") Then
                    Dim txt As TextBox = control(i)
                    CType(control(i), TextBox).Text = Decrypt(ReturnedValues(0, i))
                Else : CType(control(i), TextBox).Text = ReturnedValues(0, i)
                End If
            Next

        Catch ex As Exception
        End Try
    End Sub
    Public Sub RetrieveAll(ByVal ID() As String, ByVal IDValue() As String, ByRef dt As DataTable)   ' k is the control focus if not exists

        '			if(!TestEmpt())
        '				return

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            dt = New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.Parameters.Clear()
            MyCmd.CommandText = ""
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText &= "Select " & Table_Fields() & " From " & Table_Name & " T "
            MyCmd.CommandText &= " Where " & ID(0) & "='" & IDValue(0) & "'" & AppendWhere

            For i As Integer = 1 To ID.Length - 1
                MyCmd.CommandText &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next

            da.SelectCommand = MyCmd
            dt = New DataTable
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            FillValues(dt)

        Catch ex As Exception
            Dim ss As String = ex.Message.ToString().Trim
            ShowMSG(ex.ToString().Trim)
        Finally
            c = Nothing
        End Try

    End Sub



    '___________________________________________________________________________________
    '___________________________Fill Retuened Values ___________________________________
    Public Sub FillValues(ByVal dt As DataTable)

        'Dim ReturnedValues(0, Fields.Length) As String
        If (dt.Rows.Count = 0) Then
            Return
        End If

        ReDim ReturnedValues(dt.Rows.Count, Fields.Length)
        For j As Integer = 0 To dt.Rows.Count - 1
            For i As Integer = 0 To Fields.Length - 1
                ReturnedValues(j, i) = dt.Rows(j)(i).ToString().Trim()
            Next
        Next
    End Sub

    Public Sub FirstLast(ByVal ID() As String, ByVal MaxOrMin As String, ByRef dt As DataTable)
        DefineValues()
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            dt = New DataTable
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.Parameters.Clear()
            MyCmd.CommandText = "Select " & Table_Fields() & " From " & Table_Name & " T "

            Dim MyKeys As String = ""

            DiscountKeyFiels = 1
            MyCmd.CommandText &= WhereKeyFields()

            Dim KeyField As String = ID(KeyFields.Length - 1).Replace("T.", "") 'KeyFields[KeyFields.Length-1]
            MyCmd.CommandText &= " " & AppendWhere & " and " & KeyField & " =(select " & MaxOrMin & "(" & KeyField & ") From " & Table_Name & " " & WhereKeyFields() & " " & AppendWhere & " ) "

            DiscountKeyFiels = 0
            da.SelectCommand = MyCmd
            dt = New DataTable
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()
            FillValues(dt)

        Catch ex As Exception
            Dim ss As String = ex.Message.ToString().Trim
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub NextPrevious(ByVal ID() As String, ByVal Value() As String, ByVal NextOrBack As String, ByRef dt As DataTable)
        DefineValues()
        dt = New DataTable
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Dim MyCmd As SqlCommand = c.CreateCommand()
        If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

        MyCmd.Parameters.Clear()
        MyCmd.CommandType = CommandType.Text
        Dim MaxOrMin As String = "min"
        Dim Compare As String = ">"
        If (NextOrBack = "Next") Or (NextOrBack = "next") Then

            MaxOrMin = "min"
            Compare = " > "
        ElseIf (NextOrBack = "Back") Or (NextOrBack = "back") Then
            MaxOrMin = "max"
            Compare = " < "

        Else : Return
        End If
        Try

            dt = New DataTable
            MyCmd.CommandText = "Select " & Table_Fields() & " From " & Table_Name & " T "
            Dim KeyField As String = ID(KeyFields.Length - 1)
            KeyField = KeyField.Replace("T.", "")

            DiscountKeyFiels = 1
            MyCmd.CommandText &= WhereKeyFields() & AppendWhere

            MyCmd.CommandText &= " and " & KeyFields(KeyFields.Length - 1) & "=("
            MyCmd.CommandText &= "Select " & MaxOrMin & "(" & KeyField & ") From " & Table_Name
            MyCmd.CommandText &= WhereKeyFields()
            DiscountKeyFiels = 0

            If (Value(KeyFields.Length - 1) = "") Then
                Compare = " > "
            End If
            MyCmd.CommandText &= " and " & KeyFields(KeyFields.Length - 1) & Compare & "'" & Value(KeyFields.Length - 1) & "' " & AppendWhere & ")"

            da.SelectCommand = MyCmd
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()
            If (dt.Rows.Count = 0) Then
                Return
            End If
            FillValues(dt)

        Catch ex As Exception
        Finally
            c = Nothing
        End Try
    End Sub

    Dim EventHasAdded As Boolean = False
    Public Sub ClearControls(Optional ByVal Focus As Boolean = True)
        Addcontrol_MouseDoubleClick(control)


        Dim type As String = ""
        For i As Integer = 0 To control.Length - 1
            Dim s As String = control(i).Name

            For n As Integer = 0 To KeyFields.Length - 1
                If KeyFields(n) = Fields(i) Then
                    GoTo A
                End If
            Next

            type = control(i).GetType().ToString().Trim
            If (type = "System.Windows.Controls.ComboBox") Then

                Dim cbo As ComboBox = control(i)
                Try
                    cbo.SelectedIndex = 0
                Catch
                End Try
            ElseIf (type = "System.Windows.Controls.CheckBox") Then

                Dim chk As CheckBox = control(i)
                chk.IsChecked = False

            ElseIf (type = "System.Windows.Controls.PictureBox") Then

                'Dim chk As PictureBox = control(i)
                'chk.Image = Image.FromFile(Application.Current.StartupUri.ToString & "\Images\no.photo.gif")

            ElseIf (type = "System.Windows.Controls.RadioButton") Then
                Dim chk As RadioButton = control(i)
                chk.IsChecked = False
            ElseIf (type = "System.Windows.Controls.DatePicker") Then
                Dim chk As DatePicker = control(i)
                chk.SelectedDate = Nothing
            ElseIf (type = "System.Windows.Controls.PasswordBox") Then
                Dim chk As PasswordBox = control(i)
                chk.Password = ""
            Else : CType(control(i), TextBox).Text = ""
            End If
A:
        Next
        If Focus Then control(0).Focus()
    End Sub




    '----------------------Select Items ON Conditions-----------------------------------
    Private Function WhereKeyFields() As String

        GeneralString = ""
        GeneralString &= " Where 1=1 "
        For j As Integer = 0 To KeyFields.Length - DiscountKeyFiels - 1
            GeneralString &= " and " & KeyFields(j) & "='"
            For i As Integer = 0 To Fields.Length - 1
                If Fields(i) = KeyFields(j) Then
                    GeneralString &= Values(i)
                End If
            Next
            GeneralString &= "' "
        Next
        Return GeneralString
    End Function


    Private Function WhereLastKeyFields() As String

        GeneralString = ""

        GeneralString &= KeyFields(KeyFields.Length - 1) & "='"
        For i As Integer = 0 To Fields.Length - 1
            If Fields(i) = KeyFields(KeyFields.Length - 1) Then
                GeneralString &= Values(i)
            End If
        Next
        GeneralString &= "'"

        Return GeneralString
    End Function


    '----------------------------------------------------------------------------------
    ' still not used
    Private Function ValueFields() As String
        GeneralString = ""
        For k As Integer = 0 To KeyFields.Length - 1
            GeneralString &= Values(k)
            If (k = KeyFields.Length - 2) Then
                GeneralString &= ""
            Else : GeneralString &= "+"
            End If
        Next
        Return GeneralString
    End Function
    '--------------------------Select Items From Table---------------------------------
    Private Function Table_Fields() As String

        GeneralString = ""
        'GeneralString&="Select "
        For i As Integer = 0 To Fields.Length - 1

            GeneralString &= Fields(i)
            If Not i = Fields.Length - 1 Then
                GeneralString &= " , "
            End If
        Next
        Return GeneralString
    End Function

    Public Function RetrieveNameOnly(ByVal SqlStatment As String) As String

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandText = SqlStatment
            Dim ss As String = MyCmd.ExecuteScalar().ToString().Trim
            MyCmd.Connection.Close()
            If ss.Trim() = "" Then

                Return -1
            End If
            Return ss.ToString().Trim

        Catch
            Return -1
        Finally
            c = Nothing
        End Try
    End Function

    Public Function GetMax(ByVal Length As Integer) As String

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            DefineValues()
            Dim kf As Integer = KeyFields.Length

            DiscountKeyFiels = 1
            If (kf > 1) Then
                MyCmd.CommandText = " select max(" & KeyFields(kf - 1) & ") from " & Table_Name & " T " & WhereKeyFields()
            Else : MyCmd.CommandText = " select max(" & KeyFields(kf - 1) & ") from " & Table_Name
            End If

            Dim ss1 As String = MyCmd.ExecuteScalar().ToString().Trim
            MyCmd.Connection.Close()

            Dim ss As Integer = 0

            DiscountKeyFiels = 0
            If (ss1.Trim() = "") Then
                ss = 1
            Else

                ss = Integer.Parse(ss1)
                ss += 1
            End If
            Return ss.ToString().Trim.PadLeft(Length, "0")

        Catch ex As Exception
            Return ex.Message.ToString()
        Finally
            c = Nothing
        End Try
    End Function

    Public Function LoadConnString(ByVal file As String) As String
        Dim ConnectionString As String = ""
        Dim sr As StreamReader = New StreamReader(file)
        sr.ReadLine()
        sr.ReadLine()
        For i As Integer = 0 To 19
            sr.Read()
        Next
        ConnectionString = sr.ReadToEnd()
        sr.Close()
        Return ConnectionString
    End Function

    Public Function IIf(Exp As Boolean, TrueValue As Object, FlaseValue As Object)
        If Exp Then
            Return TrueValue
        Else
            Return FlaseValue
        End If
    End Function
    Public Function FillDate(ByVal dd As DateTime) As String

        Dim dd1 As String = dd.Month.ToString().Trim
        dd1 &= "/" & dd.Day.ToString() & "/" & dd.Year.ToString()
        Return dd1
    End Function

    Public Sub FillCombo(ByVal TableName As String, ByVal cbo As ComboBox, ByVal Condition As String, Optional ByVal c0 As String = "-", Optional OrderById As Boolean = False)

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            cbo.IsTextSearchEnabled = True
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "select 0 Id, '" & c0 & "' Name union Select Id,Name From " & TableName & "  " & Condition & " order by " & IIf(OrderById, "Id", "Name")
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet
            da.SelectCommand = MyCmd
            da.Fill(ds, TableName)
            MyCmd.Connection.Close()

            Dim d As Integer = ds.Tables(0).Rows.Count
            Dim dv As New DataView
            dv.Table = ds.Tables(0)
            Try
                cbo.ItemsSource = Nothing
            Catch ex As Exception
            End Try
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub


    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As ComboBox)

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            cbo.IsTextSearchEnabled = True
            Try
                cbo.SelectedIndex = -1
            Catch
            End Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            MyCmd.Connection.Close()
            Dim d As Integer = ds.Tables(0).Rows.Count
            Dim dv As New DataView
            dv.Table = ds.Tables(0)
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillCombo(ByVal ddtt As DataTable, ByVal cbo As ComboBox)
        Try
            cbo.IsTextSearchEnabled = True
            Try
                cbo.SelectedIndex = -1
            Catch
            End Try
            Dim dv As New DataView
            dv.Table = ddtt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
        End Try
    End Sub

    Public Sub FillListBox(ByVal SqlStatment As String, ByVal Lst As ListBox)

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            MyCmd.Connection.Close()
            Dim d As Integer = ds.Tables(0).Rows.Count
            Lst.ItemsSource = ds.Tables("Table1")
            Lst.DisplayMemberPath = "NAME"
            'Lst.ValueMember = "Id"
            Lst.SelectedIndex = 0
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillListBox(ByVal DT As DataTable, ByVal Lst As ListBox)

        Try
            'Lst.DataSource = DT
            'Lst.DisplayMember = "NAME"
            'Lst.ValueMember = "Id"
            Lst.SelectedIndex = 0
        Catch ex As Exception
            Dim s As String = ex.Message
        End Try
    End Sub

    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As Forms.DataGridViewComboBoxColumn)

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            da.SelectCommand = MyCmd
            Dim dt As New DataTable
            dt.Columns.Add("Id")
            dt.Columns.Add("Name")
            da.Fill(dt)
            MyCmd.Connection.Close()

            cbo.DataSource = dt
            cbo.ValueMember = "Id"
            cbo.DisplayMember = "NAME"
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Sub FillCombo(ByVal SqlStatment As String, ByVal cbo As Forms.DataGridViewComboBoxCell)

        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = SqlStatment
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim ds As New DataSet()
            da.SelectCommand = MyCmd
            da.Fill(ds, "Table1")
            MyCmd.Connection.Close()

            Dim d As Integer = ds.Tables(0).Rows.Count
            cbo.DataSource = ds.Tables("Table1")
            cbo.DisplayMember = "NAME"
            cbo.ValueMember = "Id"
        Catch ex As Exception
            Dim s As String = ex.Message
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function AddItemToTable(ByVal tbl As String, ByVal str As String, Optional ByVal p() As String = Nothing, Optional ByVal v() As String = Nothing) As Boolean
        Dim str1 As String = ""
        Dim str2 As String = ""
        Dim str3 As String = ""
        If Not IsNothing(p) Then
            For i As Integer = 0 To p.Length - 1
                str1 &= " and " & p(i) & "='" & v(i) & "'"
                str2 &= "," & p(i)
                str3 &= ",'" & v(i) & "'"
            Next
        End If
        Return ExcuteNonQuery("if not exists(select * from " & tbl & " where Name='" & str.Trim & "') insert " & tbl & "(Id,Name,UserName,MyGetDate" & str2 & ") values(isnull((select MAX(Id)+1 from " & tbl & " where 1=1 " & str1 & "),1),'" & str.Trim & "'," & Md.UserName & ",GETDATE()" & str3 & ")")
    End Function

    Function TestNames(ArName As TextBox, EnName As TextBox, Optional Fource As Boolean = True) As Boolean

        ArName.Text = ArName.Text.Trim
        EnName.Text = EnName.Text.Trim
        While ArName.Text.Contains("  ")
            ArName.Text = ArName.Text.Replace("  ", " ")
        End While
        While EnName.Text.Contains("  ")
            EnName.Text = EnName.Text.Replace("  ", " ")
        End While

        Dim Ar() As String
        Ar = ArName.Text.Split(" ")
        Dim En() As String
        En = EnName.Text.Split(" ")
        If Fource AndAlso Ar.Length <> En.Length Then
            ShowMSG("Arabic Name Length must be EQUALE English Name Length")
            ArName.Focus()
            Return False
        End If

        Dim x As Integer = 0
        For i As Integer = 0 To Ar.Length - 1
            If Fource AndAlso Ar(i) = En(i) And Not IsNumeric(Ar(i)) Then
                ShowMSG("Arabic Name could not be EQUALE English Name")
                EnName.Select(x, En(i).Length)
                EnName.Focus()
                Return False
            End If
            x += En(i).Length + 1
        Next


        For i As Integer = 0 To Ar.Length - 1
            Dim a As String = ExecuteScalar("delete from Names  where Arabic_Name='" & Ar(i) & "' insert into Names (Arabic_Name,English_Name) values ('" & Ar(i) & "','" & En(i) & "')")
        Next

        Return True
    End Function


    Public Function GetEnName(ArName As String) As String
        ArName = ArName.Trim
        While ArName.Contains("  ")
            ArName = ArName.Replace("  ", " ")
        End While
        Dim s() As String
        s = ArName.Split(" ")
        Dim EnName As String = ""
        For i As Integer = 0 To s.Length - 1
            Dim a As String = ExecuteScalar("select top 1 English_Name from Names where Arabic_Name='" & s(i) & "'")
            If a = "" Then
                EnName &= s(i)
            Else
                EnName &= a
            End If
            EnName &= " "
        Next
        Return EnName.Trim
    End Function


    Public Function AddItemToTable(ByVal tbl As String, ByVal Fld As String(), ByVal str As String(), Optional ByVal p() As String = Nothing, Optional ByVal v() As String = Nothing) As Boolean
        Dim str1 As String = ""
        Dim str2 As String = ""
        Dim str3 As String = ""
        Dim str4 As String = ""
        Dim str5 As String = ""
        If Not IsNothing(p) Then
            For i As Integer = 0 To p.Length - 1
                str1 &= " and " & p(i) & "='" & v(i) & "'"
                str2 &= "," & p(i)
                str3 &= ",'" & v(i) & "'"
            Next
        End If
        For i As Integer = 0 To Fld.Length - 1
            str4 &= "," & Fld(i)
            str5 &= ",'" & str(i) & "'"
        Next
        Return ExcuteNonQuery("insert " & tbl & "(Id,UserName,MyGetDate" & str2 & str4 & ") values(isnull((select MAX(Id)+1 from " & tbl & " where 1=1 " & str1 & "),1)," & Md.UserName & ",GETDATE()" & str3 & str5 & ")")
    End Function

    Public Function ExcuteNonQuery(ByVal sqlstatment As String) As Boolean

        If Not StopPro() Then Return False
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "SET XACT_ABORT ON BEGIN TRAN " & vbCrLf & sqlstatment & vbCrLf & " COMMIT"

            MyCmd.ExecuteNonQuery()
            MyCmd.Connection.Close()
        Catch ex As Exception
            Dim s As String = ex.Message.ToString()
            c = Nothing
            Return False
        Finally
            c = Nothing
        End Try
        Return True

    End Function
    Public Function StopPro() As Boolean
        Return True
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = "StopPro"
            MyCmd.ExecuteNonQuery()
            MyCmd.Connection.Close()
        Catch ex As Exception
            Dim s As String = ex.Message.ToString()
            c = Nothing
            Return False
        Finally
            c = Nothing
        End Try
        Return True

    End Function

    Public Function ExcuteAdapter(ByVal sqlstatment As String) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            dt = New DataTable
            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = sqlstatment
            da.SelectCommand = MyCmd

            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            Dim x As Integer = dt.Rows.Count
            Return dt

        Catch ex As Exception
            Dim SS As String = ex.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function

    Public Function ExcuteAdapter(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            MyCmd.Parameters.Clear()
            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            Return dt

        Catch ex As Exception
            Dim ss As String = ex.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function





    Public Sub SendEMail(ByVal Bath As String)
        Dim demoThread As New System.ComponentModel.BackgroundWorker
        AddHandler demoThread.DoWork, AddressOf MyDoWrok
        ' AddHandler demoThread.RunWorkerCompleted, AddressOf MyWorkerCompleted
        MyBath = Bath
        demoThread.RunWorkerAsync()
    End Sub
    Dim MyBath As String = ""
    Sub MyDoWrok(ByVal sender As Object, ByVal ev As EventArgs)
        SendEMail2(MyBath)
    End Sub

    Public Sub SendEMail2(ByVal Bath As String)
        Dim mailMsg As New MailMessage()
        mailMsg.From = New MailAddress("PhysicsAdvansed@gmail.com")
        mailMsg.To.Add("Mr_Aymans@Yahoo.com")
        mailMsg.To.Add("AsebBackups@yahoo.com")
        mailMsg.To.Add("PhysicsAdvansed@Yahoo.com")
        mailMsg.To.Add("Mr_Aymans@Hotmail.com")
        Dim MyNow As DateTime = ExecuteScalar("Select dbo.MyGetDate()")
        mailMsg.Subject = Bath.Replace(Application.Current.StartupUri.ToString & "\", "") & " " & MyNow.ToShortDateString & " " & MyNow.ToShortTimeString
        mailMsg.IsBodyHtml = True
        mailMsg.BodyEncoding = Encoding.UTF8
        mailMsg.Attachments.Add(New System.Net.Mail.Attachment(Bath))
        mailMsg.Body = "Thanks, Mr. Physics"
        mailMsg.Priority = MailPriority.High
        ' Smtp configuration
        Dim client As New SmtpClient()
        client.Credentials = New NetworkCredential("PhysicsAdvansed@gmail.com", "0000000000")
        client.Port = 587 'or use 465
        client.Host = "smtp.gmail.com"
        client.EnableSsl = True
        client.Timeout = 72000000
        Dim userState = mailMsg
        Try
            'you can also call client.Send(msg)
            client.SendAsync(mailMsg, userState)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ExcuteNonQuery(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String)

        If Not StopPro() Then Return
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            MyCmd.ExecuteNonQuery()
            MyCmd.Connection.Close()
        Catch ee As Exception
            Dim ss As String = ee.Message.ToString()
            ShowMSG(ee.Message.ToString())
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function ExcuteAdapter(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String, ByVal Type() As SqlDbType) As DataTable

        Dim dt As New DataTable()
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), Type(i))
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            Return dt
        Catch ee As Exception
            Dim ss As String = ee.Message.ToString()
            Return dt
        Finally
            c = Nothing
        End Try
    End Function

    Public Function ExcuteNonQuery(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As Object, ByVal Type() As SqlDbType) As Boolean

        If Not StopPro() Then Return False
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName

            For i As Integer = 0 To ParaName.Length - 1
                MyCmd.Parameters.Add("@" & ParaName(i), Type(i))
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
                MyCmd.Parameters("@" & ParaName(i)).DbType = DbType.Object  'Type(i)
            Next
            MyCmd.ExecuteNonQuery()
            MyCmd.Connection.Close()
            Return True
        Catch ee As Exception
            Dim ss As String = ee.Message.ToString()
            Return False
        Finally
            c = Nothing
        End Try
    End Function

    Public Function ean13(chaine As String)
        chaine = left(chaine, 12)
        While len(chaine) < 12
            chaine = IIf(Len(chaine) = 11, "1", "0") & chaine
        End While
        Dim i As Integer
        Dim checksum As Integer
        ean13 = ""
        If Len(chaine) = 12 Then
            For i = 1 To 12
                If Asc(Mid(chaine, i, 1)) < 48 Or Asc(Mid(chaine, i, 1)) > 57 Then
                    i = 0
                    Exit For
                End If
            Next
            If i = 13 Then
                For i = 2 To 12 Step 2
                    checksum = checksum + Val(Mid(chaine, i, 1))
                Next
                checksum = checksum * 3
                For i = 1 To 11 Step 2
                    checksum = checksum + Val(Mid(chaine, i, 1))
                Next
                chaine = chaine & (10 - checksum Mod 10) Mod 10
                ean13 = chaine
            End If
        End If
    End Function

    Public Function ean13Code(chaine As String)
        chaine = Left(chaine, 12)
        While Len(chaine) < 12
            chaine = "0" & chaine
        End While
        Dim i As Integer
        Dim checksum As Integer
        Dim first As Integer
        Dim CodeBarre As String
        Dim tableA As Boolean
        ean13Code = ""
        If Len(chaine) = 12 Then
            For i = 1 To 12
                If Asc(Mid(chaine, i, 1)) < 48 Or Asc(Mid(chaine, i, 1)) > 57 Then
                    i = 0
                    Exit For
                End If
            Next
            If i = 13 Then
                For i = 2 To 12 Step 2
                    checksum = checksum + Val(Mid(chaine, i, 1))
                Next
                checksum = checksum * 3
                For i = 1 To 11 Step 2
                    checksum = checksum + Val(Mid(chaine, i, 1))
                Next
                chaine = chaine & (10 - checksum Mod 10) Mod 10
                CodeBarre = Left(chaine, 1) & Chr(65 + Val(Mid(chaine, 2, 1)))
                first = Val(Left(chaine, 1))
                For i = 3 To 7
                    tableA = False
                    Select Case i
                        Case 3
                            Select Case first
                                Case 0 To 3
                                    tableA = True
                            End Select
                        Case 4
                            Select Case first
                                Case 0, 4, 7, 8
                                    tableA = True
                            End Select
                        Case 5
                            Select Case first
                                Case 0, 1, 4, 5, 9
                                    tableA = True
                            End Select
                        Case 6
                            Select Case first
                                Case 0, 2, 5, 6, 7
                                    tableA = True
                            End Select
                        Case 7
                            Select Case first
                                Case 0, 3, 6, 8, 9
                                    tableA = True
                            End Select
                    End Select
                    If tableA Then
                        CodeBarre = CodeBarre & Chr(65 + Val(Mid(chaine, i, 1)))
                    Else
                        CodeBarre = CodeBarre & Chr(75 + Val(Mid(chaine, i, 1)))
                    End If
                Next
                CodeBarre = CodeBarre & "*"
                For i = 8 To 13
                    CodeBarre = CodeBarre & Chr(97 + Val(Mid(chaine, i, 1)))
                Next
                CodeBarre = CodeBarre & "+"
                ean13Code = CodeBarre
            End If
        End If
    End Function


    Public Function ExecuteScalar(ByVal sqlstatment As String) As String

        If Not StopPro() Then Return ""
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.Text
            MyCmd.CommandText = sqlstatment

            Dim s As String = MyCmd.ExecuteScalar().ToString().Trim
            MyCmd.Connection.Close()
            Return s
        Catch ex As Exception
            Dim s As String = ex.Message.ToString()
            Return ""
        Finally
            c = Nothing
        End Try
    End Function


    Public Function ExecuteScalar(ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String) As String

        If Not StopPro() Then Return ""
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            Dim s As String = MyCmd.ExecuteScalar().ToString().Trim
            MyCmd.Connection.Close()
            Return s
        Catch ee As Exception
            Dim ss As String = ee.Message.ToString()
            Return ""
        Finally
            c = Nothing
        End Try
    End Function


    Public Function ToStrDate(ByVal dd As DateTime) As String
        Return dd.Year.ToString() & "/" & dd.Month.ToString().PadLeft(2, "0") & "/" & dd.Day.ToString().PadLeft(2, "0")
    End Function


    Public Function ToStrDateTime(ByVal dd As DateTime) As String
        Return (dd.Month.ToString().PadLeft(2, "0") & "/" & dd.Day.ToString().PadLeft(2, "0") & "/" & dd.ToString.Substring(6)).Replace("ص", "am").Replace("م", "pm")
    End Function

    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal tbl As String, ByVal SubId As String, ByVal SubName As String)
        Try
            cbo.IsTextSearchEnabled = True
            Dim dt As DataTable = ExcuteAdapter("select " & SubId & " 'Id'," & SubName & " 'Name From " & tbl & " union select 0 Id,'-' Name")

            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub

    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal tbl As String, ByVal SubId As String, ByVal SubName As String, ByVal where As String)
        Try
            cbo.IsTextSearchEnabled = True
            Dim dt As DataTable = ExcuteAdapter("select " & SubId & " 'Id'," & SubName & " 'Name' From " & tbl & " " & where & " union select 0 Id,'-' Name")
            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    Public Sub FillCombo(ByVal cbo As ComboBox, ByVal StoredName As String, ByVal ParaName() As String, ByVal ParaValue() As String)

        Dim dt As New DataTable()
        Dim c As New SqlConnection(cmd.Connection.ConnectionString)
        Try
            cbo.IsTextSearchEnabled = True
            Dim MyCmd As SqlCommand = c.CreateCommand()
            If MyCmd.Connection.State = ConnectionState.Closed Then MyCmd.Connection.Open()

            MyCmd.CommandTimeout = 72000000
            MyCmd.Parameters.Clear()
            MyCmd.CommandType = CommandType.StoredProcedure
            MyCmd.CommandText = StoredName
            da.SelectCommand = MyCmd

            MyCmd.Parameters.Clear()
            For i As Integer = 0 To ParaName.Length - 1

                MyCmd.Parameters.Add("@" & ParaName(i), SqlDbType.VarChar)
                MyCmd.Parameters("@" & ParaName(i)).Value = ParaValue(i)
            Next
            dt = New DataTable
            da.Fill(dt)
            MyCmd.Connection.Close()

            Dim dv As New DataView
            dv.Table = dt
            cbo.ItemsSource = dv
            cbo.SelectedValuePath = "Id"
            cbo.DisplayMemberPath = "Name"
            cbo.SelectedIndex = 0
        Catch ex As Exception
        Finally
            c = Nothing
        End Try
    End Sub

    Public Function GetDelayPeriod() As Integer
        Dim s As String = ExecuteScalar("select dbo.GetDelayPeriod()")
        If s = "" Then s = "0"
        Return Val(s)
    End Function

    Public Sub SaveImage(ByVal tbl As String, ByVal ID() As String, ByVal IDValue() As String, ByVal Field As String, ByVal PicPox As Controls.Image)
        Try

            Dim stream As New MemoryStream()
            Dim encoder As New BmpBitmapEncoder()
            Dim myTextBlock As New TextBlock()
            myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString()
            encoder.Frames.Add(BitmapFrame.Create(PicPox.Source))
            encoder.Save(stream)
            Dim ImageBytes As Byte()
            ImageBytes = stream.ToArray

            Dim statement As String = "update " & tbl & " set " & Field & "=@MyImage Where " & ID(0) & "='" & IDValue(0) & "'"
            For i As Integer = 1 To ID.Length - 1
                statement &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next

            Dim com As New SqlClient.SqlCommand(statement, con)
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes
            com.ExecuteNonQuery()

        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetImage(ByVal tbl As String, ByVal ID() As String, ByVal IDValue() As String, ByVal Field As String, ByVal PicPox As Controls.Image)
        Try
            Dim myCommand As SqlClient.SqlCommand
            Dim statement As String = "select " & Field & " from " & tbl & " Where " & ID(0) & "='" & IDValue(0) & "'"
            For i As Integer = 1 To ID.Length - 1
                statement &= " and " & ID(i) & "='" & IDValue(i) & "'"
            Next

            myCommand = New SqlClient.SqlCommand(statement, con)

            Dim imagedata() As Byte = CType(myCommand.ExecuteScalar(), Byte())
            Dim stmBLOBData As IO.MemoryStream = New IO.MemoryStream(imagedata)

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.StreamSource = stmBLOBData
            'bi3.DecodePixelWidth = 100
            'bi3.DecodePixelHeight = 100
            bi3.EndInit()
            PicPox.Source = bi3
        Catch ex As Exception
            SetNoImage(PicPox)
        End Try

    End Sub


    Public Sub SaveFile(ByVal tbl As String, ByVal KeyId As String, ByVal KeyValue As String, ByVal KeyId2 As String, ByVal KeyValue2 As String, ByVal Field As String, ByVal Path As String)
        Try
            Dim ImageBytes As Byte() = File.ReadAllBytes(Path)
            Dim com As New SqlClient.SqlCommand("if exists (select * from " & tbl & " where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "') update " & tbl & " set " & Field & "=@MyImage,LastUpdate=getdate() where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' else insert into " & tbl & "( " & KeyId & "," & KeyId2 & "," & Field & ",LastUpdate) select '" & KeyValue & "','" & KeyValue2 & "',@MyImage,getdate()", con)
            cmd.CommandTimeout = 720000
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes
            If con.State = ConnectionState.Closed Then con.Open()
            com.ExecuteNonQuery()

        Catch ex As Exception
            ShowMSG(ex.Message)
        End Try
    End Sub


    Public Sub SaveText(ByVal tbl As String, ByVal KeyId As String, ByVal KeyValue As String, ByVal KeyId2 As String, ByVal KeyValue2 As String, ByVal KeyId3 As String, ByVal KeyValue3 As String, ByVal Field As String, ByVal Str As String)
        Try
            Dim com As New SqlClient.SqlCommand("if not exists (select * from " & tbl & " where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' and " & KeyId3 & "='" & KeyValue3 & "' and " & Field & "='" & Str & "')  insert into " & tbl & "( " & KeyId & "," & KeyId2 & "," & KeyId3 & "," & Field & ",MyGetDate) select '" & KeyValue & "','" & KeyValue2 & "','" & KeyValue3 & "',@MyImage,getdate()", con)
            cmd.CommandTimeout = 720000
            com.Parameters.Add("@MyImage", SqlDbType.VarChar)
            com.Parameters("@MyImage").Value = Str
            If con.State = ConnectionState.Closed Then con.Open()
            com.ExecuteNonQuery()

        Catch ex As Exception
            ShowMSG(ex.Message)
        End Try
    End Sub


    Public Sub SaveFile(ByVal tbl As String, ByVal KeyId As String, ByVal KeyValue As String, ByVal KeyId2 As String, ByVal KeyValue2 As String, ByVal KeyId3 As String, ByVal KeyValue3 As String, ByVal Field As String, ByVal Path As String)
        Try
            Dim ImageBytes As Byte() = File.ReadAllBytes(Path)
            Dim com As New SqlClient.SqlCommand("if exists (select * from " & tbl & " where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' and " & KeyId3 & "='" & KeyValue3 & "') update " & tbl & " set " & Field & "=@MyImage,MyGetDate=getdate() where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' and " & KeyId3 & "='" & KeyValue3 & "' else insert into " & tbl & "( " & KeyId & "," & KeyId2 & "," & KeyId3 & "," & Field & ",MyGetDate) select '" & KeyValue & "','" & KeyValue2 & "','" & KeyValue3 & "',@MyImage,getdate()", con)
            cmd.CommandTimeout = 720000
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes
            If con.State = ConnectionState.Closed Then con.Open()
            com.ExecuteNonQuery()

        Catch ex As Exception
            ShowMSG(ex.Message)
        End Try
    End Sub


    Public Sub SaveFile(ByVal tbl As String, ByVal KeyId As String, ByVal KeyValue As String, ByVal KeyId2 As String, ByVal KeyValue2 As String, ByVal KeyId3 As String, ByVal KeyValue3 As String, ByVal KeyId4 As String, ByVal KeyValue4 As String, ByVal Field As String, ByVal Path As String)
        Try
            Dim ImageBytes As Byte() = File.ReadAllBytes(Path)
            Dim com As New SqlClient.SqlCommand("if exists (select * from " & tbl & " where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' and " & KeyId3 & "='" & KeyValue3 & "' and " & KeyId4 & "='" & KeyValue4 & "') update " & tbl & " set " & Field & "=@MyImage,MyGetDate=getdate() where " & KeyId & "='" & KeyValue & "' and " & KeyId2 & "='" & KeyValue2 & "' and " & KeyId3 & "='" & KeyValue3 & "' and " & KeyId4 & "='" & KeyValue4 & "' else insert into " & tbl & "( " & KeyId & "," & KeyId2 & "," & KeyId3 & "," & KeyId4 & "," & Field & ",MyGetDate) select '" & KeyValue & "','" & KeyValue2 & "','" & KeyValue3 & "','" & KeyValue4 & "',@MyImage,getdate()", con)
            cmd.CommandTimeout = 720000
            com.Parameters.Add("@MyImage", SqlDbType.Image)
            com.Parameters("@MyImage").Value = ImageBytes
            If con.State = ConnectionState.Closed Then con.Open()
            com.ExecuteNonQuery()

        Catch ex As Exception
            ShowMSG(ex.Message)
        End Try
    End Sub



    Public Sub AllowDorp(ByVal picBox As Controls.Image)
        picBox.AllowDrop = True
        AddHandler picBox.DragLeave, AddressOf pictureBox_DragDrop
        AddHandler picBox.DragEnter, AddressOf pictureBox_DragEnter
    End Sub
    Private Sub pictureBox_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effects = e.AllowedEffects
    End Sub

    Private Sub pictureBox_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
        Dim ss() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim a As Windows.Media.ImageSource
        Dim dp As DependencyProperty
        a.SetCurrentValue(dp, ss(0))
        CType(sender, Controls.Image).Source = a
    End Sub
    Public Sub ShowMSG(ByVal MSG As String)
        Dim mm As New MSG
        mm.MSG = MSG
        If mm.MSG = "" Then mm.MSG = MSG
        mm.DelMsg = False
        mm.ShowDialog()
    End Sub
    Public Function ShowDeleteMSG(Optional MSG As String = "هل أنت متأكد من المسح؟") As Boolean
        Dim mm As New MSG
        mm.MSG = MSG
        If mm.MSG = "" Then mm.MSG = MSG
        mm.DelMsg = True
        mm.ShowDialog()
        Return mm.Ok
    End Function

    Public Function Encrypt(ByVal plainText As String) As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer
        passPhrase = "Ph3322@##"        ' can be any string
        saltValue = "looP$$s##"        ' can be any string
        hashAlgorithm = "MD5"             ' can be "MD5"
        passwordIterations = 13                  ' can be any number
        initVector = "%r$t#k*h&g$d#d@s" ' must be 16 bytes
        keySize = 128                ' can be 192 or 256

        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)
        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)
        Dim password As PasswordDeriveBytes
        password = New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)

        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim encryptor As ICryptoTransform
        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
        Dim memoryStream As MemoryStream
        memoryStream = New MemoryStream()
        Dim cryptoStream As CryptoStream
        cryptoStream = New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)

        cryptoStream.FlushFinalBlock()

        Dim cipherTextBytes As Byte()
        cipherTextBytes = memoryStream.ToArray()

        memoryStream.Close()
        cryptoStream.Close()
        Return Convert.ToBase64String(cipherTextBytes)

    End Function
    Public Function Decrypt(ByVal cipherText As String) As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer

        passPhrase = "Ph3322@##"        ' can be any string
        saltValue = "looP$$s##"        ' can be any string
        hashAlgorithm = "MD5"             ' can be "MD5"
        passwordIterations = 13                  ' can be any number
        initVector = "%r$t#k*h&g$d#d@s" ' must be 16 bytes
        keySize = 128                ' can be 192 or 256



        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

        Dim cipherTextBytes As Byte()
        cipherTextBytes = Convert.FromBase64String(cipherText)
        Dim password As PasswordDeriveBytes
        password = New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)
        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim decryptor As ICryptoTransform
        decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
        Dim memoryStream As MemoryStream
        memoryStream = New MemoryStream(cipherTextBytes)
        Dim cryptoStream As CryptoStream
        cryptoStream = New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

        Dim plainTextBytes As Byte()
        ReDim plainTextBytes(cipherTextBytes.Length)
        Dim decryptedByteCount As Integer
        decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)

        memoryStream.Close()
        cryptoStream.Close()
        Return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)

    End Function

    Sub SetNoImage(ByVal image As Controls.Image, Optional ByVal IsPerson As Boolean = False, Optional ByVal Ask As Boolean = False)
        If Not Ask OrElse ShowDeleteMSG("Are you sure you want to cancel this photo?") Then
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri(IIf(IsPerson, "Images\no.photo.gif", "Images\cancel.png"), UriKind.RelativeOrAbsolute)
            bi3.EndInit()
            image.Source = bi3
        End If
    End Sub

    Sub SetNoImage(ByVal image As DNBSoft.WPF.RibbonControl.RibbonPreviewBox)
        Dim bi3 As New BitmapImage
        bi3.BeginInit()
        bi3.UriSource = New Uri("Images\cancel.png", UriKind.RelativeOrAbsolute)
        bi3.EndInit()
        image.Image = bi3
    End Sub

    Sub SetImage(Img As ImageBrush, MyUri As String)
        Try
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("pack://application:,,,/Traffic;component/Images/" & MyUri, UriKind.RelativeOrAbsolute)
            bi3.EndInit()
            Img.ImageSource = bi3
        Catch ex As Exception
            ShowMSG(ex.Message)
            Application.Current.Shutdown()
        End Try
    End Sub

    Sub SetImage(Img As Controls.Image, MyUri As String)
        Try
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("pack://application:,,,/Traffic;component/Images/" & MyUri, UriKind.RelativeOrAbsolute)
            bi3.EndInit()
            Img.Source = bi3
        Catch ex As Exception
            ShowMSG(ex.Message)
            Application.Current.Shutdown()
        End Try
    End Sub

    Sub SetImage(ByVal image As Controls.Image, Optional Shrink As Boolean = True)
        Try
            Dim OFD As New System.Windows.Forms.OpenFileDialog
            OFD.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"
            If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim bi3 As New BitmapImage
                bi3.BeginInit()
                bi3.UriSource = New Uri(OFD.FileName, UriKind.RelativeOrAbsolute)
                If Shrink Then
                    bi3.DecodePixelWidth = 100
                    bi3.DecodePixelHeight = 100
                End If
                bi3.EndInit()
                image.Source = bi3
            End If
        Catch
        End Try
    End Sub

    Sub SetImageFromScanner(ByVal image As Controls.Image, Optional Shrink As Boolean = True)
        Try
            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            Dim x As String = GetNewTempName("Scr")
            Dim WIACommonDialog As WIA.CommonDialog = New WIA.CommonDialog
            WIACommonDialog.ShowAcquireImage(WIA.WiaDeviceType.UnspecifiedDeviceType, WIA.WiaImageIntent.GrayscaleIntent, WIA.WiaImageBias.MinimizeSize, "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", False, False, False).FileData.ImageFile.SaveFile(x)
            bi3.UriSource = New Uri(x, UriKind.RelativeOrAbsolute)
            If Shrink Then
                bi3.DecodePixelWidth = 100
                bi3.DecodePixelHeight = 100
            End If
            bi3.EndInit()
            image.Source = bi3
        Catch ex As Exception
            ShowMSG(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Returns FilePath
    ''' </summary>
    Function SaveImageFromScannerToFile(Optional FileName As String = "") As String
        Try
            If Not IO.Directory.Exists(Path) Then IO.Directory.CreateDirectory(Path)

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            Dim x As String = Path & FileName
            If FileName = "" Then x = GetNewTempName("jpg")
            If IO.File.Exists(x) Then IO.File.Delete(x)
            Dim WIACommonDialog As WIA.CommonDialog = New WIA.CommonDialog
            WIACommonDialog.ShowAcquireImage(WIA.WiaDeviceType.UnspecifiedDeviceType, WIA.WiaImageIntent.GrayscaleIntent, WIA.WiaImageBias.MaximizeQuality, "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", False, False, False).FileData.ImageFile.SaveFile(x)
            Return x
        Catch ex As Exception
            ShowMSG(ex.Message)
            Return ""
        End Try
    End Function

    Function GetNewTempName(ByVal FileName As String) As String
        If Not IO.Directory.Exists(Path) Then IO.Directory.CreateDirectory(Path)
        Dim i As Integer = 0, s As String = ""
        While True
            i += 1
            's = Path & i & "." & FileName.Split(".").Last
            s = Path & FileName.Replace(FileName.Split(".").Last, "") & i & "." & FileName.Split(".").Last
            If Not IO.File.Exists(s) Then
                Exit While
            End If
        End While
        Return s
    End Function

    Sub ClearTemp()
        Try
            'Dim rpt As New ReportViewer
            'rpt.Rpt = "Blank.rpt"
            'rpt.ReportViewer_Load(Nothing, Nothing)
            'rpt.ReportViewer_FormClosing(Nothing, Nothing)
        Catch ex As Exception
        End Try
        Try
            Try
                IO.Directory.Delete(Path, True)
            Catch ex As Exception
            End Try
            If Not IO.Directory.Exists(Path) Then Return
            For Each f As String In Directory.GetFiles(Path)
                Try
                    File.Delete(f)
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MyOpenCombobox(ByVal sender As Object, ByVal e As KeyEventArgs)
        Try
            If Not e.Key = System.Windows.Input.Key.Enter Then
                CType(sender, ComboBox).IsDropDownOpen = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopFilter As Boolean = False
    Private Sub MyFilterCombobox(ByVal sender As Object, ByVal e As KeyEventArgs)
        Try
            Return
            If LopFilter OrElse e.Key = Key.Enter OrElse e.Key = Key.Left OrElse e.Key = Key.Right Then Return

            If CType(sender, ComboBox).SelectedIndex = -1 Then
                Dim ss As String = sender.Text
                Try : CType(sender, ComboBox).SelectedIndex = 0 : Catch : End Try
                sender.Text = ss
            End If
            If e.Key = Key.Up OrElse e.Key = Key.Down Then Return
            LopFilter = True
            GoTo A
            If e.Key = Key.Down Then
                Try
                    If CType(sender, ComboBox).SelectedIndex = 0 Then
                        Dim ss As String = sender.Text
                        CType(sender, ComboBox).SelectedIndex += 1
                        CType(sender, ComboBox).SelectedIndex -= 1
                        sender.Text = ss
                    End If
                Catch ex As Exception
                End Try
            ElseIf e.Key = Key.Up Then
                Try
                    If CType(sender, ComboBox).SelectedIndex = 0 Then
                        Dim ss As String = sender.Text
                        CType(sender, ComboBox).SelectedIndex -= 1
                        CType(sender, ComboBox).SelectedIndex += 1
                        sender.Text = ss
                    End If
                Catch ex As Exception
                End Try
            End If
A:
            Dim s As String = sender.Text
            CType(sender.ItemsSource, DataView).RowFilter = "Name like '" & sender.Text & "%'"
            If Not CType(sender, ComboBox).IsDropDownOpen Then CType(sender, ComboBox).IsDropDownOpen = True

            sender.Text = CType(CType(sender, ComboBox).SelectedItem, DataRowView)("Name")
        Catch ex As Exception
        End Try
        LopFilter = False
    End Sub

    Private Sub MyGotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
        CType(sender, ComboBox).IsDropDownOpen = True
    End Sub


    Public Sub SetPickerDate(DP As DatePicker, V As Object)
        If V.ToString = "01/01/1900 12:00:00 AM" Then
            DP.SelectedDate = Nothing
        Else
            Try
                DP.SelectedDate = V.ToString
            Catch ex As Exception
                DP.SelectedDate = Nothing
            End Try
        End If
    End Sub

    Sub ResetComboboxContent(cbo As ComboBox)
        Dim b As Boolean = True
        For i As Integer = 0 To cbo.Items.Count - 1
            Try
                CType(cbo.Items(i), ComboBoxItem).SetResourceReference(ComboBoxItem.BackgroundProperty, "BgDelete")
            Catch ex As Exception
            End Try
        Next

    End Sub

    Sub SetColor(C As Object)
        Try
            C.Background = System.Windows.Media.Brushes.Red
            If TypeOf (C) Is Grid Then
                For Each CC In CType(C, Grid).Children
                    If TypeOf (CC) Is Border OrElse TypeOf (CC) Is Controls.Image OrElse TypeOf (CC) Is System.Windows.Forms.Integration.WindowsFormsHost Then
                        Continue For
                    ElseIf TypeOf (CC) Is Label OrElse TypeOf (CC) Is TextBox OrElse TypeOf (CC) Is DatePicker OrElse TypeOf (CC) Is ComboBox OrElse TypeOf (CC) Is Button OrElse TypeOf (CC) Is PasswordBox Then
                        CType(CC, Control).Background = System.Windows.Media.Brushes.White
                        CType(CC, Control).Foreground = System.Windows.Media.Brushes.Red

                    ElseIf TypeOf (CC) Is RadioButton OrElse TypeOf (CC) Is CheckBox Then
                        CType(CC, Control).Background = System.Windows.Media.Brushes.Red
                        CType(CC, Control).Foreground = System.Windows.Media.Brushes.White

                    ElseIf TypeOf (CC) Is Grid OrElse TypeOf (CC) Is GroupBox OrElse TypeOf (CC) Is TabControl OrElse TypeOf (CC) Is TabItem OrElse TypeOf (CC) Is WrapPanel OrElse TypeOf (CC) Is Border OrElse TypeOf (CC) Is DockPanel Then
                        SetColor(CC)
                    Else
                        CType(CC, Control).Background = System.Windows.Media.Brushes.Red
                        CType(CC, Control).Foreground = System.Windows.Media.Brushes.White
                    End If
                Next
            ElseIf TypeOf (C) Is TabControl Then
                For Each CC In CType(C, TabControl).Items
                    SetColor(CC)
                Next
            ElseIf TypeOf (C) Is Border Then
                SetColor(CType(C, Border).Child)
            ElseIf TypeOf (C) Is TabItem Then
                SetColor(CType(C, TabItem).Content)
            ElseIf TypeOf (C) Is GroupBox Then
                SetColor(CType(C, GroupBox).Content)
            ElseIf TypeOf (C) Is WrapPanel Then
                For Each CC In CType(C, WrapPanel).Children
                    SetColor(CC)
                Next
            ElseIf TypeOf (C) Is DockPanel Then
                For Each CC In CType(C, DockPanel).Children
                    SetColor(CC)
                Next
            ElseIf TypeOf (C) Is TabItem Then
                SetColor(CType(C, TabItem).Content)
            ElseIf TypeOf (C) Is GroupBox Then
                SetColor(CType(C, GroupBox).Content)
            Else
                C.Background = System.Windows.Media.Brushes.Red
                C.Foreground = System.Windows.Media.Brushes.White
            End If

            If TypeOf (C) Is UserControl Then
                SetColor(CType(C, UserControl).Content)
            End If


        Catch ex As Exception
        End Try
    End Sub

    Sub OpenWord(txt As TextBox)
        Try
            Dim doc As Word.Document
            Dim wordApp As New Word.Application
            Dim allText As String
            Dim ofd As New Forms.OpenFileDialog With {.Filter = "Word Files(*.Doc;*.Docx)|*.Doc;*.Docx|All files (*.*)|*.*"}
            If Not ofd.ShowDialog = Forms.DialogResult.OK Then Return
            doc = wordApp.Documents.Open(ofd.FileName)
            allText = doc.Range.Text()
            doc.Close()
            txt.Text = allText
        Catch
            'error            
        End Try
    End Sub

 

    Public Sub Addcontrol_MouseDoubleClick(MyControls As Controls.Control())
        If Not EventHasAdded Then
            EventHasAdded = True
            For i As Integer = 0 To MyControls.Length - 1
                AddHandler MyControls(i).MouseDoubleClick, AddressOf control_MouseDoubleClick
            Next
        End If
    End Sub

    Private Sub control_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.F1) With {.RoutedEvent = Keyboard.KeyUpEvent})
    End Sub


End Class
