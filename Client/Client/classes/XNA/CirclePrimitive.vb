Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class CirclePrimitive
    Dim pixel As Texture2D
    Dim vectors As ArrayList

    Dim myColor As Color        'Gets/sets the colour of the primitive line object.
    Dim Position As Vector2    'Gets/sets the position of the primitive line object.
    Dim Depth As Double        'Gets/sets the render depth of the primitive line object (0 = front, 1 = back)

    Dim radius As Integer
    Dim sides As Integer

    Dim X As Integer
    Dim Y As Integer

    'Gets the number of vectors which make up the primtive line object.
    Public Function CountVectors() As Integer
        Return vectors.Count
    End Function


    ' Creates a new primitive line object.
    Public Sub New(ByVal graphicsDevice As GraphicsDevice, ByVal tRadius As Integer, ByVal tSides As Integer, _
                   ByVal tX As Integer, ByVal tY As Integer, ByVal tColor As Color, ByVal width As Integer)

        'create(pixels)
        pixel = New Texture2D(graphicsDevice, 1, width, 1, TextureUsage.None, SurfaceFormat.Color)

        Dim pixels(width - 1) As Color
        For i As Integer = 1 To width
            pixels(i - 1) = New Color(255, 255, 255)
        Next

        pixel.SetData(pixels)
        'pixel.SetData<Color>(pixels);

        myColor = tColor
        Depth = 0
        vectors = New ArrayList()

        radius = tRadius
        sides = tSides

        X = tX
        Y = tY

        CreateFilledCircle()
        Position = New Vector2(X, Y)
    End Sub

    ''Adds a vector to the primive live object.
    'Public Sub AddVector(ByVal vector As Vector2)
    '    vectors.Add(vector)
    'End Sub

    ''Insers a vector into the primitive line object.
    'Public Sub InsertVector(ByVal index As Integer, ByVal vector As Vector2)
    '    vectors.Insert(index, vectors)
    'End Sub

    ''Removes a vector from the primitive line object.
    'Public Sub RemoveVector(ByVal vector As Vector2)
    '    vectors.Remove(vector)
    'End Sub

    ''Removes a vector from the primitive line object.
    'Public Sub RemoveVector(ByVal index As Integer)
    '    vectors.RemoveAt(index)
    'End Sub

    ''Clears all vectors from the primitive line object.
    'Public Sub ClearVectors()
    '    vectors.Clear()
    'End Sub

    'Renders the primtive line object.
    Public Sub RenderFilledCircle(ByVal sb As SpriteBatch)

        If vectors.Count < 2 Then Exit Sub

        For i As Integer = 1 To vectors.Count - 1
            Dim v1 As Vector2 = vectors(i - 1)
            Dim v2 As Vector2 = vectors(i)

            ' calculate the distance between the two vectors
            Dim distance As Double = Vector2.Distance(v1, v2)

            ' calculate the angle between the two vectors
            Dim angle As Double = Math.Atan2((v2.Y - v1.Y), (v2.X - v1.X))

            ' stretch the pixel between the two vectors
            sb.Draw(pixel, Position + v1, Nothing, myColor, angle, Vector2.Zero, New Vector2(distance, 1), SpriteEffects.None, Depth)
        Next

    End Sub

    Public Sub RenderCircle(ByVal sb As SpriteBatch)

        Dim v1 As Vector2
        Dim v2 As Vector2
        Dim distance As Double
        Dim angle As Double

        For i As Integer = 1 To vectors.Count - 2 Step 2
            v1 = vectors(i - 1)
            v2 = vectors(i + 1)

            ' calculate the distance between the two vectors
            distance = Vector2.Distance(v1, v2)

            ' calculate the angle between the two vectors
            angle = Math.Atan2((v2.Y - v1.Y), (v2.X - v1.X))

            ' stretch the pixel between the two vectors
            sb.Draw(pixel, Position + v1, Nothing, myColor, angle, Vector2.Zero, New Vector2(distance, 1), SpriteEffects.None, Depth)
        Next

        v1 = vectors(vectors.Count - 2)
        v2 = vectors(vectors.Count - 1)

        ' calculate the distance between the two vectors
        distance = Vector2.Distance(v1, v2)

        ' calculate the angle between the two vectors
        angle = Math.Atan2((v2.Y - v1.Y), (v2.X - v1.X))

        ' stretch the pixel between the two vectors
        sb.Draw(pixel, Position + v1, Nothing, myColor, angle, Vector2.Zero, New Vector2(distance, 1), SpriteEffects.None, Depth)

    End Sub

    'Creates a circle starting from 0, 0.
    Public Sub CreateFilledCircle()

        vectors.Clear()

        Dim max As Double = 2 * Math.PI
        Dim dStep As Double = max / sides
        Dim theta As Double = 0

        Do While theta < max
            vectors.Add(New Vector2(radius * Math.Cos(theta), radius * Math.Sin(theta)))

            ' then add the first vector again so it's a complete loop
            vectors.Add(New Vector2(radius * Math.Cos(0), radius * Math.Sin(0)))
            theta += dStep
        Loop


    End Sub

End Class


