Public Class frmSetup4

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            For i As Integer = 1 To numberOfPlayers
                Dim outstr As String = ""

                outstr = DataGridView1.Rows(i - 1).Cells(1).Value & ";"
                outstr &= DataGridView1.Rows(i - 1).Cells(2).Value & ";"

                writeINI(sfile, "players", CStr(i), outstr)
            Next

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdRandomize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRandomize.Click
        Try
            For i As Integer = 1 To numberOfPlayers
                DataGridView1.Rows(i - 1).Cells(1).Value = rand(1680 * 4 - 100, 1680 * 2 + 100)
                DataGridView1.Rows(i - 1).Cells(2).Value = rand(1050 * 6 - 100, 100)
            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmSetup4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DataGridView1.RowCount = numberOfPlayers

            For i As Integer = 1 To numberOfPlayers
                Dim msgtokens() As String = getINI(sfile, "players", CStr(i)).Split(";")

                DataGridView1.Rows(i - 1).Cells(0).Value = i
                DataGridView1.Rows(i - 1).Cells(1).Value = msgtokens(0)
                DataGridView1.Rows(i - 1).Cells(2).Value = msgtokens(1)
            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

End Class