Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports System.Drawing

Module modImage
    Public Sub drawImage(ByVal tempImage As Image, ByVal tempX As Integer, ByVal tempY As Integer, ByVal srcRect As System.Drawing.Rectangle, _
                         ByRef Texture1 As Texture2D, ByRef Vector1 As Vector2, ByVal graphicsDevice As GraphicsDevice, ByVal sb As SpriteBatch)
        Dim g As Graphics = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
        'write the text onto the bitmap
        'Dim StringSize As SizeF = Graphics.MeasureString(Str, NewFont) 'The length of the text
        Dim Bitmap As Bitmap = New Bitmap(srcRect.Width, srcRect.Height) ' the bitmap that will hold the text
        g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display 
        g.DrawImage(tempImage, tempX, tempY, tempImage.Width, tempImage.Height)
        g.DrawLine(Pens.Red, 5, 5, 20, 20)

        'Graphics.DrawString(Str, NewFont, StringColour, 0, 0) ' Draw the string on the bitmap
        Dim MemStream As System.IO.MemoryStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
        Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Bmp) ' save the bitmap to the portion of memory



        MemStream.Position = 0 ' dont know what this does, but it is necessary

        Texture1 = Texture2D.FromFile(graphicsDevice, MemStream)
        'bitmap that is stored in memory
        'Vector1 = New Vector2(BitmapXPos, BitmapYPos) ' set the position of the texture
        sb.Draw(Texture1, Vector1, Microsoft.Xna.Framework.Graphics.Color.White)
    End Sub
End Module
