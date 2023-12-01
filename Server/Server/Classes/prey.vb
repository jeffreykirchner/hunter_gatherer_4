Public Class prey
    Public myX As Double
    Public myY As Double
    Public myType As String
    Public status As String
    Public myID As Integer
    Public targetX As Double
    Public targetY As Double

    Public myOwner As Integer

    Public Sub newLocation()
        Try
            If myType = "small" Then
                myX = rand(1680 * (smallPanelX + 1) - 100, 1680 * smallPanelX + 100)
                myY = rand(1050 * smallPanelY - 100, 100 + 1050 * (smallPanelY - 1))
            Else
                myX = rand(1680 * 2 - 100, 100)
                myY = rand(1050 * largePanelY * 2 - 100, 1050 * (largePanelY * 2 - 2) + 100)
            End If

            newTarget()

            status = "free"
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub move()
        Try
            If status = "struck" And playerList(myOwner).preyTarget <> myID Then
                status = "free"
                newTarget()
            End If

            If status <> "free" And status <> "flee" Then Exit Sub

            Dim tempMovementRate As Integer

            If status = "flee" Then
                tempMovementRate = playerSpeed * 2
            Else
                tempMovementRate = preyMovementRate
            End If

            If targetX < myX Then
                myX -= tempMovementRate
            End If

            If targetX > myX Then
                myX += tempMovementRate
            End If

            If targetY < myY Then
                myY -= tempMovementRate
            End If

            If targetY > myY Then
                myY += tempMovementRate
            End If

            If Math.Abs(targetX - myX) < tempMovementRate Then myX = targetX
            If Math.Abs(targetY - myY) < tempMovementRate Then myY = targetY

            If myX = targetX And myY = targetY Then
                If status <> "flee" Then newTarget()
            End If

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub newTarget()
        Try
            If myType = "small" Then
                targetX = rand(1680 * (smallPanelX + 1) - 100, 1680 * smallPanelX + 100)
                targetY = rand(1050 * smallPanelY - 100, 100 + 1050 * (smallPanelY - 1))
            Else
                targetX = rand(1680 * 2 - 100, 100)
                targetY = rand(1050 * largePanelY * 2 - 100, 1050 * (largePanelY * 2 - 2) + 100)
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
