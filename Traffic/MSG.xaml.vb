﻿Imports System
Imports System.Threading
Public Class MSG
    Dim bm As New BasicMethods
    Public Ok As Boolean
    Public DelMsg As Boolean = False
    Public MSG As String

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        'LoadResource()
        Ok = False
        If Not DelMsg Then
            btnNo.Width = 0
            btnNo.Height = 0
            btnYes.Content = "خروج"
            btnYes.Focus()
        End If
        lblMSG.Content = MSG
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNo.Click
        Ok = False
        Close()
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnYes.Click
        Ok = True
        Close()
    End Sub


End Class
