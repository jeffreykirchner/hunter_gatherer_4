Public Class frmSetup3

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Cursor = Cursors.WaitCursor

            For i As Integer = 1 To numberOfPeriods
                writeINI(sfile, "smallPrey", CStr(i), DataGridView1.Rows(i - 1).Cells(1).Value)
            Next

            Dim outstr As String = ""

            For k As Integer = 1 To DataGridView2.RowCount
                outstr = ""

                For j As Integer = 2 To 6
                    outstr &= DataGridView2.Rows(k - 1).Cells(j).Value & ";"
                Next

                writeINI(sfile, "largePrey", DataGridView2.Rows(k - 1).Cells(0).Value & "-" & DataGridView2.Rows(k - 1).Cells(1).Value, outstr)
            Next

            Cursor = Cursors.Default
            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmSetup3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DataGridView1.RowCount = numberOfPeriods
            DataGridView2.RowCount = numberOfPeriods * numberOfPlayers

            Dim tempC As Integer = 0

            For i As Integer = 1 To numberOfPeriods
                DataGridView1.Rows(i - 1).Cells(0).Value = i
                DataGridView1.Rows(i - 1).Cells(1).Value = getINI(sfile, "smallPrey", CStr(i))

                For k As Integer = 1 To numberOfPlayers
                    DataGridView2.Rows(tempC).Cells(0).Value = i
                    DataGridView2.Rows(tempC).Cells(1).Value = k

                    Dim msgtokens() As String = getINI(sfile, "largePrey", i & "-" & k).Split(";")
                    Dim nextToken As Integer = 0

                    If msgtokens.Count > 1 Then
                        For j As Integer = 2 To 6
                            DataGridView2.Rows(tempC).Cells(j).Value = msgtokens(nextToken)
                            nextToken += 1
                        Next
                    End If

                    tempC += 1
                Next
            Next

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdRandomize_Click(sender As Object, e As EventArgs) Handles cmdRandomize.Click
        Try
            Cursor = Cursors.WaitCursor

            For i As Integer = 1 To DataGridView2.RowCount
                randomizeRow(i)
            Next

            Cursor = Cursors.Default
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdRandomizeSelectedPlayer_Click(sender As Object, e As EventArgs) Handles cmdRandomizeSelectedPlayer.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim tempPlayer As Integer = DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Cells(1).Value

            For i As Integer = 1 To DataGridView2.RowCount
                If DataGridView2.Rows(i - 1).Cells(1).Value = tempPlayer Then
                    randomizeRow(i)
                End If
            Next

            Cursor = Cursors.Default
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub randomizeRow(tempRow As Integer)
        Try
            For j As Integer = 2 To 6

                If rand(100, 1) > largePreyProbability Then
                    DataGridView2.Rows(tempRow - 1).Cells(j).Value = "False"
                Else
                    DataGridView2.Rows(tempRow - 1).Cells(j).Value = "True"
                End If

            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class