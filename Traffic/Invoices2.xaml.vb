Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class Invoices2
    Public TableName As String = "Invoices2"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Dim TcpClientActivex1 As New TCPClientActivex
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        If Not Md.Manager Then
            btnPrint_Copy.Visibility = Windows.Visibility.Hidden
        End If
        btnPrint_Copy.Visibility = Windows.Visibility.Hidden
         
        bm.Fields = New String() {SubId, "LabelData", "DayDate", "LabelTypeId", "CarTypeId", "EmpId", "Notes", "OwnerName", "Value", "DocNo", "IssueNo"}
        bm.control = New Control() {txtID, LabelData, DayDate, LabelTypeId, CarTypeId, EmpId, Notes, OwnerName, Value, DocNo, IssueNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName 
        EmpId.IsEnabled = False
        DayDate.IsEnabled = Md.Manager

        btnNew_Click(sender, e)
    End Sub
     
    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls() 

        UndoNewId() 

        btnSave.IsEnabled = Md.Manager
        btnDelete.IsEnabled = Md.Manager
        btnPrint.IsEnabled = Md.Manager
        btnPrint2.IsEnabled = Md.Manager

        LabelTypeId_LostFocus(Nothing, Nothing)
        CarTypeId_LostFocus(Nothing, Nothing)
        EmpId_LostFocus(Nothing, Nothing)


        DayDate.Focus()
    End Sub

    Sub NewId()
        txtID.Clear()
        txtID.IsEnabled = False
    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click, btnPrint2.Click 

        If bm.IF_Exists("select Row_Number()over(order by InvoiceNo)RowNumber,InvoiceNo,Value,DocNo from Invoices where LabelData='" & LabelData.Text & "' and CarTypeId='" & CarTypeId.Text & "' and DocNo=''") Then
            bm.ShowMSG("برجاء تسديد المخالفات أولا")
            Return
        End If

        Value.Text = Val(Value.Text)

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                LastEntry.Text = ""
            End If
            Return
        End If

        If sender Is btnPrint OrElse sender Is btnPrint2 Then
            State = BasicMethods.SaveState.Print
        End If
        TraceInvoice(State.ToString)

        If sender Is btnPrint OrElse sender Is btnPrint2 Then
            Print(sender)
        Else
            btnNew_Click(sender, e)
        End If
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExcuteNonQuery("BeforeDeleteInvoices2", New String() {"InvoiceNo", "UserDelete", "State"}, New String() {txtID.Text, Md.UserName, State})
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        EmpId.Text = Md.UserName
        EmpName.Text = Md.ArName

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True
        btnPrint.IsEnabled = True
        btnPrint2.IsEnabled = True

        LabelTypeId_LostFocus(Nothing, Nothing)
        CarTypeId_LostFocus(Nothing, Nothing)
        EmpId_LostFocus(Nothing, Nothing)

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        'If txtID.Text = "" Then txtID.Text = "1"

        NewId()
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, SubNoAreaId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown
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

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        bm.LostFocus(EmpId, EmpName, "select ArName from Employees where Id=" & EmpId.Text.Trim())
    End Sub

    Private Sub CarTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CarTypeId.LostFocus
        bm.LostFocus(CarTypeId, CarTypeName, "select Name from CarTypes where Id=" & CarTypeId.Text.Trim())
    End Sub
    Private Sub CarTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CarTypeId.KeyUp
        If bm.ShowHelp("", CarTypeId, CarTypeName, e, "select cast(Id as varchar(100)) Id,Name from CarTypes", "") Then
            CarTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

      

    Private Sub Print(sender As Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "Invoices2One.rpt"
        If sender Is btnPrint2 Then
            rpt.Print()
        Else
            rpt.ShowDialog()
        End If
    End Sub


    Private Sub btnPrint_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint_Copy.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "DeletedInvoices2One.rpt"
        rpt.ShowDialog()
    End Sub 
     
        
End Class
