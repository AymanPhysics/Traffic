Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class RPT2
    
    Public Flag As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        If Flag = 3 Then
            lblViolationTypeId.Visibility = Windows.Visibility.Hidden
            ViolationTypeId.Visibility = Windows.Visibility.Hidden
            ViolationTypeName.Visibility = Windows.Visibility.Hidden
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(2015, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubNoAreaId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub LabelTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles LabelTypeId.LostFocus
        bm.LostFocus(LabelTypeId, LabelTypeName, "select Name from LabelTypes where Id=" & LabelTypeId.Text.Trim())
    End Sub
    Private Sub LabelTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles LabelTypeId.KeyUp
        If bm.ShowHelp("", LabelTypeId, LabelTypeName, e, "select cast(Id as varchar(100)) Id,Name from LabelTypes", "") Then
            LabelTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CarTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CarTypeId.LostFocus
        bm.LostFocus(CarTypeId, CarTypeName, "select Name from CarTypes where Id=" & CarTypeId.Text.Trim())
    End Sub
    Private Sub CarTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CarTypeId.KeyUp
        If bm.ShowHelp("", CarTypeId, CarTypeName, e, "select cast(Id as varchar(100)) Id,Name from CarTypes", "") Then
            CarTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub ViolationTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ViolationTypeId.LostFocus
        bm.LostFocus(ViolationTypeId, ViolationTypeName, "select Name from ViolationTypes where Id=" & ViolationTypeId.Text.Trim())
         End Sub
    Private Sub ViolationTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ViolationTypeId.KeyUp
        If bm.ShowHelp("", ViolationTypeId, ViolationTypeName, e, "select cast(Id as varchar(100)) Id,Name from ViolationTypes", "") Then
            ViolationTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub
       
      
    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "@LabelData", "@OwnerName", "@LabelTypeId", "@CarTypeId", "@ViolationTypeId", "@IssueNo"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, LabelData.Text.Trim, OwnerName.Text.Trim, Val(LabelTypeId.Text), Val(CarTypeId.Text), Val(ViolationTypeId.Text), IssueNo.Text.Trim}
        Select Case Flag
            Case 1
                rpt.Rpt = "Invoices.rpt"
            Case 2
                rpt.Rpt = "DeletedInvoices.rpt"
            Case 3
                rpt.Rpt = "Invoices2.rpt"
        End Select
        rpt.ShowDialog()
    End Sub
End Class
