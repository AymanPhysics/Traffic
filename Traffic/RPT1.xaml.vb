Imports System.Data

Public Class RPT1
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0

    Public Sub Btn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click

        Dim rpt As New ReportViewer
        Dim MyFlag As Integer = 0
        Dim txt As String = ""
        Dim lbl As String = ""
        If sender Is btn1 Then
            MyFlag = 1
            txt = txt1
            lbl = lbl1.Content
        ElseIf sender Is btn2 Then
            MyFlag = 2
            txt = txt2
            lbl = lbl2.Content
        ElseIf sender Is btn3 Then
            MyFlag = 3
            txt = txt3
            lbl = lbl3.Content
        ElseIf sender Is btn4 Then
            MyFlag = 4
            txt = txt4
            lbl = lbl4.Content
        ElseIf sender Is btn5 Then
            MyFlag = 5
            txt = txt5
            lbl = lbl5.Content
        End If

        Dim Index As Integer = Val(bm.ExecuteScalar("CreateResetData", New String() {"DayDate", "Flag", "UserName"}, New String() {bm.ToStrDate(DayDate.SelectedDate), MyFlag, Md.UserName}))
        sender.Content = txt & " [ " & Index & " ] "

        rpt.paraname = New String() {"Id", "Flag", "DayDate", "UserName", "Header", "BtnName", "lbl"}
        rpt.paravalue = New String() {Index, MyFlag, DayDate.SelectedDate, Md.UserName, CType(Parent, Page).Title, txt,
            lbl}
        rpt.Rpt = "Reset.rpt"
        'rpt.ShowDialog()
        rpt.Print()
    End Sub

    Dim Index1, Index2, Index3, Index4, Index5 As Integer
    Dim txt1, txt2, txt3, txt4, txt5 As String
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        If Not Md.Manager Then
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
            Button2.Visibility = Windows.Visibility.Hidden
        End If

        txt1 = btn1.Content
        txt2 = btn2.Content
        txt3 = btn3.Content
        txt4 = btn4.Content
        txt5 = btn5.Content

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub


    Private Sub DayDate_SelectedDateChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DayDate.SelectedDateChanged

        Index1 = Val(bm.ExecuteScalar("select max(Id)Id from ResetData where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and Flag=1"))
        Index2 = Val(bm.ExecuteScalar("select max(Id)Id from ResetData where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and Flag=2"))
        Index3 = Val(bm.ExecuteScalar("select max(Id)Id from ResetData where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and Flag=3"))
        Index4 = Val(bm.ExecuteScalar("select max(Id)Id from ResetData where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and Flag=4"))
        Index5 = Val(bm.ExecuteScalar("select max(Id)Id from ResetData where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and Flag=5"))

        btn1.Content = txt1 & " [ " & Index1 & " ] "
        btn2.Content = txt2 & " [ " & Index2 & " ] "
        btn3.Content = txt3 & " [ " & Index3 & " ] "
        btn4.Content = txt4 & " [ " & Index4 & " ] "
        btn5.Content = txt5 & " [ " & Index5 & " ] "

    End Sub




    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "txt1", "txt2", "txt3", "txt4", "txt5"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, txt1, txt2, txt3, txt4, txt5}
        rpt.Rpt = "ResetAll.rpt"
        rpt.ShowDialog()
    End Sub
End Class