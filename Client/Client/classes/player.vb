Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
'Imports System.Drawing

Public Class player
    Public myX As Double
    Public myY As Double

    Public targetX As Double
    Public targetY As Double

    Public moveCounter As Integer = 1
    Public avatarDirection As String = "down"

    Public myID As Integer
    Public colorName As String           'colorName of player

    Public showChat As Boolean

    Public labelTex As Texture2D
    Public unitsTex As Texture2D
    Public potTex As Texture2D
    Public healthTex As Texture2D
    Public stunnedTex As Texture2D

    Public tugAmountTex As Texture2D

    Public pullingIn As Boolean
    Public preyTarget As Integer

    Public unitsPlayer As Integer
    Public unitsPot As Integer

    Public fireX As Double
    Public fireY As Double
    Public fireMMX As Double
    Public fireMMY As Double

    Public currentHealth As Double
    Public stunned As Integer
    Public stunning As Integer
    Public coolDown As Integer

    Public tugging As Boolean
    Public tugger As Integer
    Public tugTarget As Integer
    Public tugAmount As Integer
    Public tugOpponet As Integer

    Public tugHelping As Boolean
    Public tugHelpTarget As Integer

    Public myGroup As Integer

    Public Sub intializePlayer(ByVal d As GraphicsDevice)
        Try
            Dim NewFont As Font
            Dim g As System.Drawing.Graphics
            Dim StringSize As SizeF
            Dim Bitmap As Bitmap
            Dim sf As New StringFormat
            Dim MemStream As System.IO.MemoryStream

            'font text setup
            'font/text setup
            NewFont = New Font("Microsoft Sans Serif", 14, FontStyle.Bold) ' The font that the text will be written in
            g = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
            sf.Alignment = StringAlignment.Center
            StringSize = g.MeasureString(colorName, NewFont) 'The length of the text

            'write the text onto the bitmap
            Bitmap = New Bitmap(CInt(StringSize.Width), CInt(StringSize.Height)) ' the bitmap that will hold the text
            g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display the text
            g.DrawString(colorName, NewFont, New SolidBrush(getMyColor(myID)), 0, 0) ' Draw the string on the bitmap

            MemStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
            Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png) ' save the bitmap to the portion of memory
            MemStream.Position = 0 ' dont know what this does, but it is necessary

            labelTex = Texture2D.FromFile(d, MemStream)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub movePlayer()
        Try
            With frmMain
                'check area not off
                If myID = inumber Then
                    If Not leftSideOn Then
                        If myX < 1680 * worldWidth * (2 / 6) And targetX < 1680 * worldWidth * (2 / 6) Then
                            myX = 1680 * worldWidth * (2 / 6) + 20
                            targetX = myX
                            sendUpdateTarget()
                            Exit Sub
                        End If

                    End If

                    If Not rightSideOn Or Not smallPreyAvailable Then
                        If myX >= 1680 * worldWidth * (4 / 6) And targetX >= 1680 * worldWidth * (4 / 6) Then
                            myX = 1680 * worldWidth * (4 / 6) - 20
                            targetX = myX
                            sendUpdateTarget()
                            Exit Sub
                        End If

                    End If

                    'player picks side
                    If myX < 1680 * worldWidth * (2 / 6) + 50 Then
                        If rightSideOn And leftSideOn Then
                            rightSideOn = False
                            .initializeMiniMap()

                            frmInstructions.pagesDone(3) = True

                            For i As Integer = 1 To 200
                                smallPrey(i).status = "out"
                            Next
                        End If
                    End If

                    If myX > 1680 * worldWidth * (4 / 6) - 50 Then
                        If rightSideOn And leftSideOn And smallPreyAvailable Then
                            leftSideOn = False
                            .initializeMiniMap()

                            For i As Integer = 1 To largePreyPerPeriod
                                largePrey(i).status = "out"
                            Next
                        End If
                    End If
                End If

                'move player
                If targetX < myX Then
                    myX -= playerSpeed
                    avatarDirection = "left"
                End If

                If targetX > myX Then
                    myX += playerSpeed
                    avatarDirection = "right"
                End If

                If targetY < myY Then
                    myY -= playerSpeed
                    avatarDirection = "up"
                End If

                If targetY > myY Then
                    myY += playerSpeed
                    avatarDirection = "down"
                End If

                If Math.Abs(targetX - myX) < playerSpeed Then myX = targetX
                If Math.Abs(targetY - myY) < playerSpeed Then myY = targetY

                If myX > 1680 * worldWidth - 165 Then myX = 1680 * worldWidth - 165
                If myX < 165 Then myX = 165
                If myY > 1050 * worldHeight - 180 Then myY = 1050 * worldHeight - 180
                If myY < 180 Then myY = 180

                If gameT2 Mod stepSpeed = 0 Then
                    If myX = targetX And myY = targetY Then
                        avatarDirection = "down"
                        moveCounter = 1
                    Else
                        If moveCounter = 4 Then
                            moveCounter = 1
                        Else
                            moveCounter += 1
                        End If
                    End If
                End If

                If pullingIn Then
                    'move prey
                    Dim tempAngle As Double
                    Dim dY As Double = 0
                    Dim dX As Double = 0

                    Dim tempPrey As New prey
                    If myX > 1680 * worldWidth * (4 / 6) Then 'small prey
                        tempPrey = smallPrey(preyTarget)
                    ElseIf myX < 1680 * worldWidth * (2 / 6) Then
                        tempPrey = largePrey(preyTarget)
                    Else
                        playerlist(inumber).preyTarget = -1
                        pullingIn = False
                    End If

                    If pullingIn Then

                        dY = myY - tempPrey.myY
                        dX = myX - tempPrey.myX
                        tempAngle = Math.Atan2(dY, dX)

                        tempPrey.myY += preyMovementRate * Math.Sin(tempAngle)
                        tempPrey.myX += preyMovementRate * Math.Cos(tempAngle)

                        tempPrey.targetX = tempPrey.myX
                        tempPrey.targetY = tempPrey.myY

                        If Math.Abs(myY - tempPrey.myY) < preyMovementRate And Math.Abs(myX - tempPrey.myX) < preyMovementRate Then

                            Dim tempFlee As Boolean = Not tempPrey.pullinResult

                            If tempPrey.myType = "small" Then
                                tempFlee = False
                            End If

                            'If myX < 1680 * worldWidth * (2 / 6) Then 'large prey flee
                            '    If rand(100, 1) > largePreyProbability Then
                            '        tempPrey.targetX = -1000
                            '        tempPrey.targetY = rand(worldHeight * 1050, 1)
                            '        tempFlee = True
                            '    End If
                            'End If

                            If tempFlee Then
                                tempPrey.targetX = -1000
                                tempPrey.targetY = rand(worldHeight * 1050, 1)
                            End If

                            pullingIn = False

                            If showInstructions Then
                                tempFlee = False
                                frmInstructions.pagesDone(4) = True
                                unitsPlayer += largePreyValue
                                updatePlayerTex()
                            End If

                            Dim outstr As String = preyTarget & ";"
                            outstr &= tempPrey.myType & ";"

                            If tempFlee Then
                                tempPrey.status = "flee"
                            Else
                                tempPrey.status = "caught"
                            End If

                            outstr &= tempPrey.status & ";"

                            If Not showInstructions Then wskClient.Send("05", outstr)

                        End If

                        If myX > 1680 * worldWidth * (4 / 6) Then 'small prey
                            smallPrey(preyTarget) = tempPrey
                        Else
                            largePrey(preyTarget) = tempPrey
                        End If
                    End If
                End If

                'update mini map hearths
                If inumber = myID Then
                    For i As Integer = 1 To numberOfPlayers
                        If i <> inumber Then
                            If returnDistance(myX, myY, playerlist(i).fireMMX, playerlist(i).fireMMY) < 700 Then
                                If playerlist(i).fireMMX <> playerlist(i).fireX Or playerlist(i).fireMMY <> playerlist(i).fireY Then
                                    playerlist(i).fireMMX = -1
                                    playerlist(i).fireMMY = -1
                                End If
                            End If

                            If returnDistance(myX, myY, playerlist(i).fireX, playerlist(i).fireY) < 700 Then
                                playerlist(i).fireMMX = playerlist(i).fireX
                                playerlist(i).fireMMY = playerlist(i).fireY
                            End If
                        End If
                    Next
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub drawPlayer(ByVal d As GraphicsDevice,
                          ByVal sb As SpriteBatch,
                          ByVal t As Texture2D,
                          ByVal tSpeechRight As Texture2D,
                          ByVal tSpeechLeft As Texture2D,
                          ByVal viewPortAdjustment As Vector2)
        Try
            With frmMain

                If myGroup <> playerlist(inumber).myGroup Then Exit Sub

                If myID <> inumber Then
                    If myX < 1680 * worldWidth * (2 / 6) - 5 Or myX > 1680 * worldWidth * (4 / 6) + 5 Then
                        Exit Sub
                    End If

                    If returnDistance(myX, myY, playerlist(inumber).myX, playerlist(inumber).myY) > 1000 Then
                        Exit Sub
                    End If
                End If


                Dim tempH As Integer = 215
                Dim tempW As Integer = 155
                Dim tempX As Integer
                Dim tempY As Integer

                If avatarDirection = "down" Then
                    tempX = 45
                ElseIf avatarDirection = "up" Then
                    tempX = 630
                ElseIf avatarDirection = "left" Then
                    tempX = 445
                ElseIf avatarDirection = "right" Then
                    tempX = 250
                End If

                If moveCounter = 1 Or moveCounter = 3 Then
                    tempY = 30
                ElseIf moveCounter = 2 Then
                    tempY = 260
                ElseIf moveCounter = 4 Then
                    tempY = 480
                End If

                Dim rect As New Rectangle(tempX, tempY, tempW, tempH) 'hands/head
                Dim rect2 As New Rectangle(830, 80, 90, 125)  'body
                Dim rect3 As New Rectangle(780, 705, 150, 40)

                If inumber = myID Then
                    Dim tempVector2 As Vector2
                    Dim tempVector3 As Vector2

                    Dim tempVector As New Vector2(.Panel2.Width / 2,
                                                  .Panel2.Height / 2)

                    'tempVector2 = New Vector2(.Panel2.Width / 2, .Panel2.Height / 2)

                    If avatarDirection = "up" Then
                        tempVector2 = New Vector2(.Panel2.Width / 2 + 1 * playerScale,
                                                  .Panel2.Height / 2 + 23 * playerScale)
                    ElseIf avatarDirection = "down" Then
                        tempVector2 = New Vector2(.Panel2.Width / 2 - 6 * playerScale,
                                                  .Panel2.Height / 2 + 23 * playerScale)
                    ElseIf avatarDirection = "left" Then
                        tempVector2 = New Vector2(.Panel2.Width / 2 + 7 * playerScale,
                                                  .Panel2.Height / 2 + 20 * playerScale)
                    ElseIf avatarDirection = "right" Then
                        tempVector2 = New Vector2(.Panel2.Width / 2 - 13 * playerScale,
                                                  .Panel2.Height / 2 + 20 * playerScale)
                    End If

                    'shadow
                    tempVector3 = New Vector2(.Panel2.Width / 2 - 5 * playerScale,
                                              .Panel2.Height / 2 + 75 * playerScale)

                    'sb.Draw(t, tempVector3, rect3, Color.White)
                    sb.Draw(t,
                              New Rectangle(tempVector3.X,
                                           tempVector3.Y,
                                           rect3.Width * playerScale,
                                           rect3.Height * playerScale),
                              rect3,
                              Color.White,
                              0,
                              New Vector2(rect3.Width / 2, rect3.Height / 2),
                              SpriteEffects.None, 0)

                    If avatarDirection = "up" Then
                        'sb.Draw(t, tempVector, rect, Color.White)
                        'sb.Draw(t, tempVector2, rect2, getMyColorXNA(myID))

                        sb.Draw(t,
                                New Rectangle(tempVector.X,
                                              tempVector.Y,
                                              rect.Width * playerScale,
                                              rect.Height * playerScale),
                                rect,
                                Color.White,
                                0,
                               New Vector2(rect.Width / 2, rect.Height / 2),
                                SpriteEffects.None, 0)

                        sb.Draw(t,
                                New Rectangle(tempVector2.X,
                                               tempVector2.Y,
                                               rect2.Width * playerScale,
                                               rect2.Height * playerScale),
                                rect2,
                                getMyColorXNA(myID),
                                0,
                                 New Vector2(rect2.Width / 2, rect2.Height / 2),
                                SpriteEffects.None, 0)
                    Else
                        'sb.Draw(t, tempVector2, rect2, getMyColorXNA(myID))
                        'sb.Draw(t, tempVector, rect, Color.White)

                        'body
                        sb.Draw(t,
                                New Rectangle(tempVector2.X,
                                               tempVector2.Y,
                                               rect2.Width * playerScale,
                                               rect2.Height * playerScale),
                                rect2,
                                getMyColorXNA(myID),
                                0,
                                New Vector2(rect2.Width / 2, rect2.Height / 2),
                                SpriteEffects.None, 0)

                        'head/hands
                        sb.Draw(t,
                               New Rectangle(tempVector.X,
                                            tempVector.Y,
                                            rect.Width * playerScale,
                                            rect.Height * playerScale),
                               rect,
                               Color.White,
                               0,
                               New Vector2(rect.Width / 2, rect.Height / 2),
                               SpriteEffects.None, 0)
                    End If

                    'color label
                    'sb.Draw(labelTex, tempVector + New Vector2(tempW / 2 - labelTex.Width / 2, -20), Color.White)
                    sb.Draw(labelTex,
                            New Rectangle(tempVector.X,
                                          tempVector.Y - 120 * playerScale,
                                          labelTex.Width * playerScale,
                                          labelTex.Height * playerScale),
                            New Rectangle(0, 0, labelTex.Width, labelTex.Height),
                            Color.White,
                            0,
                            New Vector2(labelTex.Width / 2, labelTex.Height / 2),
                            SpriteEffects.None, 0)

                    'stun label
                    If stunned > 0 Or coolDown > 0 Then
                        sb.Draw(stunnedTex, tempVector + New Vector2(tempW / 2 - stunnedTex.Width / 2, -35), Color.White)
                    End If

                    'units label
                    If avatarDirection = "down" Then
                        'sb.Draw(unitsTex, tempVector + New Vector2(tempW / 2 - unitsTex.Width / 2 - 5, +100), Color.White)
                        sb.Draw(unitsTex,
                           New Rectangle(tempVector.X - 3 * playerScale,
                                         tempVector.Y + 10 * playerScale,
                                         unitsTex.Width * playerScale,
                                         unitsTex.Height * playerScale),
                           New Rectangle(0, 0, unitsTex.Width, unitsTex.Height),
                           Color.White,
                           0,
                           New Vector2(unitsTex.Width / 2, unitsTex.Height / 2),
                           SpriteEffects.None, 0)

                        ' If showHealth Then sb.Draw(healthTex, tempVector + New Vector2(tempW / 2 - healthTex.Width / 2 - 2.5, +135), Color.White)

                        If showHealth Then
                            sb.Draw(healthTex,
                                    New Rectangle(tempVector.X - 3 * playerScale,
                                                  tempVector.Y + 40 * playerScale,
                                                  healthTex.Width * playerScale,
                                                  healthTex.Height * playerScale),
                                    New Rectangle(0, 0, healthTex.Width, healthTex.Height),
                                    Color.White,
                                    0,
                                    New Vector2(healthTex.Width / 2, healthTex.Height / 2),
                                    SpriteEffects.None, 0)
                        End If
                    End If

                    'tractor beam
                    If pullingIn Then
                        Dim tempPrey As prey
                        If myX > 1680 * worldWidth * (4 / 6) Then 'small prey
                            tempPrey = smallPrey(preyTarget)
                        Else
                            tempPrey = largePrey(preyTarget)
                        End If

                        drawTractorBeam(d, sb, viewPortAdjustment, tempPrey.myX, tempPrey.myY)
                    End If


                Else 'draw other people
                    Dim tempVector2 As Vector2
                    Dim tempVector3 As Vector2

                    Dim tempVector As New Vector2(myX, myY)

                    If avatarDirection = "up" Then
                        tempVector2 = New Vector2(myX + 1 * playerScale,
                                                  myY + 25 * playerScale)
                    ElseIf avatarDirection = "down" Then
                        tempVector2 = New Vector2(myX - 5 * playerScale,
                                                  myY + 23 * playerScale)
                    ElseIf avatarDirection = "left" Then
                        tempVector2 = New Vector2(myX + 7 * playerScale,
                                                  myY + 20 * playerScale)
                    ElseIf avatarDirection = "right" Then
                        tempVector2 = New Vector2(myX - 13 * playerScale,
                                                  myY + 20 * playerScale)
                    End If

                    tempVector3 = New Vector2(myX - 5 * playerScale,
                                              myY + 75 * playerScale)

                    'shadow
                    'sb.Draw(t, tempVector3 - viewPortAdjustment, rect3, Color.White)
                    sb.Draw(t,
                              New Rectangle(tempVector3.X - viewPortAdjustment.X,
                                            tempVector3.Y - viewPortAdjustment.Y,
                                            rect3.Width * playerScale,
                                            rect3.Height * playerScale),
                              rect3,
                              Color.White,
                              0,
                              New Vector2(rect3.Width / 2, rect3.Height / 2),
                              SpriteEffects.None, 0)

                    If avatarDirection = "up" Then
                        'sb.Draw(t, tempVector - viewPortAdjustment, rect, Color.White)
                        'sb.Draw(t, tempVector2 - viewPortAdjustment, rect2, getMyColorXNA(myID))

                        sb.Draw(t,
                              New Rectangle(tempVector.X - viewPortAdjustment.X,
                                            tempVector.Y - viewPortAdjustment.Y,
                                            rect.Width * playerScale,
                                            rect.Height * playerScale),
                              rect,
                              Color.White,
                              0,
                             New Vector2(rect.Width / 2, rect.Height / 2),
                              SpriteEffects.None, 0)

                        sb.Draw(t,
                                New Rectangle(tempVector2.X - viewPortAdjustment.X,
                                               tempVector2.Y - viewPortAdjustment.Y,
                                               rect2.Width * playerScale,
                                               rect2.Height * playerScale),
                                rect2,
                                getMyColorXNA(myID),
                                0,
                                 New Vector2(rect2.Width / 2, rect2.Height / 2),
                                SpriteEffects.None, 0)
                    Else
                        'sb.Draw(t, tempVector2 - viewPortAdjustment, rect2, getMyColorXNA(myID))
                        'sb.Draw(t, tempVector - viewPortAdjustment, rect, Color.White)

                        'body
                        sb.Draw(t,
                                New Rectangle(tempVector2.X - viewPortAdjustment.X,
                                               tempVector2.Y - viewPortAdjustment.Y,
                                               rect2.Width * playerScale,
                                               rect2.Height * playerScale),
                                rect2,
                                getMyColorXNA(myID),
                                0,
                                New Vector2(rect2.Width / 2, rect2.Height / 2),
                                SpriteEffects.None, 0)

                        'head/hands
                        sb.Draw(t,
                               New Rectangle(tempVector.X - viewPortAdjustment.X,
                                            tempVector.Y - viewPortAdjustment.Y,
                                            rect.Width * playerScale,
                                            rect.Height * playerScale),
                               rect,
                               Color.White,
                               0,
                               New Vector2(rect.Width / 2, rect.Height / 2),
                               SpriteEffects.None, 0)
                    End If

                    'color label
                    'sb.Draw(labelTex, tempVector - viewPortAdjustment + New Vector2(tempW / 2 - labelTex.Width / 2, -20), Color.White)
                    sb.Draw(labelTex,
                           New Rectangle(tempVector.X - viewPortAdjustment.X,
                                         tempVector.Y - viewPortAdjustment.Y - 120 * playerScale,
                                         labelTex.Width * playerScale,
                                         labelTex.Height * playerScale),
                           New Rectangle(0, 0, labelTex.Width, labelTex.Height),
                           Color.White,
                           0,
                           New Vector2(labelTex.Width / 2, labelTex.Height / 2),
                           SpriteEffects.None, 0)

                    'stun label
                    If stunned > 0 Or coolDown > 0 Then
                        sb.Draw(stunnedTex, tempVector - viewPortAdjustment + New Vector2(tempW / 2 - stunnedTex.Width / 2, -35), Color.White)
                    End If

                    'units label
                    If avatarDirection = "down" Then
                        'sb.Draw(unitsTex, tempVector - viewPortAdjustment + New Vector2(tempW / 2 - unitsTex.Width / 2 - 5, +100), Color.White)

                        sb.Draw(unitsTex,
                              New Rectangle(tempVector.X - viewPortAdjustment.X - 3 * playerScale,
                                            tempVector.Y - viewPortAdjustment.Y + 10 * playerScale,
                                            unitsTex.Width * playerScale,
                                            unitsTex.Height * playerScale),
                              New Rectangle(0, 0, unitsTex.Width, unitsTex.Height),
                              Color.White,
                              0,
                              New Vector2(unitsTex.Width / 2, unitsTex.Height / 2),
                              SpriteEffects.None, 0)

                        'If showHealth Then sb.Draw(healthTex, tempVector - viewPortAdjustment + New Vector2(tempW / 2 - healthTex.Width / 2 - 2.5, +135), Color.White)
                        If showHealth Then
                            sb.Draw(healthTex,
                                    New Rectangle(tempVector.X - viewPortAdjustment.X - 3 * playerScale,
                                                  tempVector.Y - viewPortAdjustment.Y + 40 * playerScale,
                                                  healthTex.Width * playerScale,
                                                  healthTex.Height * playerScale),
                                    New Rectangle(0, 0, healthTex.Width, healthTex.Height),
                                    Color.White,
                                    0,
                                    New Vector2(healthTex.Width / 2, healthTex.Height / 2),
                                    SpriteEffects.None, 0)
                        End If

                    End If
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub drawTractorBeam(ByVal d As GraphicsDevice,
                          ByVal sb As SpriteBatch,
                          ByVal viewPortAdjustment As Vector2,
                          targetX As Double,
                          targetY As Double)
        Try
            With frmMain

                If myGroup <> playerlist(inumber).myGroup Then Exit Sub

                'move prey
                Dim tempAngle As Double

                Dim dY As Double = 0
                Dim dX As Double = 0

                dY = myY - targetY
                dX = myX - targetX
                tempAngle = Math.Atan2(dY, dX)

                'draw tractor beam
                Dim tempSlope As Double = (myY - targetY) / (myX - targetX)
                If (myX - targetX) = 0 Then tempSlope = 0.999999999999

                Dim tempYIntercept As Double = myY - tempSlope * myX
                Dim rectTractor As Rectangle

                Dim tractorCircles As Integer = 15
                Dim tempScale As Double = 1 / tractorCircles * 0.75
                Dim xIncrement As Double = Math.Sqrt((myX - targetX) ^ 2 + (myY - targetY) ^ 2) / tractorCircles   'Math.Abs(myX - tempPrey.myX) / tractorCircles

                For i As Integer = 1 To tractorCircles

                    If .tractorBeamTex IsNot Nothing Then
                        rectTractor = New Rectangle(CInt(myX - Math.Cos(tempAngle) * xIncrement * i) - viewPortAdjustment.X - (.tractorBeamTex.Width * tempScale * i) / 2, _
                                                  CInt(myY - Math.Sin(tempAngle) * xIncrement * i) - viewPortAdjustment.Y - (.tractorBeamTex.Height * tempScale * i) / 2, _
                                                  .tractorBeamTex.Width * tempScale * i, _
                                                  .tractorBeamTex.Height * tempScale * i)


                        If gameT2 Mod 20 >= 10 Then
                            If i Mod 2 = 0 Then
                                sb.Draw(.tractorBeamTex, rectTractor, Color.White)
                            Else
                                sb.Draw(.tractorBeamTex, rectTractor, getMyColorXNA(myID))
                            End If
                        Else
                            If i Mod 2 = 0 Then
                                sb.Draw(.tractorBeamTex, rectTractor, getMyColorXNA(myID))
                            Else
                                sb.Draw(.tractorBeamTex, rectTractor, Color.White)
                            End If
                        End If

                    End If
                Next
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub drawPlayerRadius(ByVal d As GraphicsDevice,
                          ByVal sb As SpriteBatch,
                          ByVal t As Texture2D,
                          ByVal viewPortAdjustment As Vector2)
        Try
            With frmMain
                'Dim tempP As New System.Drawing.Point

                If myID <> inumber And showInstructions Then Exit Sub
                If myGroup <> playerlist(inumber).myGroup Then Exit Sub

                If myID <> inumber Then
                    If myX < 1680 * worldWidth * (2 / 6) - 5 Or myX > 1680 * worldWidth * (4 / 6) + 5 Then
                        Exit Sub
                    End If

                    If returnDistance(myX, myY, playerlist(inumber).myX, playerlist(inumber).myY) > 1500 Then
                        Exit Sub
                    End If
                End If

                Dim sRect As Rectangle
                Dim tempH As Integer = interactionRadius * 2
                Dim tempW As Integer = interactionRadius * 2

                If inumber = myID Then 'my chat
                    If (myX > 1680 * worldWidth * (2 / 6) And myX < 1680 * worldWidth * (4 / 6)) Then
                        Dim tempN As Integer = 10000

                        sRect = New Rectangle(.Panel2.Width / 2 - tempW / 2, .Panel2.Height / 2 - tempH / 2, interactionRadius * 2, interactionRadius * 2)

                        sb.Draw(t, sRect, Color.White)
                    End If
                Else 'other avatar's radius

                    If returnDistance(myX, myY, playerlist(inumber).myX, playerlist(inumber).myY) > 1000 Then
                        Exit Sub
                    End If

                    sRect = New Rectangle(myX - tempW / 2 - viewPortAdjustment.X, myY - tempH / 2 - viewPortAdjustment.Y, interactionRadius * 2, interactionRadius * 2)

                    Dim tempVector As New Vector2(myX - tempW / 2, myY - tempH / 2)
                    sb.Draw(t, sRect, Color.White)

                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub drawPlayerChat(ByVal d As GraphicsDevice, _
                          ByVal sb As SpriteBatch, _
                          ByVal t As Texture2D, _
                          ByVal tSpeechRight As Texture2D, _
                          ByVal tSpeechLeft As Texture2D, _
                          ByVal viewPortAdjustment As Vector2)

        Try
            With frmMain
                If myGroup <> playerlist(inumber).myGroup Then Exit Sub

                If showChat Then
                    Dim tempP As New System.Drawing.Point
                    Dim rect As Rectangle
                    Dim tempH As Integer = 215
                    Dim tempW As Integer = 155

                    If inumber = myID Then 'my chat
                        Dim tempN As Integer = 10000
                        Dim tempN2 As Integer = 0
                        Dim tempP2 As Integer = 1

                        For i As Integer = 1 To numberOfPlayers
                            If i <> inumber Then
                                tempN2 = Math.Abs(playerlist(i).myX - playerlist(inumber).myX) + Math.Abs(playerlist(i).myY - playerlist(inumber).myY)

                                If tempN2 < tempN Then
                                    tempN = tempN2
                                    tempP2 = i
                                End If
                            End If
                        Next

                        Dim tempVector As New Vector2(.Panel2.Width / 2 - tempW / 2, .Panel2.Height / 2 - tempH / 2)

                        If playerlist(inumber).myX > playerlist(tempP2).myX Then
                            tempP = New System.Drawing.Point(tempVector.X + 120, tempVector.Y - 40)

                            frmMain.setChatTextBox(tempP, True, myID)
                            rect = New Rectangle(0, 0, tSpeechRight.Width - 17, tSpeechRight.Height)
                            sb.Draw(tSpeechRight, New Vector2(tempVector.X + 90, tempVector.Y - 50), rect, Color.White)
                        Else
                            tempP = New System.Drawing.Point(tempVector.X - 143, tempVector.Y - 44)

                            frmMain.setChatTextBox(tempP, True, myID)
                            sb.Draw(tSpeechLeft, New Vector2(tempVector.X - 160, tempVector.Y - 52), Color.White)
                        End If
                    Else 'other avatar's chat

                        If returnDistance(myX, myY, playerlist(inumber).myX, playerlist(inumber).myY) > 1000 Then
                            frmMain.setChatTextBox(tempP, False, myID)
                            Exit Sub
                        End If

                        Dim tempVector As New Vector2(myX - tempW / 2, myY - tempH / 2)

                        If myX < playerlist(inumber).myX Then
                            tempP = New System.Drawing.Point(tempVector.X - 150 - viewPortAdjustment.X, tempVector.Y - 40 - viewPortAdjustment.Y)

                            sb.Draw(tSpeechLeft, New Vector2(tempVector.X - 160, tempVector.Y - 50) - viewPortAdjustment, Color.White)
                        Else
                            tempP = New System.Drawing.Point(tempVector.X + 120 - viewPortAdjustment.X, tempVector.Y - 40 - viewPortAdjustment.Y)

                            rect = New Rectangle(0, 0, tSpeechRight.Width - 17, tSpeechRight.Height)
                            sb.Draw(tSpeechRight, New Vector2(tempVector.X + 90, tempVector.Y - 50) - viewPortAdjustment, rect, Color.White)
                        End If

                        frmMain.setChatTextBox(tempP, True, myID)
                    End If

                Else 'dont show chat
                    frmMain.setChatTextBox(New Drawing.Point(0, 0), False, myID)
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try

    End Sub

    Public Sub drawFire(ByVal d As GraphicsDevice, _
                          ByVal sb As SpriteBatch, _
                          ByVal t As Texture2D, _
                          ByVal viewPortAdjustment As Vector2)
        Try
            If fireX = -1 Then Exit Sub

            If returnDistance(fireX, fireY, playerlist(inumber).myX, playerlist(inumber).myY) > 1000 Then
                Exit Sub
            End If

            'draw fire
            Dim rect As New Rectangle(60, 25, 155, 255)
            Dim tempVector As New Vector2(fireX - rect.Width / 2, fireY - rect.Height / 2)

            sb.Draw(t, tempVector - viewPortAdjustment, rect, Color.White)

            'draw bucket
            Dim rect2 As New Rectangle(240, 115, 100, 75)
            Dim tempVector2 As New Vector2(fireX - rect2.Width / 2 + 5, fireY - rect2.Height / 2 + 37)
            sb.Draw(t, tempVector2 - viewPortAdjustment, rect2, getMyColorXNA(myID))

            sb.Draw(potTex, New Vector2(fireX - potTex.Width / 2 + 5, fireY - rect2.Height / 2 + 55) - viewPortAdjustment, Color.White)

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function isOverPot(ByVal tempX As Integer, ByVal tempY As Integer) As Boolean
        Try
            If Math.Abs(tempX - fireX) < 155 / 2 And Math.Abs(tempY - fireY) < 255 / 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return False
        End Try
    End Function

    Public Function isOverPlayer(ByVal tempX As Integer, ByVal tempY As Integer) As Boolean
        Try
            If myGroup <> playerlist(inumber).myGroup Then Return False

            If Math.Abs(tempX - myX) < 155 / 2 And Math.Abs(tempY - myY) < 255 / 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return False
        End Try
    End Function

    Public Sub updateHealthBar()
        Try
            With frmMain
                'setup mini map background
                Dim rect As New System.Drawing.Rectangle(0, 0, 62, 27)

                Dim g As System.Drawing.Graphics

                'create bitmap
                Dim Bitmap As Bitmap = New Bitmap(rect.Width, rect.Height) ' the bitmap that will hold the graphics
                g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap

                g.FillRectangle(Brushes.LightGray, New System.Drawing.Rectangle(1, 1, 60, 25))
                g.FillRectangle(Brushes.Green, New System.Drawing.Rectangle(1, 1, 60 * (currentHealth / 100), 25))
                g.DrawRectangle(Pens.Black, New System.Drawing.Rectangle(1, 1, 60, 25))
                g.DrawImage(.healthMiniImg, 2, 2, .healthMiniImg.Width, .healthMiniImg.Height)
                g.DrawString(Math.Round(currentHealth, 1), New Font("Arial", 12, FontStyle.Bold), Brushes.Black, 19, 5)

                'write bitmap to screen
                Dim MemStream As System.IO.MemoryStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
                Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Bmp) ' save the bitmap to the portion of memory
                MemStream.Position = 0 ' dont know what this does, but it is necessary 
                healthTex = Texture2D.FromFile(.Device, MemStream)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updatePlayerTex()
        Try
            With frmMain
                If potsAvailable Then
                    updateStringTex(.Device, unitsTex, 14, Drawing.Color.Black, unitsPlayer)
                Else
                    updateStringTex(.Device, unitsTex, 14, Drawing.Color.Black, unitsPlayer & "/" & hearthCapacity)
                End If

                updateStringTex(.Device, potTex, 14, Drawing.Color.Black, unitsPot & "/" & hearthCapacity)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
