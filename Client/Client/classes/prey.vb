Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics


Public Class prey
    Public myX As Double
    Public myY As Double
    Public targetX As Double
    Public targetY As Double
    Public myType As String
    Public status As String
    Public value As Integer
    Public width As Integer
    Public myID As Integer
    Public pullinResult As Boolean

    'font setup
    Dim NewFont As Font
    Dim g As System.Drawing.Graphics
    Dim StringSize As SizeF
    Dim Bitmap As Bitmap
    Dim sf As New StringFormat
    Dim labelTex As Texture2D
    Dim MemStream As System.IO.MemoryStream

    Public Sub setup(ByVal d As GraphicsDevice)
        Try


            'location setup
            newLocation()

            'font text setup
            'font/text setup
            NewFont = New Font("Microsoft Sans Serif", 14, FontStyle.Bold) ' The font that the text will be written in
            g = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
            sf.Alignment = StringAlignment.Center
            StringSize = g.MeasureString(value, NewFont) 'The length of the text

            'write the text onto the bitmap
            Bitmap = New Bitmap(CInt(StringSize.Width), CInt(StringSize.Height)) ' the bitmap that will hold the text
            g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display the text
            g.DrawString(value, NewFont, Brushes.Black, 0, 0) ' Draw the string on the bitmap

            MemStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
            Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png) ' save the bitmap to the portion of memory
            MemStream.Position = 0 ' dont know what this does, but it is necessary

            labelTex = Texture2D.FromFile(d, MemStream)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub newLocation()
        Try
            If myType = "small" Then
                myX = rand(1680 * (smallPanelX + 1) - 100, 1680 * smallPanelX + 100)
                myY = rand(1050 * smallPanelY - 100, 100 + 1050 * (smallPanelY - 1))
            Else
                myX = rand(1680 * worldWidth * (2 / 6) - 100, 100)
                myY = rand(1050 * worldHeight - 100, 100)
            End If

            newTarget()

            status = "free"
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub move()
        Try
            If status = "struck" And playerlist(inumber).preyTarget <> myID Then
                status = "free"
                newTarget()
            End If

            If status <> "free" and status<>"flee" Then Exit Sub

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
            'If myType = "small" Then
            '    targetX = rand(1680 * (smallPanelX + 1) - 100, 1680 * smallPanelX + 100)
            '    targetY = rand(1050 * smallPanelY - 100, 100 + 1050 * (smallPanelY - 1))
            'Else
            targetX = rand(1680 * worldWidth * (2 / 6) - 100, 100)
            targetY = rand(1050 * worldHeight - 100, 100)
            'End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub draw(ByVal d As GraphicsDevice, ByVal sb As SpriteBatch, ByVal t As Texture2D, ByVal viewPortAdjustment As Vector2)
        Try
            If returnDistance(myX, myY, playerlist(inumber).myX, playerlist(inumber).myY) > 1000 Then
                Exit Sub
            End If

            If status = "caught" Then Exit Sub
            If status = "out" Then Exit Sub

            sb.Draw(t, New Vector2(myX - t.Width / 2, myY - t.Height / 2) - viewPortAdjustment, Color.White)
            sb.Draw(labelTex, New Vector2(myX, myY) - New Vector2(CInt(StringSize.Width) / 2, CInt(StringSize.Height) / 2) - viewPortAdjustment, Color.White)


        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function isOver(ByVal tempX As Integer, ByVal tempY As Integer) As Boolean
        Try
            If status = "caught" Then Return False
            If status = "flee" Then Return False
            If status = "out" Then Return False

            If Math.Abs(tempX - myX) < width / 2 And Math.Abs(tempY - myY) < width / 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return False
        End Try
    End Function
End Class
