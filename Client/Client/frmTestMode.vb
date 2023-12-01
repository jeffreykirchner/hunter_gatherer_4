Public Class frmTestMode

    Private Sub cmdTakeControl_Click(sender As System.Object, e As System.EventArgs) Handles cmdTakeControl.Click
        Try
            With frmMain
                testMode = False

                .Timer3.Enabled = False

                Me.Close()
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class