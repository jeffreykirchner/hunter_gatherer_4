Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class LinePrimitive

    Dim myColor As Color        'Gets/sets the colour of the primitive line object.
    Dim Depth As Double        'Gets/sets the render depth of the primitive line object (0 = front, 1 = back)

    Dim V1 As Vector2
    Dim V2 As Vector2

    Dim distance As Double
    Dim angle As Double
    Dim pixel As Texture2D

    Dim tempTexture As Texture2D

    Dim eC1 As LinePrimitive
    Dim eC2 As LinePrimitive
    Dim eC3 As LinePrimitive

    Dim eCVectors(1000) As LinePrimitive
    Dim eCVectorsCount As Integer

    Dim sCVectors(1000) As LinePrimitive
    Dim sCVectorsCount As Integer


    Dim endCap As Boolean
    Dim startCap As Boolean

    Public Sub New(ByVal graphicsDevice As GraphicsDevice, ByVal X1 As Double, ByVal Y1 As Double, _
                   ByVal X2 As Double, ByVal Y2 As Double, ByVal tColor As Color, ByVal width As Double, _
                   ByVal drawEndCap As Boolean, ByVal drawStartCap As Boolean)

        V1 = New Vector2(X1, Y1)
        V2 = New Vector2(X2, Y2)

        myColor = tColor
        Depth = 0

        distance = Vector2.Distance(V1, V2)
        ' calculate the angle between the two vectors
        angle = Math.Atan2((V2.Y - V1.Y), (V2.X - V1.X))

        pixel = New Texture2D(graphicsDevice, 1, width, 1, TextureUsage.None, SurfaceFormat.Color)

        Dim pixels(width - 1) As Color
        For i As Integer = 1 To width
            pixels(i - 1) = New Color(255, 255, 255)
        Next

        pixel.SetData(pixels)

        endCap = drawEndCap
        startCap = drawStartCap

        If endCap Then
            'center of line
            Dim ecX1 As Double = width / 2 * Math.Cos(angle + Math.PI / 2) + V2.X
            Dim ecY1 As Double = width / 2 * Math.Sin(angle + Math.PI / 2) + V2.Y

            'peak of cap
            Dim ecX2 As Double = 20 * Math.Cos(angle) + ecX1
            Dim ecY2 As Double = 20 * Math.Sin(angle) + ecY1

            eCVectorsCount = 0
            'fill half
            For i As Double = 0 To width / 2 + 10 Step 0.5
                eCVectorsCount += 1
                Dim ecXt As Double = i * Math.Cos(angle + Math.PI / 2) + ecX1
                Dim ecYt As Double = i * Math.Sin(angle + Math.PI / 2) + ecY1
                eCVectors(eCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecXt, ecYt, tColor, 1, False, False)
            Next

            'fill other half
            For i As Double = 0 To width / 2 + 10 Step 0.5
                eCVectorsCount += 1
                Dim ecXt As Double = i * Math.Cos(angle - Math.PI / 2) + ecX1
                Dim ecYt As Double = i * Math.Sin(angle - Math.PI / 2) + ecY1
                eCVectors(eCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecXt, ecYt, tColor, 1, False, False)
            Next

            ''left corner
            Dim ecX3 As Double = (width / 2 + 10) * Math.Cos(angle + Math.PI / 2) + ecX1
            Dim ecY3 As Double = (width / 2 + 10) * Math.Sin(angle + Math.PI / 2) + ecY1

            ''right corner
            Dim ecX4 As Double = (width / 2 + 10) * Math.Cos(angle - Math.PI / 2) + ecX1
            Dim ecY4 As Double = (width / 2 + 10) * Math.Sin(angle - Math.PI / 2) + ecY1

            'border
            eCVectorsCount += 1
            eCVectors(eCVectorsCount) = New LinePrimitive(graphicsDevice, ecX3, ecY3, ecX4, ecY4, tColor, 1, False, False)

            eCVectorsCount += 1
            eCVectors(eCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecX3, ecY3, tColor, 1, False, False)

            eCVectorsCount += 1
            eCVectors(eCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecX4, ecY4, tColor, 1, False, False)
        End If

        If startCap Then

            Dim tempAngle = angle - Math.PI

            'center of line
            Dim ecX1 As Double = width / 2 * Math.Cos(tempAngle - Math.PI / 2) + V1.X
            Dim ecY1 As Double = width / 2 * Math.Sin(tempAngle - Math.PI / 2) + V1.Y

            'peak of cap
            Dim ecX2 As Double = 20 * Math.Cos(tempAngle) + ecX1
            Dim ecY2 As Double = 20 * Math.Sin(tempAngle) + ecY1

            sCVectorsCount = 0
            'fill half
            For i As Double = 0 To width / 2 + 10 Step 0.5
                sCVectorsCount += 1
                Dim ecXt As Double = i * Math.Cos(tempAngle + Math.PI / 2) + ecX1
                Dim ecYt As Double = i * Math.Sin(tempAngle + Math.PI / 2) + ecY1
                sCVectors(sCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecXt, ecYt, tColor, 1, False, False)
            Next

            'fill other half
            For i As Double = 0 To width / 2 + 10 Step 0.5
                sCVectorsCount += 1
                Dim ecXt As Double = i * Math.Cos(tempAngle - Math.PI / 2) + ecX1
                Dim ecYt As Double = i * Math.Sin(tempAngle - Math.PI / 2) + ecY1
                sCVectors(sCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecXt, ecYt, tColor, 1, False, False)
            Next

            ''left corner
            Dim ecX3 As Double = (width / 2 + 10) * Math.Cos(tempAngle + Math.PI / 2) + ecX1
            Dim ecY3 As Double = (width / 2 + 10) * Math.Sin(tempAngle + Math.PI / 2) + ecY1

            ''right corner
            Dim ecX4 As Double = (width / 2 + 10) * Math.Cos(tempAngle - Math.PI / 2) + ecX1
            Dim ecY4 As Double = (width / 2 + 10) * Math.Sin(tempAngle - Math.PI / 2) + ecY1

            'border
            sCVectorsCount += 1
            sCVectors(sCVectorsCount) = New LinePrimitive(graphicsDevice, ecX3, ecY3, ecX4, ecY4, tColor, 1, False, False)

            sCVectorsCount += 1
            sCVectors(sCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecX3, ecY3, tColor, 1, False, False)

            sCVectorsCount += 1
            sCVectors(sCVectorsCount) = New LinePrimitive(graphicsDevice, ecX2, ecY2, ecX4, ecY4, tColor, 1, False, False)
        End If
    End Sub

    Public Sub Render(ByVal sb As SpriteBatch)
        'line
        sb.Draw(pixel, V1, Nothing, myColor, angle, Vector2.Zero, New Vector2(distance, 1), SpriteEffects.None, Depth)


        'end cap

        If endCap Then
            For i As Integer = 1 To eCVectorsCount
                eCVectors(i).Render(sb)
            Next
        End If

        If startCap Then
            For i As Integer = 1 To eCVectorsCount
                sCVectors(i).Render(sb)
            Next
        End If

        'If eC1 IsNot Nothing Then eC1.Render(sb)
        'If eC2 IsNot Nothing Then eC2.Render(sb)
        'If eC3 IsNot Nothing Then eC3.Render(sb)
    End Sub
End Class
