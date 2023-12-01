Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports System.Drawing


Module modFont

    Dim NewFont As Font
    Dim g As Graphics
    Dim StringSize As SizeF
    Dim Bitmap As Bitmap
    'A couple of Global objects that will be set using the WriteText Sub below and run from the Render loop 
    'Public ScoreTexture As Texture2D = Nothing
    'Public ScoreVector As Vector2 = Nothing

    ''A couple of Global objects that will be set using the WriteText Sub below and run from the Render loop 
    'Public LoadingTexture As Texture2D = Nothing
    'Public LoadingVector As Vector2 = Nothing

    'The Sub that will create and define the necessary objects and variables
    Public Sub WriteText(ByVal str As String, ByVal FontName As String, ByVal FontSize As Single, ByVal _FontStyle As FontStyle, _
                         ByVal StringColour As Brush, ByRef Texture1 As Texture2D, _
                         ByRef Vector1 As Vector2, ByVal graphicsDevice As GraphicsDevice, ByVal sb As SpriteBatch, ByVal sf As StringFormat)

        NewFont = New Font(FontName, FontSize, _FontStyle) ' The font that the text will be written in
        g = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
        'write the text onto the bitmap
        StringSize = g.MeasureString(str, NewFont) 'The length of the text
        Bitmap = New Bitmap(CInt(StringSize.Width), CInt(StringSize.Height)) ' the bitmap that will hold the text

        g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display the text
        g.DrawString(str, NewFont, StringColour, 0, 0) ' Draw the string on the bitmap

        Dim MemStream As System.IO.MemoryStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
        Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png) ' save the bitmap to the portion of memory
        MemStream.Position = 0 ' dont know what this does, but it is necessary
        Texture1 = Texture2D.FromFile(graphicsDevice, MemStream)
        'bitmap that is stored in memory
        'Vector1 = New Vector2(BitmapXPos, BitmapYPos) ' set the position of the texture

        If sf.Alignment = StringAlignment.Center Then
            sb.Draw(Texture1, Vector1 - New Vector2(CInt(StringSize.Width) / 2, 0), Microsoft.Xna.Framework.Graphics.Color.White)
        Else
            sb.Draw(Texture1, Vector1, Microsoft.Xna.Framework.Graphics.Color.White)
        End If

    End Sub

End Module

