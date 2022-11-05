Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class RPT3

    Public Flag As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
         
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubNoAreaId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub
       


    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header", "LabelData", "OwnerName"}
        rpt.paravalue = New String() {CType(Parent, Page).Title, LabelData.Text.Trim, OwnerName.Text.Trim}
        Select Case Flag
            Case 1
                rpt.Rpt = "LabelData.rpt"
        End Select
        rpt.ShowDialog()
    End Sub
End Class
