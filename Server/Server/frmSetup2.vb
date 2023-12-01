Public Class frmSetup2

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            For i As Integer = 1 To bushCount
                Dim outstr As String = ""

                outstr = DataGridView1.Rows(i - 1).Cells(1).Value & ";"
                outstr &= DataGridView1.Rows(i - 1).Cells(2).Value & ";"

                outstr &= DataGridView2.Rows(i - 1).Cells(1).Value & ";"
                outstr &= DataGridView2.Rows(i - 1).Cells(2).Value & ";"

                outstr &= DataGridView3.Rows(i - 1).Cells(1).Value & ";"
                outstr &= DataGridView3.Rows(i - 1).Cells(2).Value & ";"

                writeINI(sfile, "bushes", CStr(i), outstr)
            Next

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error:", ex)
        End Try
    End Sub

    Private Sub frmPeriodSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DataGridView1.RowCount = bushCount
            DataGridView2.RowCount = bushCount
            DataGridView3.RowCount = bushCount

            For i As Integer = 1 To bushCount
                DataGridView1.Rows(i - 1).Cells(0).Value = i
                DataGridView2.Rows(i - 1).Cells(0).Value = i
                DataGridView3.Rows(i - 1).Cells(0).Value = i

                Dim msgtokens() As String = getINI(sfile, "bushes", CStr(i)).Split(";")
                Dim nextToken As Integer = 0

                DataGridView1.Rows(i - 1).Cells(1).Value = msgtokens(nextToken)
                nextToken += 1

                DataGridView1.Rows(i - 1).Cells(2).Value = msgtokens(nextToken)
                nextToken += 1

                DataGridView2.Rows(i - 1).Cells(1).Value = msgtokens(nextToken)
                nextToken += 1

                DataGridView2.Rows(i - 1).Cells(2).Value = msgtokens(nextToken)
                nextToken += 1

                DataGridView3.Rows(i - 1).Cells(1).Value = msgtokens(nextToken)
                nextToken += 1

                DataGridView3.Rows(i - 1).Cells(2).Value = msgtokens(nextToken)
                nextToken += 1

            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try

    End Sub

    Private Sub cmdRandomize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRandomize.Click
        Try
            For i As Integer = 1 To bushCount
                DataGridView1.Rows(i - 1).Cells(1).Value = rand(6000, 5040)
                DataGridView1.Rows(i - 1).Cells(2).Value = rand(2000 * worldHeight / 6, 1050 * worldHeight / 6)

                DataGridView2.Rows(i - 1).Cells(1).Value = rand(4500, 3300)
                DataGridView2.Rows(i - 1).Cells(2).Value = rand(4000 * worldHeight / 6, 3100 * worldHeight / 6)

                DataGridView3.Rows(i - 1).Cells(1).Value = rand(5500, 4500)
                DataGridView3.Rows(i - 1).Cells(2).Value = rand(6000 * worldHeight / 6, 5200 * worldHeight / 6)
            Next

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try

    End Sub
End Class