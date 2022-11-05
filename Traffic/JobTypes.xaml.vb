Imports System.Data

Public Class JobTypes
    Public TableName As String = "JobTypes"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
    Public TableDetailsName As String = "JobTypeItems"

    Public TableSubName As String = "JobInItems"
    Public TableSubName2 As String = "JobOutItems"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G As New MyGrid

    Public Flag As Integer = 0
    Public WithImage As Boolean = False

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        'LoadResource()
        bm.Fields = New String() {SubId, SubName, "FixInsMax", "ChInsMax"}
        bm.control = New Control() {txtID, txtName, FixInsMax, ChInsMax}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Structure GC
        Shared TypeId As String = "TypeId"
        Shared TypeName As String = "TypeName"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared MyType1Id As String = "MyType1Id"
        Shared Value As String = "Value"
        Shared MyType2Id As String = "MyType2Id"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.TypeId, "TypeId")
        G.Columns.Add(GC.TypeName, "نوع البند")

        G.Columns.Add(GC.Id, "كود البند")
        G.Columns.Add(GC.Name, "اسم البند")

        Dim GCMyType1Id As New Forms.DataGridViewComboBoxColumn
        GCMyType1Id.HeaderText = "النوع"
        GCMyType1Id.Name = GC.MyType1Id
        bm.FillCombo("select Id,Name FROM MyType1", GCMyType1Id)
        G.Columns.Add(GCMyType1Id)

        G.Columns.Add(GC.Value, "القيمة")

        Dim GCMyType2Id As New Forms.DataGridViewComboBoxColumn
        GCMyType2Id.HeaderText = "الحالة"
        GCMyType2Id.Name = GC.MyType2Id
        bm.FillCombo("select Id,Name FROM MyType2", GCMyType2Id)
        G.Columns.Add(GCMyType2Id)


        G.Columns(GC.TypeId).Visible = False
        G.Columns(GC.TypeName).ReadOnly = True
        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True

        G.Columns(GC.Name).FillWeight = 160
        G.Columns(GC.Value).FillWeight = 60

        G.AllowUserToAddRows = False
    End Sub


    Sub FillControls()
        bm.FillControls()

        Dim dt As DataTable = bm.ExcuteAdapter("select * from " & TableDetailsName & " where MainId=" & txtID.Text)
        For i As Integer = 0 To dt.Rows.Count - 1
            For x As Integer = 0 To G.Rows.Count - 1
                If G.Rows(x).Cells(GC.TypeId).Value = dt.Rows(i)("TypeId").ToString AndAlso G.Rows(x).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString Then
                    G.Rows(x).Cells(GC.MyType1Id).Value = dt.Rows(i)("MyType1Id").ToString
                    G.Rows(x).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
                    G.Rows(x).Cells(GC.MyType2Id).Value = dt.Rows(i)("MyType2Id").ToString
                End If
            Next
        Next
        G.RefreshEdit()

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        G.EndEdit()

        FixInsMax.Text = Val(FixInsMax.Text)
        ChInsMax.Text = Val(ChInsMax.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, TableDetailsName, New String() {"MainId"}, New String() {txtID.Text}, New String() {"TypeId", "Id", "Name", "MyType1Id", "Value", "MyType2Id"}, New String() {GC.TypeId, GC.Id, GC.Name, GC.MyType1Id, GC.Value, GC.MyType2Id}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Integer}, New String() {GC.Id}) Then Return

        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            txtName.Clear()

            G.Rows.Clear()
            dt = bm.ExcuteAdapter("select Id,Name from " & TableSubName)
            For i As Integer = 0 To dt.Rows.Count - 1
                G.Rows.Add({"1", "استحقاق", dt.Rows(i)(0), dt.Rows(i)(1), "1", 0, "1"})
            Next
            dt = bm.ExcuteAdapter("select Id,Name from " & TableSubName2)
            For i As Integer = 0 To dt.Rows.Count - 1
                G.Rows.Add({"2", "استقطاع", dt.Rows(i)(0), dt.Rows(i)(1), "1", 0, "1"})
            Next

            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExcuteNonQuery("delete from " & TableDetailsName & " where MainId='" & txtID.Text.Trim & "'")
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
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Dim lop As Boolean = False
    Private Sub CheckedChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim ch As CheckBox = sender
        Dim p As TreeViewItem = ch.Parent

        If Not lop Then
            For Each n As TreeViewItem In p.Items
                CType(n.Header, CheckBox).IsChecked = ch.IsChecked
            Next
        End If

        If p.Parent.GetType.ToString = "System.Windows.Controls.TreeViewItem" Then
            lop = True
            Dim PP As TreeViewItem = p.Parent
            If ch.IsChecked Then CType(PP.Header, CheckBox).IsChecked = True
            lop = False
        End If
    End Sub



End Class
