Imports System.Data
Imports System.IO

Public Class InvoicesAttachments

    Dim bm As New BasicMethods

    Private Sub Payment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

    End Sub


    Private Sub btnGet_Click(sender As Object, e As RoutedEventArgs) Handles btnGet.Click
        LoadTree()
    End Sub

    Private Sub LabelData_LostFocus(sender As Object, e As RoutedEventArgs) Handles LabelData.LostFocus

    End Sub

    Private Sub btnbtnScanner(sender As Object, e As RoutedEventArgs) Handles btnScanner.Click
        'bm.SetImageFromScanner(Image1)
        Dim x As String = bm.SaveImageFromScannerToFile()
        bm.SaveFile("InvoicesAttachments", "LabelData", LabelData.Text, "CarTypeId", CarTypeId.Text, "FileName", (x.Split("\"))(x.Split("\").Length - 1), "ImageData", x)
        LoadTree()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As RoutedEventArgs) Handles btnBrowse.Click
        'bm.SetImage(Image1)
        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("InvoicesAttachments", "LabelData", LabelData.Text, "CarTypeId", CarTypeId.Text, "FileName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "ImageData", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub

    Private Sub LoadTree()
        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExcuteAdapter("select FileName from InvoicesAttachments where LabelData='" & LabelData.Text & "' and CarTypeId='" & CarTypeId.Text & "' order by FileName")

        If dt.Rows.Count > 0 Then
            LabelData.IsEnabled = False
            CarTypeId.IsEnabled = False
        End If

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn2 As New TreeViewItem
            nn2.FontSize = 16
            nn2.Tag = dt.Rows(i)("FileName")
            nn2.Header = dt.Rows(i)("FileName")
            TreeView1.Items.Add(nn2)
        Next
    End Sub

    Private Sub TreeView1_KeyDown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles TreeView1.KeyDown
        If e.Key = Key.Delete Then
            Try
                If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 16 Then
                    If bm.ShowDeleteMSG("Are you sure you want to Delete this file?") Then
                        bm.ExcuteNonQuery("delete from InvoicesAttachments where LabelData='" & LabelData.Text & "' and CarTypeId='" & CarTypeId.Text & "' and FileName='" & CType(TreeView1.SelectedItem, TreeViewItem).Header & "'")
                        LoadTree()
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub TreeView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TreeView1.MouseDoubleClick
        Try
            Dim MyImagedata() As Byte
            Dim FileName As String = bm.GetNewTempName(CType(TreeView1.SelectedItem, TreeViewItem).Header)
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select ImageData from InvoicesAttachments where LabelData='" & LabelData.Text & "' and CarTypeId='" & CarTypeId.Text & "' and FileName='" & CType(TreeView1.SelectedItem, TreeViewItem).Header & "'", con)
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
            File.WriteAllBytes(FileName, MyImagedata)
            Process.Start(FileName)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        LabelData.Clear()
        CarTypeId.Clear()
        CarTypeName.Clear()

        LabelData.IsEnabled = True
        CarTypeId.IsEnabled = True

        TreeView1.Items.Clear()
    End Sub

    Private Sub CarTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CarTypeId.LostFocus
        bm.LostFocus(CarTypeId, CarTypeName, "select Name from CarTypes where Id=" & CarTypeId.Text.Trim())
    End Sub
    Private Sub CarTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CarTypeId.KeyUp
        If bm.ShowHelp("", CarTypeId, CarTypeName, e, "select cast(Id as varchar(100)) Id,Name from CarTypes", "") Then
            CarTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub


End Class
