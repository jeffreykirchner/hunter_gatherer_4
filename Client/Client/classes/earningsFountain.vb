Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics


Public Class earningsFoutain
    Public labelTex As Microsoft.Xna.Framework.Graphics.Texture2D = Nothing
    Public myX As Double
    Public myY As Double
    Public targetX As Double
    Public targetY As Double
    Public speed As Double
    Public easOutSpeed As Double
    Public opacity As Integer
    Public enabled As Boolean
    Public myText As String
    Public minimumSpeed As Double
    Public symbol As String

    Public Sub move()
        Try
            If Not enabled Then Exit Sub

            If Math.Abs(myX - targetX) < speed And Math.Abs(myY - targetY) < speed Then
                myX = targetX
                myY = targetY
                enabled = False
                Exit Sub
            End If

            Dim tempAngle As Double
            Dim dY As Double = 0
            Dim dX As Double = 0

            dY = myY - targetY
            dX = myX - targetX
            tempAngle = Math.Atan2(dY, dX)

            myY -= speed * Math.Sin(tempAngle)
            myX -= speed * Math.Cos(tempAngle)

            speed *= easOutSpeed
            If speed <= 0 Then speed = easOutSpeed
            If speed < minimumSpeed Then speed = minimumSpeed

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub New(ByVal text As String,
                   ByVal d As Microsoft.Xna.Framework.Graphics.GraphicsDevice,
                   fontSize As Integer,
                   fontColor As Brush,
                   Optional symbol As String = "")
        Try
            Dim NewFont As Font
            Dim g As System.Drawing.Graphics
            Dim StringSize As SizeF
            Dim Bitmap As Bitmap
            Dim sf As New StringFormat
            Dim MemStream As System.IO.MemoryStream

            Dim tempS As String = text
            myText = text

            NewFont = New Font("Calibri", fontSize, FontStyle.Bold) ' The font that the text will be written in

            'font text setup
            'font/text setup

            g = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
            sf.Alignment = StringAlignment.Center
            StringSize = g.MeasureString(tempS, NewFont) 'The length of the text

            'write the text onto the bitmap
            Bitmap = New Bitmap(CInt(StringSize.Width), CInt(StringSize.Height)) ' the bitmap that will hold the text
            g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display the text

            g.DrawString(tempS, NewFont, fontColor, 0, 0) ' Draw the string on the bitmap

            MemStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
            Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png) ' save the bitmap to the portion of memory
            MemStream.Position = 0 ' dont know what this does, but it is necessary

            labelTex = Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(d, MemStream)

            Me.symbol = symbol

            enabled = True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub draw(mainSB As SpriteBatch, tempV As Vector2)
        Try
            With frmMain
                If enabled Then

                    mainSB.Draw(labelTex,
                                New Vector2(myX - labelTex.Width / 2, myY - labelTex.Height / 2) - tempV,
                                Color.White)

                    If symbol = "health" Then
                        If .healthFountainTex IsNot Nothing Then
                            mainSB.Draw(.healthFountainTex,
                                   New Vector2(myX + labelTex.Width / 2, myY - .healthFountainTex.Height / 2) - tempV,
                                   New Rectangle(0, 0, .healthFountainTex.Width, .healthFountainTex.Height),
                                   Color.White,
                                   0,
                                   New Vector2(0, 0),
                                   1,
                                   SpriteEffects.None,
                                   0)
                        End If
                    ElseIf symbol = "yield" Then
                        mainSB.Draw(.yeildTex,
                              New Vector2(myX + .yeildTex.Width / 2 - 50, myY - .yeildTex.Height / 2) - tempV,
                              New Rectangle(0, 0, .yeildTex.Width, .yeildTex.Height),
                              Color.White,
                              0,
                              New Vector2(0, 0),
                              1,
                              SpriteEffects.None,
                              0)
                    End If
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
