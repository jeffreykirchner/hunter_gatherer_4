Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Particle2D

    Public texture As Texture2D
    Public position As Vector2
    Public rotation As Double
    Public scale As Double
    Public myColor As Color
    Public width As Integer
    Public height As Integer
    Public origin As Vector2
    Public depth As Integer

    Public Sub New()
        position = Vector2.Zero
        rotation = 0
        scale = 1
        myColor = Color.White
        origin = Vector2.Zero
        depth = 0
    End Sub

    Public Function getTexture() As Texture2D
        Return texture
    End Function

    Public Sub setTexture(ByVal newTexture As Texture2D)
        texture = newTexture
    End Sub

    Public Sub Render(ByVal sb As SpriteBatch)
        sb.Draw(texture, New Rectangle(position.X, position.Y, (width * scale), (height * scale)), _
                New Rectangle(0, 0, texture.Width, texture.Height), myColor, rotation, origin, SpriteEffects.None, depth)
    End Sub
End Class
