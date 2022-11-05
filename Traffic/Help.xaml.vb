Imports System.Data

Public Class Help
    Dim bm As New BasicMethods
    Public FirstColumn As String = "المسلسل", SecondColumn As String = "الاسم", Statement As String = "", TableName As String = ""
    Public MyFields() As String = Nothing, MyValues() As String = Nothing

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""

    Public Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        ''LoadResource()
        Banner1.StopTimer = True
        Banner1.Header = Header
        If TableName = "" Then btnSave.Visibility = Visibility.Hidden
        Try
            dt = bm.ExcuteAdapter(Statement)
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = IIf(FirstColumn Is Nothing, FirstColumn, FirstColumn)
            dt.Columns(1).ColumnName = IIf(SecondColumn Is Nothing, SecondColumn, SecondColumn)

            DataGridView1.Foreground = System.Windows.Media.Brushes.Black
            dv.Table = dt
            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 120
            DataGridView1.Columns(1).Width = 300
            If dt.Columns.Count = 3 Then
                DataGridView1.Columns(0).Width = 80
                DataGridView1.Columns(1).Width = 260
                DataGridView1.Columns(2).Width = 80
            End If
            DataGridView1.SelectedIndex = 0
        Catch
        End Try

        'If Md.MyProject = Client.ClothesRed Then
        '    DataGridView1.Foreground = System.Windows.Media.Brushes.Red

        '    Banner1.R.Fill = System.Windows.Media.Brushes.Red
        '    Banner1.lblMain.Foreground = System.Windows.Media.Brushes.White
        '    Banner1.Background = System.Windows.Media.Brushes.White
        '    Banner1.Foreground = System.Windows.Media.Brushes.Red
        'End If
        DataGridView1.IsReadOnly = True
        txtName.Focus()
    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged
        Try
            dv.RowFilter = " [" & dt.Columns(0).ColumnName & "] like '%" & txtID.Text & "%' and [" & dt.Columns(1).ColumnName & "] like '%" & txtName.Text & "%'"
        Catch
        End Try
    End Sub

    Public SelectedId As Integer = 0
    Public SelectedName As String = ""
    Public SelectedRow As System.Data.DataRowView
    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = System.Windows.Input.Key.Enter Then
                SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
                SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
                SelectedRow = DataGridView1.Items(DataGridView1.SelectedIndex)
                Close()
            ElseIf e.Key = Input.Key.Escape Then
                Close()
            ElseIf e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            SelectedRow = DataGridView1.Items(DataGridView1.SelectedIndex)
            Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        Try
            If txtName.Text.Trim = "" Then Return
            txtID.Clear()
            For i As Integer = 0 To dv.Table.Rows.Count - 1
                If txtName.Text.Trim = dv.Table.Rows(i).Item(1).ToString Then Return
            Next
            txtName.Text = txtName.Text.Trim
            If Not bm.AddItemToTable(TableName, txtName.Text.Trim, MyFields, MyValues) Then Return
            Window_Loaded(Nothing, Nothing)
            DataGridView1.SelectedIndex = dv.Table.Rows.Count - 1
            txtId_TextChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub


End Class