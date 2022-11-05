Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class Invoices
    Public TableName As String = "Invoices"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Dim TcpClientActivex1 As New TCPClientActivex
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        If Not Md.Manager Then
            btnPrint_Copy.Visibility = Windows.Visibility.Hidden
            IsPayed.Visibility = Windows.Visibility.Hidden
        End If

        LoadWFH()

        bm.Fields = New String() {SubId, "LabelData", "DayDate", "LabelTypeId", "CarTypeId", "ViolationTypeId", "Notes", "OwnerName", "MinValue", "MaxValue", "Value", "IsPayed", "PayDate", "DocNo", "IssueNo"}
        bm.control = New Control() {txtID, LabelData, DayDate, LabelTypeId, CarTypeId, ViolationTypeId, Notes, OwnerName, MinValue, MaxValue, Value, IsPayed, PayDate, DocNo, IssueNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        IsPayed_Checked(Nothing, Nothing)
        IsPayed.Visibility = Windows.Visibility.Hidden

        PayDate.IsEnabled = Md.Manager
        DocNo.IsEnabled = Md.Manager

        btnNew_Click(sender, e)
    End Sub

    Private Sub LoadWFH()
        WFH.Child = TcpClientActivex1
        WFH.Visibility = Windows.Visibility.Hidden
        btnCamera.Visibility = Windows.Visibility.Hidden
        btnCancel.Visibility = Windows.Visibility.Hidden
        Image1.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

        UndoNewId()
        LoadTree()

        btnSave.IsEnabled = Md.Manager
        btnDelete.IsEnabled = Md.Manager
        btnPrint.IsEnabled = Md.Manager
        btnPrint2.IsEnabled = Md.Manager

        LabelTypeId_LostFocus(Nothing, Nothing)
        CarTypeId_LostFocus(Nothing, Nothing)
        ViolationTypeId_LostFocus(Nothing, Nothing)

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
        If Not Md.Manager AndAlso (Val(Value.Text) < Val(MinValue.Text) OrElse Val(Value.Text) > Val(MaxValue.Text)) Then
            bm.ShowMSG("برجاء تحديد مبلغ صحيح")
            Value.Focus()
            Return
        End If

        MinValue.Text = Val(MinValue.Text)
        MaxValue.Text = Val(MaxValue.Text)
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
        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

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
        bm.ExcuteNonQuery("BeforeDeleteInvoices", New String() {"InvoiceNo", "UserDelete", "State"}, New String() {txtID.Text, Md.UserName, State})
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
        bm.SetNoImage(Image1)

        LoadTree()

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True
        btnPrint.IsEnabled = True
        btnPrint2.IsEnabled = True

        LabelTypeId_LostFocus(Nothing, Nothing)
        CarTypeId_LostFocus(Nothing, Nothing)
        ViolationTypeId_LostFocus(Nothing, Nothing)

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
        bm.LostFocus(ViolationTypeId, MinValue, "select MinValue from ViolationTypes where Id=" & ViolationTypeId.Text.Trim())
        bm.LostFocus(ViolationTypeId, MaxValue, "select MaxValue from ViolationTypes where Id=" & ViolationTypeId.Text.Trim())
    End Sub
    Private Sub ViolationTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ViolationTypeId.KeyUp
        If bm.ShowHelp("", ViolationTypeId, ViolationTypeName, e, "select cast(Id as varchar(100)) Id,Name from ViolationTypes", "") Then
            ViolationTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable(New String() {SubId}, New String() {txtID.Text.Trim}, cboSearch)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub Print(sender As Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "InvoicesOne.rpt"
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
        rpt.Rpt = "DeletedInvoicesOne.rpt"
        rpt.ShowDialog()
    End Sub





    Private Sub btnCamera_Click(sender As Object, e As RoutedEventArgs) Handles btnCamera.Click
        If TcpClientActivex1.isListening = False AndAlso TcpClientActivex1.isCameraOn Then
            Dim s = TcpClientActivex1.GetImage()
            TcpClientActivex1.StopCamera()
            'Dim ss As New Controls.im
            'TcpClientActivex1.Image = s
            sender.Content = "Start Camera"
        Else
            TcpClientActivex1.StartCamera()
            TcpClientActivex1.FrameRate = 21
            sender.Content = "Stop Camera"
        End If
    End Sub

    Private Sub btnbtnScanner(sender As Object, e As RoutedEventArgs) Handles btnScanner.Click
        'bm.SetImageFromScanner(Image1)
        Dim FileName As String = bm.ExecuteScalar("select isnull(max(cast(replace(FileName,'.jpg','')as int)),0)+1 from InvoicesImages where InvoiceNo=" & Val(txtID.Text)) & ".jpg"
        Dim x As String = bm.SaveImageFromScannerToFile(FileName)
        bm.SaveFile("InvoicesImages", "InvoiceNo", txtID.Text, "FileName", (x.Split("\"))(x.Split("\").Length - 1), "ImageData", x)
        LoadTree()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As RoutedEventArgs) Handles btnBrowse.Click
        'bm.SetImage(Image1)
        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("InvoicesImages", "InvoiceNo", txtID.Text, "FileName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "ImageData", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub

    Private Sub LoadTree()
        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExcuteAdapter("select FileName from InvoicesImages where InvoiceNo=" & txtID.Text & " order by FileName")
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
                        bm.ExcuteNonQuery("delete from InvoicesImages where InvoiceNo=" & txtID.Text & " and FileName='" & CType(TreeView1.SelectedItem, TreeViewItem).Header & "'")
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
            myCommand = New SqlClient.SqlCommand("select ImageData from InvoicesImages where InvoiceNo=" & txtID.Text & " and FileName='" & CType(TreeView1.SelectedItem, TreeViewItem).Header & "'", con)
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
            File.WriteAllBytes(FileName, MyImagedata)
            Process.Start(FileName)
        Catch ex As Exception
        End Try 
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        bm.SetNoImage(Image1)
    End Sub

    Private Sub IsPayed_Checked(sender As Object, e As RoutedEventArgs) Handles IsPayed.Checked, IsPayed.Unchecked
        'Try
        '    If IsPayed.IsChecked Then
        '        PayDate.Visibility = Windows.Visibility.Visible
        '        lblPayDate.Visibility = Windows.Visibility.Visible
        '        Dim MyNow As DateTime = bm.MyGetDate()
        '        PayDate.SelectedDate = MyNow
        '    Else
        '        PayDate.Visibility = Windows.Visibility.Hidden
        '        lblPayDate.Visibility = Windows.Visibility.Hidden
        '    End If
        'Catch ex As Exception
        'End Try
    End Sub
End Class
