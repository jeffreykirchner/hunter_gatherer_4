Public Class frmSetup5

    Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            For i As Integer = 1 To DataGridView1.RowCount
                Dim outstr As String = ""

                For j As Integer = 1 To DataGridView1.ColumnCount - 1
                    outstr &= DataGridView1.Rows(i - 1).Cells(j).Value & ";"
                Next

                writeINI(sfile, "groups", CStr(i), outstr)
            Next

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmSetup6_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            DataGridView1.RowCount = numberOfPeriods

            For i As Integer = 1 To DataGridView1.RowCount
                Dim msgtokens() As String = getINI(sfile, "groups", CStr(i)).Split(";")

                DataGridView1.Rows(i - 1).Cells(0).Value = i

                For j As Integer = 1 To DataGridView1.ColumnCount - 1
                    DataGridView1.Rows(i - 1).Cells(j).Value = msgtokens(j - 1)
                Next
            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdCopyDown_Click(sender As System.Object, e As System.EventArgs) Handles cmdCopyDown.Click
        Try
            Dim currentRow As Integer = DataGridView1.CurrentCell.RowIndex
            Dim currentColumn As Integer = DataGridView1.CurrentCell.ColumnIndex

            For i As Integer = currentRow + 1 To numberOfPeriods
                For j As Integer = 1 To DataGridView1.ColumnCount - 1
                    DataGridView1.Rows(i - 1).Cells(j).Value = DataGridView1.Rows(currentRow).Cells(j).Value
                Next
            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class