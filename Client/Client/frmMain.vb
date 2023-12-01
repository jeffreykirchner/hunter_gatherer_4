Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics



Public Class frmMain
    'Public WithEvents mobjSocketClient As TCPConnection
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Delegate Sub SetTextCallback2()

    Public Device As Graphics.GraphicsDevice = Nothing
    Public XNAGraphics As GraphicsDeviceManager
    'Public XNAGame As Game

    'Define SpriteBatch, Textures, and Rectangles for all Non-ball Tectures
    Public mainSB As SpriteBatch = Nothing
    Public NonBallTexCreationParams As TextureCreationParameters = TextureCreationParameters.Default
    Public BackgroundTex As Texture2D = Nothing
    Public BackgroundTexLeft As Texture2D = Nothing
    Public BackgroundTexRight As Texture2D = Nothing
    'Public BackgroundRect As Rectangle = Nothing
    Public PlayAreaTex As Texture2D = Nothing
    Public PlayAreaRect As Rectangle = Nothing
    Public BallLauncherTex As Texture2D = Nothing
    Public BallLauncherRect As Rectangle = Nothing
    Public ArrowSBRotate As Single = 0
    Public ArrowTex As Texture2D = Nothing
    Public speechTexRight As Texture2D = Nothing
    Public speechTexLeft As Texture2D = Nothing
    Public preySmallTex As Texture2D = Nothing
    Public preyLargeTex As Texture2D = Nothing
    Public miniMapLocationTex As Texture2D = Nothing
    Public tractorBeamTex As Texture2D = Nothing
    Public fireTex As Texture2D = Nothing
    Public potBorderTex As Texture2D = Nothing
    Public fireMarkerTex As Texture2D = Nothing

    Public moveTextTex As Texture2D = Nothing
    Public restTextTex As Texture2D = Nothing

    Public huntingIconTex As Texture2D = Nothing
    Public tradingIconTex As Texture2D = Nothing
    Public interimIconTex As Texture2D = Nothing
    Public timeRemainingTex As Texture2D = Nothing
    Public restIconTex As Texture2D = Nothing

    Public radiusTex As Texture2D = Nothing
    Public explosionTex As Texture2D = Nothing

    Public healthFountainTex As Texture2D
    Public yeildTex As Texture2D

    Public bushMiniImg As Image = Image.FromFile(Application.StartupPath & "\Graphics\bush1Mini.png")
    Public treeMiniImg As Image = Image.FromFile(Application.StartupPath & "\Graphics\tree1Mini.png")
    Public rockMiniImg As Image = Image.FromFile(Application.StartupPath & "\Graphics\moutainMini.png")

    Public healthMiniImg As Image = Image.FromFile(Application.StartupPath & "\Graphics\HealthSmall.png")

    Public miniMapTex As Texture2D = Nothing

    Public viewPortX As Integer = 0
    Public viewPortY As Integer = 0

    'Public targetX As Integer = viewPortX
    'Public targetY As Integer = viewPortY

    'Public emitter As ParticleEmitter2D

    'Public imgAvatarWalk As Image = Image.FromFile(System.Windows.Forms.Application.StartupPath & "\graphics\Avatar1.png")
    Public texAvatarWalk As Texture2D = Nothing
    Public texBush As Texture2D = Nothing
    Public texTree As Texture2D = Nothing
    Public texRock As Texture2D = Nothing

    Public chatBoxes(30) As TextBox

    Private healthScreen As screen
    Public imgHealth As Image = Image.FromFile(System.Windows.Forms.Application.StartupPath & "\graphics\Health.png")

    Public healthFadeAmount As Integer
    Public healthFadeTransparency As Integer = 0

    Public transferCount As Integer = 0
    Public transfers(1000) As clsTransfer

    Public healthTransitionColor As Drawing.Color


    Private Sub frmChat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try

            'if ALT+K are pressed kill the client
            'if ALT+Q are pressed bring up connection box
            If e.Alt = True Then
                If CInt(e.KeyValue) = CInt(Keys.K) Then
                    If MessageBox.Show("Close Program?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
                    modMain.closing = True
                    Me.Close()
                ElseIf CInt(e.KeyValue) = CInt(Keys.Q) Then
                    frmConnect.Show()
                End If
            Else
                'If e.KeyValue = Keys.W Then
                '    viewOffSetY += 10
                'ElseIf e.KeyValue = Keys.S Then
                '    viewOffSetY -= 10
                'ElseIf e.KeyValue = Keys.A Then
                '    viewOffSetX += 10
                'ElseIf e.KeyValue = Keys.D Then
                '    viewOffSetX -= 10
                'End If
            End If
        Catch ex As Exception
            appEventLog_Write("error frmChat_KeyDown:", ex)
        End Try
    End Sub

    Private Sub frmChat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            sfile = System.Windows.Forms.Application.StartupPath & "\client.ini"

            'take IP from command line
            Dim commandLine As String = Command()

            If commandLine <> "" Then
                writeINI(sfile, "Settings", "ip", commandLine)
            End If

            'connect
            myIPAddress = getINI(sfile, "Settings", "ip")
            myPortNumber = getINI(sfile, "Settings", "port")
            connect()

            'Start the 3d Device
            IntializeGraphics()

            'forces windows to only use the Onpaint method to draw the window
            ' Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque, True)

            Dim r As New System.Drawing.Rectangle(0, 0, Panel1.Width, Panel1.Height)
            healthScreen = New screen(Panel1, r)
        Catch ex As Exception
            appEventLog_Write("errorfrmChat_Load :", ex)
        End Try

    End Sub

    Public Sub recoverGraphics()
        Try

            If Device.GraphicsDeviceStatus = GraphicsDeviceStatus.NotReset Then
                Device.Reset()
            End If

        Catch ex As Exception
            Threading.Thread.Sleep(100)
            recoverGraphics()
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub IntializeGraphics()

        Try
            Dim presentParams As New PresentationParameters
            presentParams.SwapEffect = SwapEffect.Discard
            presentParams.MultiSampleType = MultiSampleType.None

            presentParams.IsFullScreen = False
            presentParams.BackBufferWidth = Panel2.Width
            presentParams.BackBufferHeight = Panel2.Height
            'presentParams.BackBufferCount = 1
            ' presentParams.DeviceWindowHandle = Panel2.Handle

            Dim XNAGraphicsAdapater As Microsoft.Xna.Framework.Graphics.GraphicsAdapter = Graphics.GraphicsAdapter.Adapters.Item(0)

            Device = New Graphics.GraphicsDevice(XNAGraphicsAdapater, DeviceType.Hardware, Panel2.Handle, presentParams)
            Device.RenderState.MultiSampleAntiAlias = False

            'XNAGame = New Game()

            mainSB = New SpriteBatch(Device)

            'BackgroundRect.Height = Panel2.Height
            'BackgroundRect.Width = Panel2.Width
            'NonBallTexCreationParams.TextureUsage = TextureUsage.Linear

            'NonBallTexCreationParams.Height = Panel2.Height
            'NonBallTexCreationParams.Height = Panel2.Width

            BackgroundTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\backgroundTile.png")
            BackgroundTexLeft = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\backgroundTileRed.png")
            BackgroundTexRight = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\backgroundTileBlue.png")
            speechTexLeft = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\speechBubbleLeft.png")
            speechTexRight = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\speechBubbleRight.png")
            preySmallTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\preySmall.png")
            preyLargeTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\preyLarge.png")
            miniMapLocationTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\miniMapLocation.png")
            tractorBeamTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\particle2.png")
            fireTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\Hearth1.png")
            potBorderTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\potBorder.png")
            fireMarkerTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\fireMarker.png")

            texAvatarWalk = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\AvatarWalk1.png")
            texBush = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\bush1.png")
            texTree = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\tree1.png")
            texRock = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\moutain.png")

            huntingIconTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\tractorIcon.png")
            tradingIconTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\tradeArrows.png")

            If smallPreyAvailable Then
                interimIconTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\interimIcon.png")
            Else
                interimIconTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\interimIcon2.png")
            End If

            restIconTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\speechBubbleIcon.png")

            radiusTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\Radius.png")
            explosionTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\explosion.png")

            healthFountainTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\Health2.png")
            yeildTex = Texture2D.FromFile(Device, Application.StartupPath & "\Graphics\yield2.png")

            updateStringTex(Device, restTextTex, 30, Drawing.Color.DimGray, "Break: Nobody can capture hexagons.")
            updateStringTex(Device, timeRemainingTex, 20, Drawing.Color.DimGray, " ")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub initializeMiniMap()
        Try
            'setup mini map background
            Dim rect As New System.Drawing.Rectangle(0, 0, 1680 * worldWidth * miniMapScale, 1050 * worldHeight * miniMapScale)
            Dim rect3 As New System.Drawing.Rectangle(0, 0, 1680 * worldWidth * miniMapScale - 1, 1050 * worldHeight * miniMapScale - 1)
            Dim g As System.Drawing.Graphics

            'create bitmap
            Dim Bitmap As Bitmap = New Bitmap(rect.Width, rect.Height) ' the bitmap that will hold the graphics
            g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap

            'background panels
            Dim rect2 As New System.Drawing.Rectangle(0, 0, rect3.Width / 3, rect3.Height)
            If leftSideOn Then
                g.FillRectangle(Brushes.IndianRed, rect2)
            Else
                g.FillRectangle(Brushes.Black, rect2)
            End If

            rect2 = New System.Drawing.Rectangle(rect3.Width / 3, 0, rect3.Width / 3, rect3.Height)
            g.FillRectangle(Brushes.Olive, rect2)

            rect2 = New System.Drawing.Rectangle(rect3.Width / 3 * 2, 0, rect3.Width / 3, rect3.Height)

            If smallPreyAvailable Then
                If rightSideOn Then
                    g.FillRectangle(Brushes.CadetBlue, rect2)
                Else
                    g.FillRectangle(Brushes.Black, rect2)
                End If
            End If

            'border
            g.DrawLine(Pens.DarkGray, CInt(rect3.Width / 3), 0, CInt(rect3.Width / 3), rect3.Height)
            g.DrawLine(Pens.DarkGray, CInt(rect3.Width / 3 * 2), 0, CInt(rect3.Width / 3 * 2), rect3.Height)

            If smallPreyAvailable Then
                g.DrawRectangle(Pens.DarkGray, rect3)
            Else
                g.DrawRectangle(Pens.DarkGray, rect3.X, rect3.Y, CInt(rect3.Width * (2 / 3)), rect3.Height)
            End If

            'bushes/trees/rocks
            Dim f As New Font("Arial", 8, FontStyle.Regular)

            If landMarks Then
                For i As Integer = 1 To bushCount
                    g.DrawImage(bushMiniImg, CInt(bushLocations(i).X * miniMapScale - 4), CInt(bushLocations(i).Y * miniMapScale - 7), bushMiniImg.Width, bushMiniImg.Height)
                    g.DrawImage(treeMiniImg, CInt(treeLocations(i).X * miniMapScale - 6), CInt(treeLocations(i).Y * miniMapScale - 8), treeMiniImg.Width, treeMiniImg.Height)
                    g.DrawImage(rockMiniImg, CInt(rockLocations(i).X * miniMapScale - 4), CInt(rockLocations(i).Y * miniMapScale - 3), rockMiniImg.Width, rockMiniImg.Height)
                Next
            End If

            'write bitmap to screen
            Dim MemStream As System.IO.MemoryStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
            Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Bmp) ' save the bitmap to the portion of memory
            MemStream.Position = 0 ' dont know what this does, but it is necessary 
            miniMapTex = Texture2D.FromFile(Device, MemStream)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Render(ByVal mainForm As frmMain)
        Try
            If Device Is Nothing Then Return

            viewPortX = playerlist(inumber).myX - Panel2.Width / 2
            viewPortY = playerlist(inumber).myY - Panel2.Height / 2

            gameT += 1

            'Clear the backbuffer to a white color
            Device.Clear(ClearOptions.Target, Microsoft.Xna.Framework.Graphics.Color.White, 1.0F, 0)

            mainSB.Begin(SpriteBlendMode.AlphaBlend)

            Dim tempV As New Vector2(viewPortX, viewPortY)

            Dim tempVAdjustment As New Vector2(playerlist(inumber).myX, playerlist(inumber).myY)

            'background
            For i As Integer = 1 To worldWidth
                For j As Integer = 1 To worldHeight
                    If returnDistance(backgroundTileLocations(i, j).X + BackgroundTex.Width / 2, backgroundTileLocations(i, j).Y + BackgroundTex.Height / 2, _
                                      playerlist(inumber).myX, playerlist(inumber).myY) <= 2000 Then
                        If i <= 2 Then
                            If leftSideOn Then
                                mainSB.Draw(BackgroundTexLeft, backgroundTileLocations(i, j) - tempV, Color.White)
                            Else
                                mainSB.Draw(BackgroundTexLeft, backgroundTileLocations(i, j) - tempV, Color.Black)
                            End If
                        ElseIf i <= 4 Then
                            mainSB.Draw(BackgroundTex, backgroundTileLocations(i, j) - tempV, Color.White)
                        Else
                            If smallPreyAvailable Then
                                If rightSideOn Then
                                    mainSB.Draw(BackgroundTexRight, backgroundTileLocations(i, j) - tempV, Color.White)
                                Else
                                    mainSB.Draw(BackgroundTexRight, backgroundTileLocations(i, j) - tempV, Color.Black)
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            If interactionRadius < 900 Then
                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).drawPlayerRadius(Device, mainSB, radiusTex, tempV)
                Next
            End If

            If landMarks Then
                'draw bushes
                For i As Integer = 1 To bushCount
                    If returnDistance(bushLocations(i).X, bushLocations(i).Y, playerlist(inumber).myX, playerlist(inumber).myY) <= 1000 Then
                        mainSB.Draw(texBush, bushLocations(i) - tempV, Color.White)
                    End If
                Next

                'draw trees
                For i As Integer = 1 To bushCount
                    If returnDistance(treeLocations(i).X, treeLocations(i).Y, playerlist(inumber).myX, playerlist(inumber).myY) <= 1000 Then
                        mainSB.Draw(texTree, treeLocations(i) - tempV, Color.White)
                    End If
                Next

                'draw rocks
                For i As Integer = 1 To bushCount
                    If returnDistance(rockLocations(i).X, rockLocations(i).Y, playerlist(inumber).myX, playerlist(inumber).myY) <= 1000 Then
                        mainSB.Draw(texRock, rockLocations(i) - tempV, Color.White)
                    End If
                Next
            End If

            'draw fire
            If Not showInstructions Then
                For i As Integer = 1 To numberOfPlayers
                    If i <> inumber Then
                        playerlist(i).drawFire(Device, mainSB, fireTex, tempV)
                    End If
                Next
            End If
            playerlist(inumber).drawFire(Device, mainSB, fireTex, tempV)


            Dim tempX As Double = playerlist(inumber).targetX - 5 - viewPortX
            Dim tempY As Double = playerlist(inumber).targetY - 5 - viewPortY
            Dim cP As New CirclePrimitive(Device, 10, 15, tempX, tempY, Color.Red, 2)
            cP.RenderCircle(mainSB)


            'transfer tractor beam
            For i As Integer = 1 To numberOfPlayers
                If playerlist(i).stunning > 0 Then
                    playerlist(i).drawTractorBeam(Device,
                                                  mainSB,
                                                  tempV,
                                                  playerlist(playerlist(i).stunning).myX,
                                                  playerlist(playerlist(i).stunning).myY)
                End If
            Next

            'tug of war tractor beam
            For i As Integer = 1 To numberOfPlayers
                If playerlist(i).tugging And
                   playerlist(i).tugger = i And
                   playerlist(i).myGroup = playerlist(inumber).myGroup Then

                    playerlist(i).drawTractorBeam(Device,
                                                 mainSB,
                                                 tempV,
                                                 playerlist(i).myX - 200,
                                                 playerlist(i).myY)

                    playerlist(playerlist(i).tugTarget).drawTractorBeam(Device,
                                                mainSB,
                                                tempV,
                                                playerlist(i).myX - 200,
                                                playerlist(i).myY)

                    mainSB.Draw(preySmallTex,
                                New Vector2(playerlist(i).myX - 200 - preySmallTex.Width / 2, playerlist(i).myY - preySmallTex.Height / 2) - tempV,
                                Color.White)

                    mainSB.Draw(playerlist(i).tugAmountTex,
                               New Vector2(playerlist(i).myX - 200 - playerlist(i).tugAmountTex.Width / 2,
                                           playerlist(i).myY - playerlist(i).tugAmountTex.Height / 2) - tempV,
                               Color.White)

                End If
            Next

            'help tug tractor beam
            For i As Integer = 1 To numberOfPlayers
                If playerlist(i).tugHelping Then
                    playerlist(i).drawTractorBeam(Device,
                                                  mainSB,
                                                  tempV,
                                                  playerlist(playerlist(i).tugHelpTarget).myX,
                                                  playerlist(playerlist(i).tugHelpTarget).myY)
                End If
            Next

            If Not showInstructions Then
                'draw players
                For i As Integer = 1 To numberOfPlayers
                    If i <> inumber Then playerlist(i).drawPlayer(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)
                Next
            End If

            If showInstructions Then
                playerlist(9).drawPlayer(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)
                playerlist(10).drawPlayer(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)

                'transfer tractor beam
                For i As Integer = 9 To 10
                    If playerlist(i).stunning > 0 Then
                        playerlist(i).drawTractorBeam(Device,
                                                      mainSB,
                                                      tempV,
                                                      playerlist(playerlist(i).stunning).myX,
                                                      playerlist(playerlist(i).stunning).myY)
                    End If
                Next

                'tug of war tractor beam
                For i As Integer = 9 To 10
                    If playerlist(i).tugging And playerlist(i).tugger = i Then
                        playerlist(i).drawTractorBeam(Device,
                                                     mainSB,
                                                     tempV,
                                                     playerlist(i).myX - 200,
                                                     playerlist(i).myY)

                        playerlist(playerlist(i).tugTarget).drawTractorBeam(Device,
                                                    mainSB,
                                                    tempV,
                                                    playerlist(i).myX - 200,
                                                    playerlist(i).myY)

                        mainSB.Draw(preySmallTex,
                                    New Vector2(playerlist(i).myX - 200 - preySmallTex.Width / 2, playerlist(i).myY - preySmallTex.Height / 2) - tempV,
                                    Color.White)

                        mainSB.Draw(playerlist(i).tugAmountTex,
                                   New Vector2(playerlist(i).myX - 200 - playerlist(i).tugAmountTex.Width / 2,
                                               playerlist(i).myY - playerlist(i).tugAmountTex.Height / 2) - tempV,
                                   Color.White)

                    End If
                Next

                'help tug tractor beam
                For i As Integer = 9 To 10
                    If playerlist(i).tugHelping Then
                        playerlist(i).drawTractorBeam(Device,
                                                      mainSB,
                                                      tempV,
                                                      playerlist(playerlist(i).tugHelpTarget).myX,
                                                      playerlist(playerlist(i).tugHelpTarget).myY)
                    End If
                Next
            End If

            'draw mini map

            mainSB.Draw(miniMapTex, New Vector2(5, 5), Microsoft.Xna.Framework.Graphics.Color.White)

            If Not showInstructions Then
                'hearths mini maps
                For i As Integer = 1 To numberOfPlayers
                    If i <> inumber And
                       playerlist(i).fireMMX <> -1 And
                       playerlist(i).myGroup = playerlist(inumber).myGroup Then

                        mainSB.Draw(fireMarkerTex, New Vector2(playerlist(i).fireMMX * miniMapScale - fireMarkerTex.Width / 2 + 5, _
                                                               playerlist(i).fireMMY * miniMapScale - fireMarkerTex.Height / 2 + 5), _
                                                               getMyColorXNA(i))

                    End If
                Next
            End If

            If playerlist(inumber).fireMMX <> -1 Then
                mainSB.Draw(fireMarkerTex, New Vector2(playerlist(inumber).fireMMX * miniMapScale - fireMarkerTex.Width / 2 + 5, _
                                                       playerlist(inumber).fireMMY * miniMapScale - fireMarkerTex.Height / 2 + 5), _
                                                       getMyColorXNA(inumber))
            End If

            'my marker
            mainSB.Draw(miniMapLocationTex, New Vector2(playerlist(inumber).myX * miniMapScale - miniMapLocationTex.Width / 2 + 5, _
                                                        playerlist(inumber).myY * miniMapScale - miniMapLocationTex.Height / 2 + 5), _
                                                        Microsoft.Xna.Framework.Graphics.Color.White)

            If phase = "interim" Then
                mainSB.Draw(moveTextTex, New Vector2(Panel2.Width / 2 - moveTextTex.Width / 2, 600), Microsoft.Xna.Framework.Graphics.Color.White)
                mainSB.Draw(interimIconTex, New Vector2(Panel2.Width - 85, 20), Microsoft.Xna.Framework.Graphics.Color.White)
            ElseIf phase = "hunting" Then
                If leftSideOn And rightSideOn Then mainSB.Draw(moveTextTex, New Vector2(Panel2.Width / 2 - moveTextTex.Width / 2, 600), Microsoft.Xna.Framework.Graphics.Color.White)
                mainSB.Draw(huntingIconTex, New Vector2(Panel2.Width - 75, 10), Microsoft.Xna.Framework.Graphics.Color.White)
            ElseIf phase = "rest" Then
                mainSB.Draw(restIconTex, New Vector2(Panel2.Width - 80, 15), Microsoft.Xna.Framework.Graphics.Color.White)
                mainSB.Draw(restTextTex, New Vector2(Panel2.Width / 2 - restTextTex.Width / 2, 800), Microsoft.Xna.Framework.Graphics.Color.White)
            Else
                mainSB.Draw(tradingIconTex, New Vector2(Panel2.Width - 80, +20), Microsoft.Xna.Framework.Graphics.Color.White)
            End If

            mainSB.Draw(timeRemainingTex, New Vector2(Panel2.Width - 200, 15), Microsoft.Xna.Framework.Graphics.Color.White)

            If Not showInstructions Then
                For i As Integer = 1 To numberOfPlayers
                    If i <> inumber Then playerlist(i).drawPlayerChat(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)
                Next
            End If

            'my player annimations
            playerlist(inumber).drawPlayer(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)

            'transfer animation
            For i As Integer = 1 To 1000
                If transfers(i) IsNot Nothing Then transfers(i).draw(Device, mainSB, preySmallTex, tempV)
            Next

            'my player chat
            playerlist(inumber).drawPlayerChat(Device, mainSB, texAvatarWalk, speechTexRight, speechTexLeft, tempV)

            'draw prey
            If smallPreyAvailable Then
                For i As Integer = 1 To smallPreyPerPeriod
                    smallPrey(i).draw(Device, mainSB, preySmallTex, tempV)
                Next
            End If

            For i As Integer = 1 To largePreyPerPeriod
                largePrey(i).draw(Device, mainSB, preyLargeTex, tempV)
            Next

            'explosions
            For i As Integer = 1 To 100
                If explosionList(i).enabled Then
                    mainSB.Draw(explosionTex,
                                New Rectangle(Math.Round(explosionList(i).myX - tempV.X),
                                              Math.Round(explosionList(i).myY - tempV.Y),
                                              explosionTex.Width * explosionList(i).size,
                                              explosionTex.Height * explosionList(i).size),
                                Nothing,
                                getMyColorXNA(explosionList(i).owner),
                                explosionList(i).rotation,
                                New Vector2((explosionTex.Width / 2), (explosionTex.Height / 2)),
                                SpriteEffects.None, 0)
                End If
            Next

            'earnings fountain
            For i As Integer = 1 To 100
                earningsFountainList(i).draw(mainSB, tempV)
            Next

            If pnlPot.Visible Then
                If pnlPot.Width = 130 Then
                    mainSB.Draw(potBorderTex, New Vector2(pnlPot.Location.X - 16, pnlPot.Location.Y - 4), Microsoft.Xna.Framework.Graphics.Color.White)
                Else
                    mainSB.Draw(potBorderTex,
                                New Rectangle(pnlPot.Location.X - 16, pnlPot.Location.Y - 4, potBorderTex.Width + 50, potBorderTex.Height),
                                Microsoft.Xna.Framework.Graphics.Color.White)
                End If
            End If

            mainSB.End()

            'Try
            If Not modMain.closing Then Device.Present()
            'Catch

            'End Try
        Catch ex As Exception
            recoverGraphics()
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub OnResetDevice(ByVal sender As Object, ByVal e As EventArgs)

        If Device Is Nothing Then
            'Start the 3d Device
            IntializeGraphics()
        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Timer1.Enabled = False
            gameLoop()
        Catch ex As Exception
            appEventLog_Write("error Timer1_Tick:", ex)
        End Try
    End Sub


    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try
            gameT2 += 1

            'move players
            For i As Integer = 1 To numberOfPlayers
                playerlist(i).movePlayer()
            Next

            If showInstructions Then
                playerlist(9).movePlayer()
                playerlist(10).movePlayer()
            End If

            'move prey
            For i As Integer = 1 To smallPreyPerPeriod
                smallPrey(i).move()
            Next

            For i As Integer = 1 To largePreyPerPeriod
                largePrey(i).move()
            Next

            For i As Integer = 1 To 100
                explosionList(i).move()
                earningsFountainList(i).move()
            Next

            If gameT2 Mod 20 = 0 Then drawHealth()
        Catch ex As Exception
            appEventLog_Write("error Timer2_Tick client:", ex)
        End Try
    End Sub

    Public Sub drawHealth()
        Try
            healthScreen.erase1()

            Dim g As Drawing.Graphics = healthScreen.GetGraphics

            Dim tempUnit As Double = Panel1.Height / 100

            'current health
            Dim rect As New Drawing.Rectangle(0, (100 - currentHealth) * tempUnit, Panel1.Width, currentHealth * tempUnit)
            g.FillRectangle(Brushes.Green, rect)

            'health fade
            If healthFadeTransparency > 0 Then
                If healthTransitionColor = Drawing.Color.Chartreuse Then
                    rect = New Drawing.Rectangle(0, (100 - currentHealth) * tempUnit, Panel1.Width, healthFadeAmount * tempUnit)
                    g.FillRectangle(New SolidBrush(Drawing.Color.FromArgb(healthFadeTransparency, healthTransitionColor)), rect)
                    healthFadeTransparency -= 20
                Else
                    rect = New Drawing.Rectangle(0, (100 - currentHealth) * tempUnit - healthFadeAmount * tempUnit + 1, Panel1.Width, healthFadeAmount * tempUnit)
                    g.FillRectangle(New SolidBrush(Drawing.Color.FromArgb(healthFadeTransparency, healthTransitionColor)), rect)
                    healthFadeTransparency -= 20
                End If
            End If

            'health image
            g.DrawImage(imgHealth, 3, CInt(Panel1.Height / 2 - imgHealth.Height / 2), imgHealth.Width, imgHealth.Height)

            Dim fmt As New StringFormat
            fmt.Alignment = StringAlignment.Center
            Dim f As New Font("Arial", 20, FontStyle.Bold)

            g.DrawString(Math.Round(currentHealth, 1), f, Brushes.Black, Panel1.Width / 2, Panel1.Height - 40, fmt)

            healthScreen.flip()

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not modMain.closing Then e.Cancel = True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub Panel2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseDown
        Try
            handlePanelClick(e.X, e.Y, e.Button)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub handlePanelClick(ByVal tempX As Integer, ByVal tempY As Integer, ByVal button As System.Windows.Forms.MouseButtons)
        Try
            Dim outstr As String = ""

            If playerlist(inumber) Is Nothing Then Exit Sub

            If playerlist(inumber).stunned > 0 Then Exit Sub
            If playerlist(inumber).tugging Then Exit Sub

            If showInstructions Then
                If playerlist(inumber).stunning > 0 And pnlPot.Visible Then Exit Sub
            End If

            If Not showInstructions Then
                pnlPot.Visible = False
                pnlHelp.Visible = False
            End If

            If placeFire Then

                If Not showInstructions Then
                    For i As Integer = 1 To numberOfPlayers
                        If i <> inumber Then
                            If returnDistance(viewPortX + tempX, viewPortY + tempY, playerlist(i).fireX, playerlist(i).fireY) < 200 Then
                                If Not testMode Then MessageBox.Show("You cannot place your Pot that close to another Pot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                placeFire = False
                                Cursor = Cursors.Default
                                cmdFire.Text = ""

                                Exit Sub
                            End If
                        End If
                    Next
                End If

                If viewPortX + tempX < 1680 * worldWidth * (2 / 6) + 50 Or viewPortX + tempX > 1680 * worldWidth * (4 / 6) - 50 Then
                    If Not testMode Then MessageBox.Show("You cannot place your pot in a Blue or Red Area.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    placeFire = False
                    Cursor = Cursors.Default
                    cmdFire.Text = ""

                    Exit Sub
                End If

                placeFire = False
                cmdFire.Enabled = False

                playerlist(inumber).fireX = viewPortX + tempX
                playerlist(inumber).fireY = viewPortY + tempY
                playerlist(inumber).fireMMX = playerlist(inumber).fireX
                playerlist(inumber).fireMMY = playerlist(inumber).fireY

                outstr = playerlist(inumber).fireX & ";"
                outstr &= playerlist(inumber).fireY & ";"

                Cursor = Cursors.Default
                cmdFire.Text = ""

                wskClient.Send("06", outstr)
            Else
                If Not leftSideOn Then
                    If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) Then
                        Exit Sub
                    End If
                End If

                If Not rightSideOn Then
                    If playerlist(inumber).myX > 1680 * worldWidth * (4 / 6) Then
                        Exit Sub
                    End If
                End If

                If button = Windows.Forms.MouseButtons.Left Then
                    txtMessages.Text = ""

                    Dim centerX As Integer = Panel2.Width / 2
                    Dim centerY As Integer = Panel2.Height / 2

                    playerlist(inumber).targetX = viewPortX + tempX
                    playerlist(inumber).targetY = viewPortY + tempY

                    If testMode Then
                        If playerlist(inumber).targetX < 200 Then playerlist(inumber).targetX = 200
                        If playerlist(inumber).targetX > worldWidth * 1680 - 200 Then playerlist(inumber).targetX = worldWidth * 1680 - 200

                        If playerlist(inumber).targetY < 200 Then playerlist(inumber).targetY = 200
                        If playerlist(inumber).targetY > worldHeight * 1050 - 200 Then playerlist(inumber).targetY = worldHeight * 1050 - 200
                    End If

                    playerlist(inumber).showChat = False

                    sendUpdateTarget()

                    If showInstructions Then
                        If currentInstruction = 6 And frmInstructions.pagesDone(6) = False Then
                            playerlist(9).targetX = playerlist(inumber).targetX + 200
                            playerlist(9).targetY = playerlist(inumber).targetY
                        End If
                    End If

                ElseIf button = Windows.Forms.MouseButtons.Right Then

                    If playerlist(inumber).coolDown > 0 Then Exit Sub

                    isOverFire = 0
                    isOverPlayer = 0

                    If playerlist(inumber).myX > 1680 * worldWidth * (2 / 6) And playerlist(inumber).myX < 1680 * worldWidth * (4 / 6) And phase <> "rest" Then
                        For i As Integer = 1 To numberOfPlayers
                            If i <> inumber Then
                                If returnDistance(playerlist(i).myX, playerlist(i).myY, playerlist(inumber).myX, playerlist(inumber).myY) < interactionRadius + 50 Then
                                    If playerlist(i).isOverPlayer(tempX + viewPortX, tempY + viewPortY) Then
                                        isOverPlayer = i
                                    End If
                                End If
                            End If
                        Next

                        If showInstructions Then
                            isOverPlayer = 0

                            If Not frmInstructions.pagesDone(currentInstruction) And
                               (currentInstruction = 6 Or currentInstruction = 7 Or currentInstruction = 8) Then

                                For i As Integer = 9 To 10
                                    If returnDistance(playerlist(i).myX, playerlist(i).myY, playerlist(inumber).myX, playerlist(inumber).myY) < interactionRadius + 50 Then
                                        If playerlist(i).isOverPlayer(tempX + viewPortX, tempY + viewPortY) Then
                                            isOverPlayer = i
                                        End If
                                    End If
                                Next

                                If currentInstruction = 6 Or currentInstruction = 7 Then
                                    If isOverPlayer = 10 Then isOverPlayer = 0
                                ElseIf currentInstruction = 8 Then
                                    If isOverPlayer = 9 Then isOverPlayer = 0
                                End If
                            End If
                        End If


                        If isOverPlayer = 0 Then
                            For i As Integer = 1 To numberOfPlayers
                                If returnDistance(playerlist(i).fireX, playerlist(i).fireY, playerlist(inumber).myX, playerlist(inumber).myY) < interactionRadius + 50 Then
                                    If playerlist(i).isOverPot(tempX + viewPortX, tempY + viewPortY) Then
                                        If showInstructions Then
                                            If i = inumber Then isOverFire = i
                                        Else
                                            isOverFire = i
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If (isOverFire > 0 Or isOverPlayer > 0) And (tempX < 210 And tempY < 180) Then
                            isOverFire = 0
                            isOverPlayer = 0
                            If Not testMode Then MessageBox.Show("Move Closer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If

                    If isOverPlayer > 0 Then

                        Dim pt As New System.Drawing.Point(playerlist(isOverPlayer).myX - viewPortX, playerlist(isOverPlayer).myY - viewPortY)

                        If (playerlist(isOverPlayer).stunning > 0 Or
                            playerlist(isOverPlayer).stunned > 0 Or
                            playerlist(isOverPlayer).tugHelping) And
                            Not showInstructions Then

                            isOverPlayer = 0
                            Exit Sub
                        ElseIf playerlist(isOverPlayer).tugging Then

                            pnlHelp.Location = pt
                            pnlHelp.Visible = True

                            ToolTip1.SetToolTip(cmdHelp, "Join " & playerlist(isOverPlayer).colorName & ".")

                            Exit Sub
                        Else

                            If Not showInstructions Then
                                If playerlist(inumber).unitsPlayer = 0 And playerlist(isOverPlayer).unitsPlayer = 0 Then Exit Sub
                            End If

                            sendStun(isOverPlayer)

                            pnlPot.Location = pt

                            If allowHit Then
                                pnlPot.Width = 180
                            Else
                                pnlPot.Width = 130
                            End If

                            rb1.Text = "Me -> " & playerlist(isOverPlayer).colorName
                            rb2.Text = playerlist(isOverPlayer).colorName & " -> Me"

                            If allowTake Then
                                rb2.Visible = True
                            Else
                                rb2.Visible = False
                            End If

                            rb1.Checked = True
                            nudPot.Maximum = playerlist(inumber).unitsPlayer
                            nudPot.Value = Math.Min(18, nudPot.Maximum)

                            pnlPot.Visible = True
                        End If

                    ElseIf isOverFire > 0 Then

                        Dim pt As New System.Drawing.Point(playerlist(isOverFire).fireX - viewPortX - 57, playerlist(isOverFire).fireY - viewPortY - 90)
                        pnlPot.Location = pt

                        pnlPot.Width = 130


                        rb1.Text = "Me -> Pot"
                        rb2.Text = "Pot -> Me"

                        If allowTake Then
                            rb2.Visible = True
                        Else
                            If isOverFire = inumber Then
                                rb2.Visible = True
                            Else
                                rb2.Visible = False
                            End If
                        End If

                        rb1.Checked = True
                        nudPot.Maximum = Math.Min(playerlist(inumber).unitsPlayer, hearthCapacity - playerlist(isOverFire).unitsPot)
                        nudPot.Value = Math.Min(18, nudPot.Maximum)

                        pnlPot.Visible = True
                    Else
                        If Not playerlist(inumber).pullingIn Then
                            For i As Integer = 1 To smallPreyPerPeriod
                                If smallPrey(i).isOver(tempX + viewPortX, tempY + viewPortY) Then
                                    smallPrey(i).status = "struck"
                                    playerlist(inumber).pullingIn = True
                                    playerlist(inumber).preyTarget = i
                                    Exit For
                                End If
                            Next

                            For i As Integer = 1 To largePreyPerPeriod
                                If largePrey(i).isOver(tempX + viewPortX, tempY + viewPortY) Then
                                    largePrey(i).status = "struck"
                                    playerlist(inumber).pullingIn = True
                                    playerlist(inumber).preyTarget = i
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim go As Boolean = True

            Do While go
                If modMain.closing = True Then go = False

                OnResetDevice(Device, Nothing)
                Render(Me)
            Loop
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdChat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChat.Click
        Try
            cmdChatAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdChatAction()
        Try
            If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) Or playerlist(inumber).myX > 1680 * worldWidth * (4 / 6) Then Exit Sub

            Dim outstr As String = ""

            If Not checkValidText(txtChat.Text) Then Exit Sub

            If txtChat.Text = "" Then Exit Sub

            pnlPot.Visible = False

            outstr = txtChat.Text & ";"

            txtChat.Text = ""

            If Not showInstructions Then cmdChat.Enabled = False
            wskClient.Send("04", outstr)

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub


    Private Sub txtChat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChat.TextChanged
        Try
            Me.AcceptButton = cmdChat
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Delegate Sub m_setChatTextBox(ByVal tLocation As System.Drawing.Point, ByVal tVisible As Boolean, ByVal index As Integer)

    Public Sub setChatTextBox(ByVal tLocation As System.Drawing.Point, ByVal tVisible As Boolean, ByVal index As Integer)
        Try
            If Me.InvokeRequired Then
                Dim obj(2) As Object
                obj(0) = tLocation
                obj(1) = tVisible
                obj(2) = index

                Me.BeginInvoke(New m_setChatTextBox(AddressOf setChatTextBox), obj)
            Else
                If chatBoxes(index) IsNot Nothing Then
                    chatBoxes(index).Location = tLocation
                    chatBoxes(index).Visible = tVisible
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub gameLoop()
        Try
            Dim go As Boolean = True

            Do While go
                If modMain.closing = True Then go = False

                OnResetDevice(Device, Nothing)
                Render(Me)
                Application.DoEvents()
            Loop

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try

    End Sub

    'Public Sub DisplayMessage(ByVal Message As String)
    '    'check if the call is coming in
    '    'on a thread other then the one this oject
    '    'was created in
    '    If Me.InvokeRequired Then
    '        'Call is from a different thread
    '        'create a delegate to call the method

    '        'Use an object array to store the parameters
    '        Dim obj(0) As Object
    '        obj(0) = Message
    '        'Use Begin Invoke to call this
    '        'method on this instance's thread
    '        Me.BeginInvoke(New m_delDisplayMessage(AddressOf DisplayMessage), obj)
    '    Else
    '        'Call is from the same thread,
    '        'just set the text
    '        Me.StatusBar.Panels(0).Text = Message
    '    End If
    'End Sub

    'The Delegate definition for our method
    'Private Delegate Sub m_delDisplayMessage(ByVal Message As String)


    Private Sub cmdFire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFire.Click
        Try
            cmdFireAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdFireAction()
        Try
            If placeFire Then
                placeFire = False
                Cursor = Cursors.Default
                cmdFire.Text = ""
            Else
                placeFire = True
                Cursor = Cursors.Cross
                cmdFire.Text = "CANCEL"
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        Try
            cmdSendAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdSendAction()
        Try
            If showInstructions Then
                If nudPot.Value = 0 Then Exit Sub

                If currentInstruction = 6 Then
                    If Not rb1.Checked Then Exit Sub

                    playerlist(inumber).stunning = 0
                    playerlist(9).stunned = 0

                    pnlPot.Visible = False

                    playerlist(inumber).unitsPlayer -= nudPot.Value
                    playerlist(9).unitsPlayer += nudPot.Value

                    playerlist(inumber).updatePlayerTex()
                    playerlist(9).updatePlayerTex()

                    transferCount += 1
                    transfers(transferCount) = New clsTransfer(playerlist(inumber).myX,
                                                               playerlist(inumber).myY,
                                                               playerlist(9).myX,
                                                               playerlist(9).myY)
                    If transferCount = 1000 Then transferCount = 0

                    frmInstructions.pagesDone(6) = True
                ElseIf currentInstruction = 7 Then
                    If Not rb2.Checked Then Exit Sub

                    pnlPot.Visible = False

                    playerlist(inumber).tugAmount = nudPot.Value
                    playerlist(9).tugAmount = nudPot.Value

                    playerlist(inumber).tugging = True
                    playerlist(9).tugging = True

                    playerlist(inumber).tugger = inumber
                    playerlist(9).tugger = inumber

                    playerlist(inumber).tugTarget = 9
                    playerlist(9).tugTarget = 9

                    playerlist(inumber).stunned = 0
                    playerlist(9).stunned = 0

                    playerlist(inumber).stunning = 0
                    playerlist(9).stunning = 0

                    playerlist(inumber).coolDown = 0
                    playerlist(9).coolDown = 0

                    playerlist(inumber).tugOpponet = 9
                    playerlist(9).tugOpponet = inumber

                    If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) + 500 Then
                        playerlist(inumber).targetX = 1680 * worldWidth * (2 / 6) + 500
                    End If

                    playerlist(9).targetX = playerlist(inumber).targetX - 400
                    playerlist(9).targetY = playerlist(inumber).targetY

                    updateStringTex(Device,
                                    playerlist(inumber).tugAmountTex,
                                    20,
                                    Drawing.Color.Black,
                                    playerlist(inumber).tugAmount)

                    pnlYield.Visible = True
                    pnlYield.Location = New System.Drawing.Point(Panel2.Width / 2, Panel2.Height / 2 - 150)

                    If inumber = inumber Then
                        ToolTip1.SetToolTip(cmdYield, "Yield to " & playerlist(9).colorName & ".")
                    Else
                        ToolTip1.SetToolTip(cmdYield, "Yield to " & playerlist(inumber).colorName & ".")
                    End If

                End If
            Else
                Dim outstr As String = ""

                pnlPot.Visible = False

                If nudPot.Text = "0" Then Exit Sub


                If rb1.Checked Then
                    outstr = rb1.Text & ";"
                Else
                    outstr = rb2.Text & ";"

                    If InStr(rb2.Text, " -> Me") And tugOfWar Then
                        If playerlist(inumber).currentHealth = 0 Then

                            addEarningsFountain("No Health",
                                               playerlist(inumber).myX + 50,
                                               playerlist(inumber).myY,
                                               playerlist(inumber).myX + 50,
                                               playerlist(inumber).myY - 50,
                                               20,
                                               Brushes.White,
                                               0.2,
                                               0.99,
                                               0.25,
                                               "none")

                            Exit Sub
                        End If
                    End If
                End If

                outstr &= nudPot.Text & ";"
                outstr &= isOverFire & ";"
                outstr &= isOverPlayer & ";"

                wskClient.Send("08", outstr)
            End If

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cboPot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub nudPot_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPot.ValueChanged
        Try
            nudPot.Validate()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Try
            If showInstructions Then
                doTestModeInstructions()
            Else
                doTestMode()
            End If


            Timer3.Interval = rand(1500, 500)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdStrikePlayer_Click(sender As System.Object, e As System.EventArgs) Handles cmdStrikePlayer.Click
        Try
            Dim outstr As String = ""

            pnlPot.Visible = False

            If isOverPlayer = 0 Then Exit Sub
            If currentHealth < hitCost Then Exit Sub

            outstr &= isOverPlayer & ";"

            wskClient.Send("12", outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub rb1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rb1.CheckedChanged
        Try
            If pnlPot.Visible Then

                If isOverPlayer > 0 Then
                    nudPot.Maximum = playerlist(inumber).unitsPlayer
                    nudPot.Value = Math.Min(18, nudPot.Maximum)
                Else
                    nudPot.Maximum = Math.Min(playerlist(inumber).unitsPlayer, hearthCapacity - playerlist(isOverFire).unitsPot)
                    nudPot.Value = Math.Min(18, nudPot.Maximum)
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub rb2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rb2.CheckedChanged
        Try
            If pnlPot.Visible Then

                If isOverPlayer > 0 Then
                    nudPot.Maximum = playerlist(isOverPlayer).unitsPlayer
                    nudPot.Value = Math.Min(18, nudPot.Maximum)
                Else
                    nudPot.Maximum = playerlist(isOverFire).unitsPot
                    nudPot.Value = Math.Min(18, nudPot.Maximum)
                End If

            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdYield_Click(sender As System.Object, e As System.EventArgs) Handles cmdYield.Click
        Try
            cmdYieldAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdYieldAction()
        Try
            If showInstructions Then
                If currentInstruction = 7 Then
                    frmInstructions.pagesDone(7) = True
                    playerlist(inumber).tugging = False
                    playerlist(9).tugging = False
                    pnlYield.Visible = False

                    addEarningsFountain(" ",
                                         playerlist(inumber).myX,
                                         playerlist(inumber).myY - 125,
                                         playerlist(inumber).myX,
                                         playerlist(inumber).myY - 175,
                                         20,
                                         Brushes.White,
                                         0.2,
                                         0.99,
                                         0.25,
                                         "yield")
                End If

            Else
                Dim outstr As String = ""

                pnlYield.Visible = False

                wskClient.Send("13", outstr)
            End If

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
        Try
            cmdHelpAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdHelpAction()
        Try
            If showInstructions Then
                If isOverPlayer <> 10 Then Exit Sub

                playerlist(inumber).tugHelping = True
                playerlist(inumber).tugHelpTarget = 10

                pnlHelp.Visible = False

            Else
                If playerlist(inumber).currentHealth = 0 Then
                    addEarningsFountain("No Health",
                                             playerlist(inumber).myX + 50,
                                             playerlist(inumber).myY,
                                             playerlist(inumber).myX + 50,
                                             playerlist(inumber).myY - 50,
                                             20,
                                             Brushes.White,
                                             0.2,
                                             0.99,
                                             0.25,
                                             "none")

                    Exit Sub
                End If


                Dim outstr As String = ""

                pnlHelp.Visible = False

                outstr = isOverPlayer & ";"

                wskClient.Send("14", outstr)
            End If


        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        Try
            cmdCancelAction()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub cmdCancelAction()
        Try
            Dim outstr As String = ""

            pnlHelp.Visible = False

            If showInstructions Then Exit Sub

            wskClient.Send("15", outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
