Public Class explosion
    Public myX As Double
    Public myY As Double
    Public size As Double
    Public speed As Double
    Public easOutSpeed As Double
    Public enabled As Boolean
    Public owner As Integer
    Public rotation As Double

    Public Sub move()
        Try
            If Not enabled Then Exit Sub

            If size >= 1.2 Then enabled = False

            rotation += Math.PI / 12

            size += speed

            speed -= easOutSpeed

            If speed < easOutSpeed Then speed = easOutSpeed

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
