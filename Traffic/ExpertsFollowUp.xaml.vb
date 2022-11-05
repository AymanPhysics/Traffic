Imports System.Data

Public Class ExpertsFollowUp
    Public MainTableName As String = "ExpertsFollowUpFlags"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "ExpertsFollowUp"
    Public MainId As String = "Flag"
    Public SubId As String = "InvoiceNo"
    Public RecipientTableName = "Recipients"

    Public Flag As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Public Structure FlagState
        'Don't forget to edit RPTs and Stored Procedures after Editing this structure
        Shared متابعة_الخبراء_أحوال As Integer = 1
        Shared متابعة_الخبراء_تجارى As Integer = 2
        Shared متابعة_الخبراء_ضرائب As Integer = 3
        Shared متابعة_الخبراء_عمال As Integer = 4
        Shared متابعة_الخبراء_مدنى As Integer = 5
        Shared متابعة_الخبراء_مساكن As Integer = 6
        Shared الطب_الشرعى_أحوال As Integer = 11
        Shared الطب_الشرعى_تجارى As Integer = 12
        Shared الطب_الشرعى_ضرائب As Integer = 13
        Shared الطب_الشرعى_عمال As Integer = 14
        Shared الطب_الشرعى_مدنى As Integer = 15
        Shared الطب_الشرعى_مساكن As Integer = 16
        Shared الموقوف_أحوال As Integer = 21
        Shared الموقوف_تجارى As Integer = 22
        Shared الموقوف_ضرائب As Integer = 23
        Shared الموقوف_عمال As Integer = 24
        Shared الموقوف_مدنى As Integer = 25
        Shared الموقوف_مساكن As Integer = 26
    End Structure

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        ''LoadResource()

        lblMain.Visibility = Windows.Visibility.Hidden
        CboMain.Visibility = Windows.Visibility.Hidden
        If Not Md.Manager Then btnPrint_Copy.Visibility = Windows.Visibility.Hidden

        bm.Fields = New String() {MainId, SubId, "DayDate", "appeal", "Year", "Related", "CirclId", "AppellantId", "AppellantId2", "DateFirst", "DateNext", "RecipientId", "ReleaseNo", "ReleaseDate", "Notes", "ComeId", "ComeDate", "AgencyId"}
        bm.control = New Control() {CboMain, txtID, DayDate, appeal, Year, Related, CirclId, AppellantId, AppellantId2, DateFirst, DateNext, RecipientId, ReleaseNo, ReleaseDate, Notes, ComeId, ComeDate, AgencyId}
        bm.KeyFields = New String() {MainId, SubId}
        bm.Table_Name = TableName

        bm.FillCombo(MainTableName, CboMain, "")
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()

        UndoNewId()

        'btnSave.IsEnabled = Md.Manager
        btnDelete.IsEnabled = Md.Manager
        'btnPrint.IsEnabled = Md.Manager
        'btnPrint2.IsEnabled = Md.Manager

        DayDate.IsEnabled = Md.Manager
        appeal.IsEnabled = Md.Manager
        Year.IsEnabled = Md.Manager
        Related.IsEnabled = Md.Manager
        CirclId.IsEnabled = Md.Manager
        AppellantId.IsEnabled = Md.Manager
        AppellantId2.IsEnabled = Md.Manager
        DateFirst.IsEnabled = Md.Manager
        'RecipientId.IsEnabled = Md.Manager
        'ReleaseNo.IsEnabled = Md.Manager
        'ReleaseDate.IsEnabled = Md.Manager
        'Notes.IsEnabled = Md.Manager
        'ComeId.IsEnabled = Md.Manager
        'ComeDate.IsEnabled = Md.Manager


        CirclId_LostFocus(Nothing, Nothing)
        AppellantId_LostFocus(Nothing, Nothing)
        AppellantId2_LostFocus(Nothing, Nothing)
        RecipientId_LostFocus(Nothing, Nothing)
        ComeId_LostFocus(Nothing, Nothing)
        AgencyId_LostFocus(Nothing, Nothing)

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
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click, btnPrint2.Click
        If Val(appeal.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblappeal.Content)
            appeal.Focus()
            Return
        End If
        If Val(Year.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblYear.Content)
            Year.Focus()
            Return
        End If
        'If Val(Related.Text) = 0 Then
        '    bm.ShowMSG("برجاء تحديد " & lblRelated.Content)
        '    Related.Focus()
        '    Return
        'End If
        If Val(CirclId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblCirclId.Content)
            CirclId.Focus()
            Return
        End If
        If Val(AppellantId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblAppellantId.Content)
            AppellantId.Focus()
            Return
        End If
        If Val(AppellantId2.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblAppellantId2.Content)
            AppellantId2.Focus()
            Return
        End If
        If DayDate.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد " & lblDayDate.Content)
            DayDate.Focus()
            Return
        End If
        If DateFirst.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد " & lblDateFirst.Content)
            DateFirst.Focus()
            Return
        End If
        If DateNext.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد " & lblDateNext.Content)
            DateNext.Focus()
            Return
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "=" & CboMain.SelectedValue.ToString)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}) Then
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
        bm.ExcuteNonQuery("BeforeDeleteExpertsFollowUp", New String() {"Flag", "InvoiceNo", "UserDelete", "State"}, New String() {CboMain.SelectedValue.ToString, txtID.Text, Md.UserName, State})
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        CboMain.SelectedValue = Flag
        bm.ClearControls()

        'btnSave.IsEnabled = True
        btnDelete.IsEnabled = True
        'btnPrint.IsEnabled = True
        'btnPrint2.IsEnabled = True

        DayDate.IsEnabled = True
        appeal.IsEnabled = True
        Year.IsEnabled = True
        Related.IsEnabled = True
        CirclId.IsEnabled = True
        AppellantId.IsEnabled = True
        AppellantId2.IsEnabled = True
        DateFirst.IsEnabled = True
        'RecipientId.IsEnabled = True
        'ReleaseNo.IsEnabled = True
        'ReleaseDate.IsEnabled = True
        'Notes.IsEnabled = True
        'ComeId.IsEnabled = True
        'ComeDate.IsEnabled = True


        CirclId_LostFocus(Nothing, Nothing)
        AppellantId_LostFocus(Nothing, Nothing)
        AppellantId2_LostFocus(Nothing, Nothing)
        RecipientId_LostFocus(Nothing, Nothing)
        AgencyId_LostFocus(Nothing, Nothing)
        ComeId_LostFocus(Nothing, Nothing)

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
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & "=" & CboMain.SelectedValue.ToString)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, appeal.KeyDown, Year.KeyDown, Related.KeyDown, CirclId.KeyDown, AppellantId.KeyDown, AppellantId2.KeyDown, RecipientId.KeyDown, ReleaseNo.KeyDown, ReleaseDate.KeyDown, ComeId.KeyDown, ComeDate.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub CirclId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CirclId.LostFocus
        bm.LostFocus(CirclId, CirclName, "select Name from ExpertsCircles where Id=" & CirclId.Text.Trim())
    End Sub
    Private Sub CirclId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CirclId.KeyUp
        If bm.ShowHelp("الدوائر", CirclId, CirclName, e, "select cast(Id as varchar(100)) Id,Name from ExpertsCircles", "ExpertsCircles") Then
            CirclId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub AgencyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AgencyId.LostFocus
        bm.LostFocus(AgencyId, AgencyName, "select Name from Agencies where Id=" & AgencyId.Text.Trim())
    End Sub
    Private Sub AgencyId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AgencyId.KeyUp
        If bm.ShowHelp("الخبراء", AgencyId, AgencyName, e, "select cast(Id as varchar(100)) Id,Name from Agencies", "Agencies") Then
            AgencyId_LostFocus(Nothing, Nothing)
        End If
    End Sub



    Private Sub AppellantId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AppellantId.LostFocus
        bm.LostFocus(AppellantId, AppellantName, "select Name from Appellants where Id=" & AppellantId.Text.Trim())
    End Sub
    Private Sub AppellantId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AppellantId.KeyUp
        If bm.ShowHelp("المتازعين", AppellantId, AppellantName, e, "select cast(Id as varchar(100)) Id,Name from Appellants", "Appellants") Then
            AppellantId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub AppellantId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AppellantId2.LostFocus
        bm.LostFocus(AppellantId2, Appellant2Name, "select Name from Appellants where Id=" & AppellantId2.Text.Trim())
    End Sub
    Private Sub AppellantId2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AppellantId2.KeyUp
        If bm.ShowHelp("المتازعين", AppellantId2, Appellant2Name, e, "select cast(Id as varchar(100)) Id,Name from Appellants", "Appellants") Then
            AppellantId2_LostFocus(Nothing, Nothing)
        End If
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


    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, cboSearch)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub Print(sender As Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {CboMain.SelectedValue.ToString, Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "ExpertsFollowUpOne.rpt"
        If sender Is btnPrint2 Then
            rpt.Print()
        Else
            rpt.ShowDialog()
        End If
    End Sub


    Private Sub btnPrint_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint_Copy.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {CboMain.SelectedValue.ToString, Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "DeletedExpertsFollowUpOne.rpt"
        rpt.ShowDialog()
    End Sub

End Class
