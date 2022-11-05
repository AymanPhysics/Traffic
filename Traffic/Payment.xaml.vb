Imports System.Data

Public Class Payment

    Dim bm As New BasicMethods
    WithEvents G As New MyGrid

    Private Sub Payment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        btnNew_Click(Nothing, Nothing)
        PayDate.IsEnabled = Md.Manager
    End Sub
    Structure GC
        Shared RowNumber As String = "RowNumber"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared Value As String = "Value"
        Shared DocNo As String = "DocNo"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.RowNumber, "العدد")
        G.Columns.Add(GC.InvoiceNo, "المسلسل")
        G.Columns.Add(GC.Value, "المبلغ")
        G.Columns.Add(GC.DocNo, "رقم القسيمة")
        G.Columns(GC.RowNumber).ReadOnly = True
        G.Columns(GC.InvoiceNo).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.AllowUserToAddRows = False
    End Sub



    Private Sub btnGet_Click(sender As Object, e As RoutedEventArgs) Handles btnGet.Click
        G.Rows.Clear()
        Dim dt As DataTable = bm.ExcuteAdapter("select Row_Number()over(order by InvoiceNo)RowNumber,InvoiceNo,Value,DocNo from Invoices where LabelData='" & LabelData.Text & "' and CarTypeId='" & CarTypeId.Text & "' and DocNo=''")
        If dt.Rows.Count > 0 Then
            LabelData.IsEnabled = False
            CarTypeId.IsEnabled = False
        End If
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)("RowNumber").ToString, dt.Rows(i)("InvoiceNo").ToString, dt.Rows(i)("Value").ToString, dt.Rows(i)("DocNo").ToString})
        Next
        G.RefreshEdit()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G.EndEdit()
        For i As Integer = 0 To G.Rows.Count - 1
            bm.ExcuteNonQuery("Update Invoices set DocNo='" & G.Rows(i).Cells(GC.DocNo).Value & "',PayDate='" & bm.ToStrDate(PayDate.SelectedDate) & "' where InvoiceNo='" & G.Rows(i).Cells(GC.InvoiceNo).Value & "'")
        Next
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        LabelData.Clear()
        CarTypeId.Clear()
        CarTypeName.Clear()

        LabelData.IsEnabled = True
        CarTypeId.IsEnabled = True

        PayDate.SelectedDate = bm.MyGetDate.Date

        G.Rows.Clear()
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
