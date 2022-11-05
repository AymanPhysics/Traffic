Imports System.Data

Public Class RPT7
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public RptFlag As Integer = 0
    Public RecipientTableName = "Recipients"

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@RecipientId", "@ComeId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {RptFlag, Val(RecipientId.Text), Val(ComeId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "Invoices.rpt"
            Case 2
                rpt.Rpt = "DeletedInvoices.rpt"
            Case 3
                rpt.Rpt = "ExpertsFollowUp.rpt"
            Case 4
                rpt.Rpt = "DeletedExpertsFollowUp.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        If Flag = 1 OrElse Flag = 2 Then
            lblAccusationId.Visibility = Windows.Visibility.Hidden
            RecipientId.Visibility = Windows.Visibility.Hidden
            RecipientName.Visibility = Windows.Visibility.Hidden
            lblSubNoId_Copy.Visibility = Windows.Visibility.Hidden
            ComeId.Visibility = Windows.Visibility.Hidden
            ComeName.Visibility = Windows.Visibility.Hidden
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub
    


    Private Sub RecipientId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RecipientId.LostFocus
        bm.LostFocus(RecipientId, RecipientName, "select Name from " & RecipientTableName & " where Id=" & RecipientId.Text.Trim())
    End Sub
    Private Sub RecipientId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles RecipientId.KeyUp
        If bm.ShowHelp("المكاتب", RecipientId, RecipientName, e, "select cast(Id as varchar(100)) Id,Name from " & RecipientTableName, RecipientTableName) Then
            RecipientId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ComeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ComeId.LostFocus
        bm.LostFocus(ComeId, ComeName, "select Name from Comes where Id=" & ComeId.Text.Trim())
    End Sub
    Private Sub ComeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ComeId.KeyUp
        If bm.ShowHelp("", ComeId, ComeName, e, "select cast(Id as varchar(100)) Id,Name from Comes", "Comes") Then
            ComeId_LostFocus(Nothing, Nothing)
        End If
    End Sub


End Class