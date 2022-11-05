Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO

Public Class RPT9
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(txtMonth.Text) = 0 OrElse Val(txtYear.Text) = 0 Then Return

        Dim MyNow As DateTime = bm.MyGetDate()
        If Flag = 3 AndAlso Not (Val(txtMonth.Text) = MyNow.Month AndAlso Val(txtYear.Text) = MyNow.Year) AndAlso Val(EmpId.Text) = 0 Then
            bm.ShowMSG("برجاء اختيار موظف")
            EmpId.Focus()
            Return
        End If

        Dim rpt As New ReportViewer
        Select Case Flag
            Case 1, 4
                rpt.Rpt = "SalaryHistory.rpt"
            Case 2
                rpt.Rpt = "SalaryHistory2.rpt"
            Case 3
                bm.ExcuteNonQuery("CalcSalaryEmp2", New String() {"Month", "Year", "EmpId"}, New String() {Val(txtMonth.Text), Val(txtYear.Text), Val(EmpId.Text)})
                bm.ShowMSG("تم احتساب المرتبات")
                Return
        End Select

        rpt.paraname = New String() {"@EmpId", "@Month", "@Year", "@SummaryMonths", "Header"}
        rpt.paravalue = New String() {Val(EmpId.Text), txtMonth.Text, txtYear.Text, IIf(Flag = 4, 1, 0), CType(Parent, Page).Title}
        rpt.ShowDialog()

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        'LoadResource()
        bm.Addcontrol_MouseDoubleClick({EmpId})

        If Flag = 3 AndAlso Not Md.Manager Then
            txtMonth.IsEnabled = False
            txtYear.IsEnabled = False
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        txtMonth.Text = MyNow.Month
        txtYear.Text = MyNow.Year
        If Flag = 3 Then txtMonth.Clear()
    End Sub
    Private Sub LoadResource()

        Select Case Flag
            Case 1, 2
                Button2.SetResourceReference(System.Windows.Controls.Button.ContentProperty, "View Report")
            Case 3
                lblEmpId.Visibility = Windows.Visibility.Hidden
                EmpId.Visibility = Windows.Visibility.Hidden
                EmpName.Visibility = Windows.Visibility.Hidden
        End Select

        lblEmpId.SetResourceReference(Label.ContentProperty, "Employee")
        lblFromDate.SetResourceReference(System.Windows.Controls.Label.ContentProperty, "Month")
        lblFromDate_Copy.SetResourceReference(System.Windows.Controls.Label.ContentProperty, "Year")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtMonth.KeyDown, txtYear.KeyDown, EmpId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId.KeyUp
        If bm.ShowHelp("Employees", EmpId, EmpName, e, "Select cast(Id as varchar(10))Id,Name from Employees2") Then
            EmpId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        If Val(EmpId.Text.Trim) = 0 Then
            EmpId.Clear()
            EmpName.Clear()
            Return
        End If
        bm.LostFocus(EmpId, EmpName, "select Name from Employees2 where Id=" & EmpId.Text.Trim())
    End Sub



End Class