Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class clsTransfer
    Structure oneDot
        Public x As Integer
        Public y As Integer
        Public size As Double   '% of orginal size
        Public direction As String
    End Structure

    Public dots(100) As oneDot
    Public dotCount As Integer = 0
    Public drawMe As Boolean = False

    Public Sub New(ByVal tempStartX As Double, ByVal tempStartY As Double, ByVal tempEndX As Double, ByVal tempEndY As Double)
        Try

            Dim dY As Double = tempStartY - tempEndY
            Dim dX As Double = tempStartX - tempEndX
            Dim tempAngle As Double = Math.Atan2(dY, dX)

            Dim tempDistance As Double = returnDistance(tempStartX, tempStartY, tempEndX, tempEndY)

            dotCount = tempDistance / 25
            If dotCount > 100 Then dotCount = 100

            Dim tempIncrement As Double = tempDistance / dotCount
            Dim tempD As Double = 0
            Dim tempIncrement2 = 100 / dotCount

            For i As Integer = 1 To dotCount
                dots(i).x = tempStartX - tempD * Math.Cos(tempAngle)
                dots(i).y = tempStartY - tempD * Math.Sin(tempAngle)
                dots(i).size = 100 - (i - 1) * tempIncrement2
                dots(i).direction = "up"

                tempD += tempIncrement
            Next

            drawMe = True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub draw(ByVal d As GraphicsDevice, ByVal sb As SpriteBatch, ByVal t As Texture2D, ByVal viewPortAdjustment As Vector2)
        Try
            If Not drawMe Then Exit Sub

            For i As Integer = 1 To dotCount
                If returnDistance(dots(i).x, dots(i).y, playerlist(inumber).myX, playerlist(inumber).myY) <= 1000 Then
                    Dim tempRect As New Rectangle(dots(i).x - t.Width / 2 * (dots(i).size / 100) - viewPortAdjustment.X _
                                                  , dots(i).y - t.Height / 2 * (dots(i).size / 100) - viewPortAdjustment.Y, _
                                                  t.Width * (dots(i).size / 100), t.Height * (dots(i).size / 100))

                    sb.Draw(t, tempRect, Color.White)


                    If gameT2 Mod 2 = 0 Then
                        If dots(i).direction = "up" Then
                            dots(i).size += 10
                            If dots(i).size > 100 Then
                                dots(i).size = 100
                                dots(i).direction = "down"
                            End If
                        Else
                            dots(i).size -= 10
                            If dots(i).size < 0 Then dots(i).size = 0
                        End If
                    End If
                End If
            Next

            If dots(dotCount).size = 0 And dots(dotCount).direction = "down" Then drawMe = False
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
