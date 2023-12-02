Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

'Programmed by Jeffrey Kirchner 
'kirchner@chapman.edu/jkirchner@gmail.com
'Economic Science Institute, Chapman University 2008-2013 ©

Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Module modMain
    Public sfile As String

    Public inumber As Integer                  'client ID number
    Public numberOfPlayers As Integer          'number of total players in experiment
    Public currentPeriod As Integer            'current period of experiment
    Public myIPAddress As String               'IP address of client 
    Public myPortNumber As String              'port number of client      
    Public exchangeRate As Integer             'client's exchange rate
    Public currentInstruction As Integer       'current instruction
    Public numberOfPeriods As Integer          'number of periods  
    Public showInstructions As Boolean         'wether to show instructions to subject

    Public WithEvents wskClient As Winsock
    Public closing As Boolean = False
    Public playerlist(30) As player

    Public gameT As Double = 0
    Public gameT2 As Double = 0

    Public bushCount As Integer
    Public bushLocations(100) As Vector2
    Public treeLocations(100) As Vector2
    Public rockLocations(100) As Vector2

    Public backgroundTileLocations(6, 6) As Vector2

    Public playerSpeed As Double
    Public largePreyPerPeriod As Integer
    Public smallPreyPerPeriod As Integer
    Public largePreyValue As Integer
    Public smallPreyValue As Integer
    Public preyMovementRate As Integer

    Public smallPrey(200) As prey
    Public largePrey(200) As prey

    Public huntingLength As Integer
    Public tradingLength As Integer

    Public phase As String
    Public placeFire As Boolean
    Public isOverFire As Integer
    Public isOverPlayer As Integer

    Public hearthCapacity As Integer
    Public stepSpeed As Integer
    Public largePreyProbability As Integer
    Public startingHealth As Integer
    Public currentHealth As Double
    Public healthLoss As Integer

    Public leftSideOn As Boolean             'enable left and right sides of screen
    Public rightSideOn As Boolean

    Public smallPanelX As Integer
    Public smallPanelY As Integer
    Public largePanelY As Integer

    Public interimLength As Integer
    Public testMode As Boolean

    Public restPeriodFrequency As Integer
    Public restPeriodLength As Integer

    'test mode
    Public upDownDirection As String = "up"
    Public leftRightDirection As String = "right"

    Public instructionX As Integer
    Public instructionY As Integer
    Public windowX As Integer
    Public windowY As Integer

    Public showHealth As Boolean

    Public interactionRadius As Integer
    Public interactionLength As Integer
    Public interactionCoolDown As Integer

    Public allowHit As Boolean
    Public hitDamage As Integer
    Public hitCost As Integer
    Public earningsMultiplier As Double

    Public explosionList(100) As explosion
    Public allowTake As Boolean

    Public tugOfWar As Boolean
    Public tugOfWarCost As Double
    Public smallPreyAvailable As Boolean

    Public earningsFountainList(100) As earningsFoutain
    Public potsAvailable As Boolean

    Public periodEarningsShown As Boolean

    Public playerScale As Double
    Public worldWidth As Integer
    Public worldHeight As Integer

    Public miniMapScale As Double = 0.075

    Public landMarks As Boolean


#Region " Winsock Code "
    Private Sub wskClient_DataArrival(ByVal sender As Object, ByVal e As WinsockDataArrivalEventArgs) Handles wskClient.DataArrival
        Try
            Dim buf As String = Nothing
            CType(sender, Winsock).Get(buf)

            Dim msgtokens() As String = buf.Split("#")
            Dim i As Integer

            'appEventLog_Write("data arrival: " & buf)

            For i = 1 To msgtokens.Length - 1
                takeMessage(msgtokens(i - 1))
            Next

        Catch ex As Exception
            appEventLog_Write("error wskClient_DataArrival:", ex)
        End Try
    End Sub

    Private Sub wskClient_ErrorReceived(ByVal sender As System.Object, ByVal e As WinsockErrorEventArgs) Handles wskClient.ErrorReceived
        ' Log("Error: " & e.Message)
    End Sub

    Private Sub wskClient_StateChanged(ByVal sender As Object, ByVal e As WinsockStateChangingEventArgs) Handles wskClient.StateChanged
        Try
            'appEventLog_Write("state changed")

            If e.New_State = WinsockStates.Closed Then
                frmConnect.Show()
            End If
        Catch ex As Exception
            appEventLog_Write("error wskClient_StateChanged:", ex)
        End Try

    End Sub

    Public Sub connect()
        Try

            wskClient = New Winsock
            wskClient.BufferSize = 8192
            wskClient.LegacySupport = False
            wskClient.LocalPort = 8080
            wskClient.MaxPendingConnections = 1
            wskClient.Protocol = WinsockProtocols.Tcp
            wskClient.RemotePort = myPortNumber
            wskClient.RemoteServer = myIPAddress
            wskClient.SynchronizingObject = frmMain

            wskClient.Connect()
        Catch
            frmMain.Hide()
            frmConnect.Show()
        End Try
    End Sub

#End Region

#Region " General Functions "
    Public Sub main()
        AppEventLog_Init()
        appEventLog_Write("Begin")

        ToggleScreenSaverActive(False)

        Application.EnableVisualStyles()
        Application.Run(frmMain)

        ToggleScreenSaverActive(True)

        appEventLog_Write("End")
        AppEventLog_Close()
    End Sub

    Public Function getMyColor(ByVal index As Integer) As System.Drawing.Color
        Try
            'appEventLog_Write("get color")

            Select Case index
                Case 1
                    getMyColor = System.Drawing.Color.Blue
                Case 2
                    getMyColor = System.Drawing.Color.Red
                Case 3
                    getMyColor = System.Drawing.Color.Yellow
                Case 4
                    getMyColor = System.Drawing.Color.Green
                Case 5
                    getMyColor = System.Drawing.Color.Purple
                Case 6
                    getMyColor = System.Drawing.Color.Orange
                Case 7
                    getMyColor = System.Drawing.Color.Brown
                Case 8
                    getMyColor = System.Drawing.Color.Teal
                Case 9
                    getMyColor = System.Drawing.Color.Gray
                Case 10
                    getMyColor = System.Drawing.Color.Khaki
            End Select
        Catch ex As Exception
            appEventLog_Write("error getMyColor:", ex)
        End Try
    End Function

    Public Function getMyColorXNA(ByVal index As Integer) As Color
        Try
            'appEventLog_Write("get color")

            Select Case index
                Case 1
                    getMyColorXNA = Color.Blue
                Case 2
                    getMyColorXNA = Color.Red
                Case 3
                    getMyColorXNA = Color.Yellow
                Case 4
                    getMyColorXNA = Color.Green
                Case 5
                    getMyColorXNA = Color.Purple
                Case 6
                    getMyColorXNA = Color.Orange
                Case 7
                    getMyColorXNA = Color.Brown
                Case 8
                    getMyColorXNA = Color.Teal
                Case 9
                    Return Color.Gray
                Case 10
                    Return Color.Khaki
            End Select
        Catch ex As Exception
            appEventLog_Write("error getMyColor:", ex)
        End Try
    End Function

    Public Function timeConversion(ByVal sec As Integer) As String
        Try
            'appEventLog_Write("time conversion :" & sec)
            timeConversion = Format((sec \ 60), "00") & ":" & Format((sec Mod 60), "00")
        Catch ex As Exception
            appEventLog_Write("error timeConversion:", ex)
            timeConversion = ""
        End Try
    End Function

    Public Sub setID(ByVal sinstr As String)
        Try
            'appEventLog_Write("set id :" & sinstr)

            Dim msgtokens() As String

            msgtokens = sinstr.Split(";")

            inumber = msgtokens(0)

            appEventLog_Write("Client# = " & inumber)

        Catch ex As Exception
            appEventLog_Write("error setID:", ex)
        End Try
    End Sub


    Public Sub sendIPAddress(ByVal sinstr As String)
        Try
            'appEventLog_Write("send ip :" & sinstr)

            With frmMain
                'Dim outstr As String

                inumber = sinstr

                appEventLog_Write("Client# = " & inumber)

                'outstr = SystemInformation.ComputerName
                '.wskClient.Send("03", outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error sendIPAddress:", ex)
        End Try
    End Sub

    Public Function numberSuffix(ByVal sinstr As Integer) As String
        numberSuffix = sinstr
        Try
            Select Case sinstr
                Case 1
                    numberSuffix = sinstr & "st"
                Case 2
                    numberSuffix = sinstr & "nd"
                Case 3
                    numberSuffix = sinstr & "rd"
                Case Is >= 4
                    numberSuffix = sinstr & "th"
            End Select
        Catch ex As Exception
            appEventLog_Write("error numberSuffix:", ex)
        End Try
    End Function
#End Region

    Private Sub takeMessage(ByVal sinstr As String)
        Try
            'take message from server
            'msgtokens(0) has type of message sent, having different types of messages allows you to send different formats for different actions.
            'msgtokens(1) has the semicolon delimited data that is to be parsed and acted upon.  


            Dim msgtokens() As String
            msgtokens = sinstr.Split("|")

            Select Case msgtokens(0) 'case statement to handle each of the different types of messages
                Case "01"
                    'close client
                    closing = True

                    'frmMain.BackgroundWorker1.CancelAsync()

                    wskClient.Close()
                    frmMain.Close()

                Case "02"
                    begin(msgtokens(1))
                Case "03"
                    setID(msgtokens(1))
                Case "04"
                    updateTarget(msgtokens(1))
                Case "05"
                    sendIPAddress(msgtokens(1))
                Case "06"
                    endGame(msgtokens(1))
                Case "07"
                    takeChat(msgtokens(1))
                Case "08"
                    updateTime(msgtokens(1))
                Case "09"
                    startNextPeriod(msgtokens(1))
                Case "10"
                    updateFire(msgtokens(1))
                Case "11"
                    startInterim(msgtokens(1))
                Case "12"
                    endEarly(msgtokens(1))
                Case "13"
                    finishedInstructions(msgtokens(1))
                Case "14"
                    startRest(msgtokens(1))
                Case "15"
                    unStick(msgtokens(1))
                Case "16"
                    takeTansfer(msgtokens(1))
                Case "17"
                    takeStun(msgtokens(1))
                Case "18"
                    takeHit(msgtokens(1))
                Case "19"
                    takeStartTug(msgtokens(1))
                Case "20"
                    takeYieldTug(msgtokens(1))
                Case "21"
                    takeTugHelp(msgtokens(1))
                Case "22"
                    takeCancelTugHelp(msgtokens(1))
            End Select
        Catch ex As Exception
            appEventLog_Write("error takeMessage:", ex)
        End Try

    End Sub

    Public Sub begin(ByVal sinstr As String)
        With frmMain
            Try
                'server has signaled client to start experiment

                smallPanelX = rand(5, 4)
                smallPanelY = 1 'rand(6, 1)
                largePanelY = rand(3, 1)

                'setup chat boxes
                .chatBoxes(1) = .txtChat1
                .chatBoxes(2) = .txtChat2
                .chatBoxes(3) = .txtChat3
                .chatBoxes(4) = .txtChat4
                .chatBoxes(5) = .txtChat5
                .chatBoxes(6) = .txtChat6
                .chatBoxes(7) = .txtChat7
                .chatBoxes(8) = .txtChat8

                'parse incoming data string
                Dim msgtokens() As String = sinstr.Split(";")
                Dim nextToken As Integer = 0

                numberOfPeriods = msgtokens(nextToken)
                nextToken += 1

                numberOfPlayers = msgtokens(nextToken)
                nextToken += 1

                showInstructions = msgtokens(nextToken)
                nextToken += 1

                playerSpeed = msgtokens(nextToken)
                nextToken += 1

                bushCount = msgtokens(nextToken)
                nextToken += 1

                largePreyPerPeriod = msgtokens(nextToken)
                nextToken += 1

                smallPreyPerPeriod = msgtokens(nextToken)
                nextToken += 1

                largePreyValue = msgtokens(nextToken)
                nextToken += 1

                smallPreyValue = msgtokens(nextToken)
                nextToken += 1

                preyMovementRate = msgtokens(nextToken)
                nextToken += 1

                huntingLength = msgtokens(nextToken)
                nextToken += 1

                tradingLength = msgtokens(nextToken)
                nextToken += 1

                hearthCapacity = msgtokens(nextToken)
                nextToken += 1

                stepSpeed = msgtokens(nextToken)
                nextToken += 1

                largePreyProbability = msgtokens(nextToken)
                nextToken += 1

                startingHealth = msgtokens(nextToken)
                currentHealth = startingHealth
                nextToken += 1

                healthLoss = msgtokens(nextToken)
                nextToken += 1

                interimLength = msgtokens(nextToken)
                nextToken += 1

                testMode = msgtokens(nextToken)
                nextToken += 1

                restPeriodFrequency = msgtokens(nextToken)
                nextToken += 1

                restPeriodLength = msgtokens(nextToken)
                nextToken += 1

                instructionX = msgtokens(nextToken)
                nextToken += 1

                instructionY = msgtokens(nextToken)
                nextToken += 1

                windowX = msgtokens(nextToken)
                nextToken += 1

                windowY = msgtokens(nextToken)
                nextToken += 1

                showHealth = msgtokens(nextToken)
                nextToken += 1

                interactionRadius = msgtokens(nextToken)
                nextToken += 1

                interactionLength = msgtokens(nextToken)
                nextToken += 1

                interactionCoolDown = msgtokens(nextToken)
                nextToken += 1

                allowHit = msgtokens(nextToken)
                nextToken += 1

                hitCost = msgtokens(nextToken)
                nextToken += 1

                hitDamage = msgtokens(nextToken)
                nextToken += 1

                earningsMultiplier = msgtokens(nextToken)
                nextToken += 1

                allowTake = msgtokens(nextToken)
                nextToken += 1

                tugOfWar = msgtokens(nextToken)
                nextToken += 1

                tugOfWarCost = msgtokens(nextToken)
                nextToken += 1

                smallPreyAvailable = msgtokens(nextToken)
                nextToken += 1

                potsAvailable = msgtokens(nextToken)
                nextToken += 1

                playerScale = msgtokens(nextToken)
                nextToken += 1

                worldWidth = msgtokens(nextToken)
                nextToken += 1

                worldHeight = msgtokens(nextToken)
                nextToken += 1

                landMarks = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i) = New player

                    playerlist(i).myX = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).myY = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).colorName = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).currentHealth = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).myGroup = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).updateHealthBar()

                    playerlist(i).targetX = playerlist(i).myX
                    playerlist(i).targetY = playerlist(i).myY

                    playerlist(i).myID = i
                    playerlist(i).showChat = False

                    playerlist(i).pullingIn = False
                    playerlist(i).intializePlayer(.Device)

                    playerlist(i).unitsPlayer = 0
                    playerlist(i).unitsPot = 0
                    playerlist(i).updatePlayerTex()

                    playerlist(i).fireMMX = -1
                    playerlist(i).fireMMY = -1
                    playerlist(i).fireX = -1
                    playerlist(i).fireY = -1

                    playerlist(i).stunned = 0
                    playerlist(i).stunning = 0

                    playerlist(i).tugging = False
                    playerlist(i).tugHelping = False
                Next

                For i As Integer = 1 To bushCount
                    bushLocations(i).X = msgtokens(nextToken)
                    nextToken += 1

                    bushLocations(i).Y = msgtokens(nextToken)
                    nextToken += 1

                    treeLocations(i).X = msgtokens(nextToken)
                    nextToken += 1

                    treeLocations(i).Y = msgtokens(nextToken)
                    nextToken += 1

                    rockLocations(i).X = msgtokens(nextToken)
                    nextToken += 1

                    rockLocations(i).Y = msgtokens(nextToken)
                    nextToken += 1
                Next

                'setup background tiles
                For i As Integer = 1 To worldWidth
                    For j As Integer = 1 To worldHeight
                        backgroundTileLocations(i, j) = New Vector2((i - 1) * .BackgroundTexLeft.Width, (j - 1) * .BackgroundTexLeft.Height)
                    Next
                Next

                'setup prey
                For i As Integer = 1 To 200
                    smallPrey(i) = New prey
                    smallPrey(i).myID = i
                    smallPrey(i).value = smallPreyValue
                    smallPrey(i).setup(.Device)
                    smallPrey(i).width = .preySmallTex.Width
                    smallPrey(i).status = "out"
                    smallPrey(i).myType = "small"
                Next

                For i As Integer = 1 To smallPreyPerPeriod
                    smallPrey(i).newLocation()
                Next

                For i As Integer = 1 To largePreyPerPeriod
                    largePrey(i) = New prey
                    largePrey(i).myType = "large"
                    largePrey(i).myID = i
                    largePrey(i).value = largePreyValue
                    largePrey(i).setup(.Device)
                    largePrey(i).width = .preyLargeTex.Width

                    largePrey(i).pullinResult = msgtokens(nextToken)
                    nextToken += 1
                Next

                .initializeMiniMap()

                .txtPeriod.Text = "1"

                phase = "hunting"

                .Timer1.Enabled = True
                .Timer2.Enabled = True
                placeFire = False

                .rb1.Checked = 0
                isOverFire = 0
                isOverPlayer = 0

                leftSideOn = True
                rightSideOn = True
                .initializeMiniMap()

                If testMode Then
                    .Timer3.Enabled = True
                    frmTestMode.Show()
                End If

                If showInstructions Then
                    frmInstructions.Show()
                    frmInstructions.Location = New System.Drawing.Point(instructionX, instructionY)

                    For i As Integer = 9 To 10
                        playerlist(i) = New player

                        playerlist(i).myX = 5000
                        nextToken += 1

                        playerlist(i).myY = 200
                        nextToken += 1

                        playerlist(i).myGroup = playerlist(inumber).myGroup

                        playerlist(i).colorName = getMyColorName(i)

                        playerlist(i).currentHealth = 50

                        playerlist(i).updateHealthBar()

                        playerlist(i).targetX = playerlist(i).myX
                        playerlist(i).targetY = playerlist(i).myY

                        playerlist(i).myID = i
                        playerlist(i).showChat = False

                        playerlist(i).pullingIn = False
                        playerlist(i).intializePlayer(.Device)

                        playerlist(i).unitsPlayer = 0
                        playerlist(i).unitsPot = 0
                        playerlist(i).updatePlayerTex()

                        playerlist(i).fireMMX = -1
                        playerlist(i).fireMMY = -1
                        playerlist(i).fireX = -1
                        playerlist(i).fireY = -1

                        playerlist(i).stunned = 0
                        playerlist(i).stunning = 0

                        playerlist(i).tugging = False
                        playerlist(i).tugHelping = False
                    Next
                End If

                .Location = New System.Drawing.Point(windowX, windowY)

                For i As Integer = 1 To 100
                    explosionList(i) = New explosion
                Next

                For i As Integer = 1 To 100
                    earningsFountainList(i) = New earningsFoutain("A", .Device, 16, Brushes.Black)
                    earningsFountainList(i).enabled = False
                Next

                If smallPreyAvailable Then
                    updateStringTex(.Device, .moveTextTex, 40, Drawing.Color.DimGray, "<< Choose a Side >>")
                Else
                    updateStringTex(.Device, .moveTextTex, 40, Drawing.Color.DimGray, "<< Gather Hexagons")
                End If

                If Not potsAvailable Then
                    .cmdFire.Visible = False
                End If

                periodEarningsShown = False
            Catch ex As Exception
                appEventLog_Write("error begin:", ex)
            End Try

        End With
    End Sub

    Public Sub endGame(ByVal sinstr As String)
        Try
            'end the experiment
            With frmMain
                Dim msgtokens() As String = sinstr.Split(";")
                Dim nextToken As Integer = 0

                updatePeriodEarnings(msgtokens(nextToken))
                nextToken += 1

                phase = "interim"

                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(interimLength))

                .Panel2.Enabled = False
                .gbChat.Enabled = False

                frmNames.Show()
                .Timer3.Enabled = False
            End With
        Catch ex As Exception
            appEventLog_Write("error endGame:", ex)
        End Try
    End Sub

    Public Sub endEarly(ByVal sinstr As String)
        Try
            'end experiment early
            Dim msgtokens() As String
            msgtokens = sinstr.Split(";")

            numberOfPeriods = msgtokens(0)
        Catch ex As Exception
            appEventLog_Write("error endEarly:", ex)
        End Try
    End Sub

    'Public Sub periodResults(ByVal sinstr As String)
    '    Try
    '        'show the results at end of period
    '        With frmMain

    '            Dim msgtokens() As String = Split(sinstr, ";")
    '            Dim nextToken As Integer = 0

    '        End With
    '    Catch ex As Exception
    '        appEventLog_Write("error periodResults:", ex)
    '    End Try
    'End Sub

    Public Sub updateTarget(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim tempP As Integer = msgtokens(nextToken)
                nextToken += 1

                If tempP <> inumber Then
                    playerlist(tempP).targetX = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(tempP).targetY = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(tempP).showChat = False
                Else
                    nextToken += 1
                    nextToken += 1
                End If

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).stunned = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).stunning = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).coolDown = msgtokens(nextToken)
                    nextToken += 1
                Next

                If playerlist(tempP).coolDown > 0 Then
                    updateStringTex(.Device, playerlist(tempP).stunnedTex, 12, Drawing.Color.Black, "Cooling..." & playerlist(tempP).coolDown)
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateFire(ByVal sinstr As String)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempP As Integer = msgtokens(nextToken)
            nextToken += 1

            If tempP <> inumber Then
                playerlist(tempP).fireX = msgtokens(nextToken)
                nextToken += 1

                playerlist(tempP).fireY = msgtokens(nextToken)
                nextToken += 1
            End If

            If tempP = inumber Then
                playerlist(inumber).fireMMX = playerlist(inumber).fireX
                playerlist(inumber).fireMMY = playerlist(inumber).fireY
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function colorToId(ByVal str As String) As Integer
        Try
            Dim i As Integer

            'appEventLog_Write("color to id :" & str)

            For i = 1 To numberOfPlayers
                If str = playerList(i).colorName Then
                    colorToId = i
                    Exit Function
                End If
            Next

            colorToId = -1
        Catch ex As Exception
            appEventLog_Write("error colorToId:", ex)
            Return 0
        End Try
    End Function

    Public Sub takeChat(ByVal sinstr As String)

        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim tempP As Integer = msgtokens(nextToken)
                nextToken += 1

                If playerlist(tempP).showChat Then
                    .chatBoxes(tempP).Text &= vbCrLf

                    .chatBoxes(tempP).Text &= ": " & msgtokens(nextToken)
                    nextToken += 1

                    .chatBoxes(tempP).SelectionStart = .chatBoxes(tempP).TextLength
                    .chatBoxes(tempP).ScrollToCaret()

                Else
                    playerlist(tempP).showChat = True
                    .chatBoxes(tempP).Text = ": " & msgtokens(nextToken)
                    nextToken += 1
                End If

                If Not showInstructions Then
                    For i As Integer = 1 To numberOfPlayers
                        playerlist(i).stunned = msgtokens(nextToken)
                        nextToken += 1

                        playerlist(i).stunning = msgtokens(nextToken)
                        nextToken += 1

                        playerlist(i).coolDown = msgtokens(nextToken)
                        nextToken += 1
                    Next

                    If playerlist(tempP).coolDown > 0 Then
                        updateStringTex(.Device, playerlist(tempP).stunnedTex, 12, Drawing.Color.Black, "Cooling..." & playerlist(tempP).coolDown)
                    End If
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function checkValidText(ByVal sinstr As String) As Boolean
        Try
            If InStr(sinstr, "|") > 0 Then
                MsgBox("Please do not use the ""|"" character.", MsgBoxStyle.Critical)
                sinstr = ""
                Return False
            End If

            If InStr(sinstr, "#") > 0 Then
                MsgBox("Please do not use the ""#"" character.", MsgBoxStyle.Critical)
                sinstr = ""
                Return False
            End If

            If InStr(sinstr, ";") > 0 Then
                MsgBox("Please do not use the "";"" character.", MsgBoxStyle.Critical)
                sinstr = ""
                Return False
            End If

            Return True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return False
        End Try
    End Function

    Public Sub updateTime(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(CInt(msgtokens(nextToken))))
                nextToken += 1

                .txtPeriod.Text = msgtokens(nextToken)
                nextToken += 1

                If Not showInstructions Then phase = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    Dim tempUnitsPlayer As Integer = msgtokens(nextToken)
                    nextToken += 1

                    Dim tempUnitsPot As Integer = msgtokens(nextToken)
                    nextToken += 1

                    Dim tempHealth As Double = msgtokens(nextToken)
                    nextToken += 1

                    If Not showInstructions Then playerlist(i).stunned = msgtokens(nextToken)
                    nextToken += 1

                    If Not showInstructions Then playerlist(i).stunning = msgtokens(nextToken)
                    nextToken += 1

                    If Not showInstructions Then playerlist(i).coolDown = msgtokens(nextToken)
                    nextToken += 1

                    If Not showInstructions Then playerlist(i).tugHelping = msgtokens(nextToken)
                    nextToken += 1

                    If tempUnitsPlayer <> playerlist(i).unitsPlayer Then
                        If Not showInstructions Then playerlist(i).unitsPlayer = tempUnitsPlayer
                        playerlist(i).updatePlayerTex()
                    End If

                    If tempUnitsPot <> playerlist(i).unitsPot Then
                        playerlist(i).unitsPot = tempUnitsPot
                        playerlist(i).updatePlayerTex()
                    End If

                    If tempHealth <> playerlist(i).currentHealth Then
                        playerlist(i).currentHealth = tempHealth
                        playerlist(i).updateHealthBar()
                    End If

                    If playerlist(i).stunned > 0 Then
                        updateStringTex(.Device, playerlist(i).stunnedTex, 12, Drawing.Color.Black, "zzz..." & playerlist(i).stunned)
                    ElseIf playerlist(i).coolDown > 0 Then
                        updateStringTex(.Device, playerlist(i).stunnedTex, 12, Drawing.Color.Black, "Cooling..." & playerlist(i).coolDown)
                    End If

                    If playerlist(i).tugging And playerlist(i).myGroup = playerlist(inumber).myGroup Then

                        If playerlist(i).currentHealth > 0 And playerlist(playerlist(i).tugOpponet).currentHealth > 0 Then

                            Dim tempTugAmount As Integer = 1

                            For j As Integer = 1 To numberOfPlayers
                                If playerlist(j).tugHelping And playerlist(j).tugHelpTarget = playerlist(i).tugOpponet Then
                                    tempTugAmount += 1
                                End If
                            Next

                            'tug helpers
                            For j As Integer = 1 To numberOfPlayers
                                If playerlist(j).tugHelping And
                                   playerlist(j).tugHelpTarget = i Then

                                    addEarningsFountain("-" & Math.Round(tempTugAmount * tugOfWarCost, 1),
                                                playerlist(j).myX + 50,
                                                playerlist(j).myY,
                                                playerlist(j).myX + 50,
                                                playerlist(j).myY - 50,
                                                20,
                                                Brushes.Red,
                                                0.2,
                                                0.99,
                                                0.25,
                                                "health")
                                End If
                            Next

                            addEarningsFountain("-" & Math.Round(tempTugAmount * tugOfWarCost, 1),
                                                playerlist(i).myX + 50,
                                                playerlist(i).myY,
                                                playerlist(i).myX + 50,
                                                playerlist(i).myY - 50,
                                                20,
                                                Brushes.Red,
                                                0.2,
                                                0.99,
                                                0.25,
                                                "health")

                            If playerlist(i).currentHealth < 0 Then playerlist(i).currentHealth = 0
                        End If
                    End If
                Next

                If showInstructions Then
                    Dim tempTugAmountInstructions As Double

                    If currentInstruction = 8 And playerlist(inumber).tugHelping And frmInstructions.page8counter = 5 Then

                        playerlist(10).unitsPlayer += 1
                        playerlist(9).unitsPlayer -= 1

                        playerlist(10).updatePlayerTex()
                        playerlist(9).updatePlayerTex()

                        playerlist(10).tugging = False
                        playerlist(9).tugging = False

                        playerlist(inumber).tugHelping = False

                        frmInstructions.pagesDone(8) = True

                        addEarningsFountain("+" & "1" & " units.",
                                            playerlist(10).myX,
                                            playerlist(10).myY - 90,
                                            playerlist(10).myX,
                                            playerlist(10).myY - 140,
                                            20,
                                            Brushes.White,
                                            0.2,
                                            0.99,
                                            0.25,
                                            "units")

                        addEarningsFountain("-" & "1" & " units.",
                                            playerlist(9).myX,
                                            playerlist(9).myY - 90,
                                            playerlist(9).myX,
                                            playerlist(9).myY - 140,
                                            20,
                                            Brushes.White,
                                            0.2,
                                            0.99,
                                            0.25,
                                            "units")

                        addEarningsFountain(" ",
                                            playerlist(9).myX,
                                            playerlist(9).myY - 125,
                                            playerlist(9).myX,
                                            playerlist(9).myY - 175,
                                            20,
                                            Brushes.White,
                                            0.2,
                                            0.99,
                                            0.25,
                                            "yield")
                    Else
                        If currentInstruction = 8 And playerlist(inumber).tugHelping Then
                            frmInstructions.page8counter += 1
                            Dim a As Integer = frmInstructions.page8counter
                        End If

                        For i As Integer = 9 To 10
                            If currentInstruction = 7 Then
                                tempTugAmountInstructions = 1
                            ElseIf currentInstruction = 8 Then
                                If playerlist(inumber).tugHelping And playerlist(inumber).tugHelpTarget <> i Then
                                    tempTugAmountInstructions = 2
                                ElseIf playerlist(inumber).tugHelping Then
                                    tempTugAmountInstructions = 1
                                Else
                                    tempTugAmountInstructions = 1
                                End If
                            End If

                            If playerlist(i).tugging And playerlist(i).myGroup = playerlist(inumber).myGroup Then
                                'tug helpers
                                For j As Integer = 1 To numberOfPlayers
                                    If playerlist(j).tugHelping And playerlist(j).tugHelpTarget = i Then

                                        addEarningsFountain("-" & Math.Round(tempTugAmountInstructions * tugOfWarCost, 1),
                                                    playerlist(j).myX + 50,
                                                    playerlist(j).myY,
                                                    playerlist(j).myX + 50,
                                                    playerlist(j).myY - 50,
                                                    20,
                                                    Brushes.Red,
                                                    0.2,
                                                    0.99,
                                                    0.25,
                                                    "health")
                                    End If
                                Next

                                addEarningsFountain("-" & Math.Round(tempTugAmountInstructions * tugOfWarCost, 1),
                                                    playerlist(i).myX + 50,
                                                    playerlist(i).myY,
                                                    playerlist(i).myX + 50,
                                                    playerlist(i).myY - 50,
                                                    20,
                                                    Brushes.Red,
                                                    0.2,
                                                    0.99,
                                                    0.25,
                                                    "health")
                            End If
                        Next

                    End If


                End If

                Dim tempYieldCount As Integer = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To tempYieldCount
                    takeYieldTug(msgtokens(nextToken))
                    nextToken += 1
                Next

                currentHealth = playerlist(inumber).currentHealth

                'check stunning
                If .pnlPot.Visible And playerlist(inumber).stunning = 0 And isOverPlayer > 0 Then
                    .pnlPot.Visible = False
                    isOverPlayer = 0
                End If

                If phase = "trading" Then
                    Dim doUpdate As Boolean = False
                    If leftSideOn = True Or rightSideOn = True Then
                        doUpdate = True
                    End If

                    leftSideOn = False
                    rightSideOn = False

                    If doUpdate Then
                        For i As Integer = 1 To 200
                            smallPrey(i).status = "out"
                        Next

                        For i As Integer = 1 To largePreyPerPeriod
                            largePrey(i).status = "out"
                        Next

                        playerlist(inumber).pullingIn = False

                        .initializeMiniMap()

                        If Not leftSideOn Then
                            If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) + 50 Then
                                playerlist(inumber).targetX = 1680 * worldWidth * (2 / 6) + 200
                                'playerlist(inumber).targetY = 1050 * 3

                                sendUpdateTarget()

                                .txtMessages.Text = "Auto Return."
                            End If
                        End If

                        If Not rightSideOn Then
                            If playerlist(inumber).myX > 1680 * worldWidth * (4 / 6) - 50 Then
                                playerlist(inumber).targetX = 1680 * worldWidth * (4 / 6) - 200
                                'playerlist(inumber).targetY = 1050 * 3

                                sendUpdateTarget()

                                .txtMessages.Text = "Auto Return."
                            End If
                        End If

                    End If
                End If

                .cmdChat.Enabled = True

                sendUpdate()
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendUpdate()
        Try
            Dim outstr As String = ""

            outstr = playerlist(inumber).myX & ";"
            outstr &= playerlist(inumber).myY & ";"
            outstr &= playerlist(inumber).pullingIn & ";"
            outstr &= playerlist(inumber).preyTarget & ";"

            outstr &= smallPreyPerPeriod & ";"

            For i As Integer = 1 To smallPreyPerPeriod
                outstr &= smallPrey(i).myX & ";"
                outstr &= smallPrey(i).myY & ";"
                outstr &= smallPrey(i).status & ";"
            Next

            For i As Integer = 1 To largePreyPerPeriod
                outstr &= largePrey(i).myX & ";"
                outstr &= largePrey(i).myY & ";"
                outstr &= largePrey(i).status & ";"
            Next

            wskClient.Send("09", outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub startNextPeriod(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                .txtPeriod.Text = msgtokens(nextToken)
                nextToken += 1

                smallPreyPerPeriod = msgtokens(nextToken)
                nextToken += 1

                currentHealth = msgtokens(nextToken)
                playerlist(inumber).updateHealthBar()
                nextToken += 1

                phase = "hunting"

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).myGroup = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).unitsPlayer = 0
                    playerlist(i).unitsPot = 0
                    playerlist(i).updatePlayerTex()
                    playerlist(i).pullingIn = False
                Next

                smallPanelX = rand(5, 4)
                smallPanelY = rand(6, 1)
                largePanelY = rand(3, 1)

                'setup prey
                For i As Integer = 1 To smallPreyPerPeriod
                    smallPrey(i).newLocation()
                Next

                For i As Integer = smallPreyPerPeriod + 1 To 200
                    smallPrey(i).status = "out"
                Next

                For i As Integer = 1 To largePreyPerPeriod
                    largePrey(i).newLocation()

                    largePrey(i).pullinResult = msgtokens(nextToken)
                    nextToken += 1
                Next

                leftSideOn = True
                rightSideOn = True

                .initializeMiniMap()

                .healthFadeAmount = healthLoss
                .healthFadeTransparency = 250
                .healthTransitionColor = Drawing.Color.Red

                .cmdFire.Enabled = True

                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(huntingLength))

                periodEarningsShown = False
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function returnDistance(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Integer
        Try
            Return Math.Sqrt((x2 - x1) ^ 2 + (y2 - y1) ^ 2)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Sub sendUpdateTarget()
        Try
            Dim outstr As String

            outstr = playerlist(inumber).targetX & ";"
            outstr &= playerlist(inumber).targetY & ";"

            wskClient.Send("01", outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub startInterim(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = sinstr.Split(";")
                Dim nextToken As Integer = 0

                updatePeriodEarnings(msgtokens(nextToken))
                nextToken += 1

                .pnlYield.Visible = False
                .pnlHelp.Visible = False

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).tugging = False
                    playerlist(i).tugHelping = False
                Next

                phase = "interim"
                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(interimLength))
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updatePeriodEarnings(ByVal sinstr As String)
        Try
            With frmMain

                If Not periodEarningsShown Then
                    Dim msgtokens() As String = Split(sinstr, ",")
                    Dim nextToken As Integer = 0

                    Dim tempUnitsAdded As Integer = msgtokens(nextToken)
                    nextToken += 1

                    Dim tempUnitsWasted As Integer = msgtokens(nextToken)
                    nextToken += 1

                    currentHealth = msgtokens(nextToken)
                    playerlist(inumber).updateHealthBar()
                    nextToken += 1

                    .txtProfit.Text = Format(CDbl(msgtokens(nextToken)) / 100, "0.00")
                    nextToken += 1


                    periodEarningsShown = True

                    .healthFadeAmount = tempUnitsAdded
                    .healthFadeTransparency = 250
                    .healthTransitionColor = Drawing.Color.Chartreuse


                    addEarningsFountain("+" & tempUnitsAdded & " Health",
                                                  playerlist(inumber).myX + 50,
                                                  playerlist(inumber).myY,
                                                  playerlist(inumber).myX + 50,
                                                  playerlist(inumber).myY - 50,
                                                  20,
                                                  Brushes.White,
                                                  0.1,
                                                  0.99,
                                                  0.25,
                                                  "none")

                    If tempUnitsWasted > 0 Then
                        addEarningsFountain(tempUnitsWasted & " Units Wasted",
                                              playerlist(inumber).myX + 50,
                                              playerlist(inumber).myY + 25,
                                              playerlist(inumber).myX + 50,
                                              playerlist(inumber).myY - 25,
                                              20,
                                              Brushes.Red,
                                              0.1,
                                              0.99,
                                              0.25,
                                              "none")

                    End If

                    playerlist(inumber).unitsPlayer = 0
                    playerlist(inumber).unitsPot = 0

                    playerlist(inumber).updatePlayerTex()
                    playerlist(inumber).updateHealthBar()
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub startRest(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                updatePeriodEarnings(msgtokens(nextToken))
                nextToken += 1

                .pnlYield.Visible = False
                .pnlHelp.Visible = False

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).tugging = False
                    playerlist(i).tugHelping = False
                Next

                phase = "rest"
                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(restPeriodLength))
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateStringTex(ByVal d As GraphicsDevice, _
                                ByRef tempTex As Texture2D, _
                                ByVal fontSize As Integer, _
                                ByVal fontColor As System.Drawing.Color, _
                                ByVal stringValue As String)
        Try
            Dim NewFont As Font
            Dim g As System.Drawing.Graphics
            Dim StringSize As SizeF
            Dim Bitmap As Bitmap
            Dim sf As New StringFormat
            Dim MemStream As System.IO.MemoryStream

            'font text setup
            'font/text setup
            NewFont = New Font("Microsoft Sans Serif", fontSize, FontStyle.Bold) ' The font that the text will be written in
            g = System.Drawing.Graphics.FromHwnd(frmMain.Panel2.Handle) ' The Graphics object that will 
            sf.Alignment = StringAlignment.Center
            StringSize = g.MeasureString(stringValue, NewFont) 'The length of the text

            'write the text onto the bitmap
            Bitmap = New Bitmap(CInt(StringSize.Width), CInt(StringSize.Height)) ' the bitmap that will hold the text
            g = System.Drawing.Graphics.FromImage(Bitmap) 'tell the graphics object that it will be using the bitmap
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit ' and how it will display the text
            g.DrawString(stringValue, NewFont, New SolidBrush(fontColor), 0, 0) ' Draw the string on the bitmap

            MemStream = New System.IO.MemoryStream 'set aside a portion of memory to hold the bitmap
            Bitmap.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png) ' save the bitmap to the portion of memory
            MemStream.Position = 0 ' dont know what this does, but it is necessary

            tempTex = Texture2D.FromFile(d, MemStream)

        Catch ex As Exception
            frmMain.recoverGraphics()
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub doTestMode()
        Try
            With frmMain
                If phase = "trading" Then

                    If playerlist(inumber).targetX = playerlist(inumber).myX And playerlist(inumber).targetY = playerlist(inumber).myY Then
                        Dim tempN As Integer = rand(100, 1)

                        If .pnlPot.Visible Then
                            .rb1.Checked = True

                            If .rb2.Visible Then
                                If rand(2, 1) = 1 Then
                                    .rb2.Checked = True
                                End If
                            End If

                            .nudPot.Value = rand(.nudPot.Maximum, .nudPot.Minimum)
                            .cmdSendAction()

                            Exit Sub
                        ElseIf .pnlHelp.Visible Then
                            If rand(2, 1) = 1 Then
                                .cmdHelpAction()
                            Else
                                .cmdCancelAction()
                            End If
                        ElseIf .pnlYield.Visible Then
                            If rand(10, 1) Then
                                .cmdYieldAction()
                            End If
                        End If

                        If tempN <= 10 Then 'place fire
                            If .cmdFire.Enabled And potsAvailable Then
                                .cmdFireAction()
                                .handlePanelClick(rand(.Panel2.Width, 1), rand(.Panel2.Height, 1), MouseButtons.Left)
                            End If
                        ElseIf tempN <= 40 Then 'new location
                            testModeNewDestination()
                        ElseIf tempN <= 50 Then 'chat
                            Dim outstr As String = ""
                            For i As Integer = 1 To rand(20, 5)
                                outstr &= Chr(rand(123, 60))
                            Next
                            .txtChat.Text = outstr
                            .cmdChatAction()
                        Else 'transfer units
                            If playerlist(inumber).unitsPlayer > 0 Then

                                If rand(100, 1) > 50 Then
                                    'transfer to players
                                    For i As Integer = 1 To numberOfPlayers
                                        If i <> inumber Then
                                            If returnDistance(playerlist(i).myX, playerlist(i).myY, playerlist(inumber).myX, playerlist(inumber).myY) <= 700 Then

                                                .handlePanelClick(playerlist(i).myX - .viewPortX, playerlist(i).myY - .viewPortY, MouseButtons.Right)
                                                Exit Sub
                                                'End If
                                            End If
                                        End If
                                    Next
                                ElseIf potsAvailable Then
                                    'transfer to pots
                                    For i As Integer = 1 To numberOfPlayers
                                        If returnDistance(playerlist(i).fireX, playerlist(i).fireY, playerlist(inumber).myX, playerlist(inumber).myY) <= 700 Then

                                            .handlePanelClick(playerlist(i).fireX - .viewPortX, playerlist(i).fireY - .viewPortY, MouseButtons.Right)

                                            Exit For
                                            'End If
                                        End If
                                    Next

                                End If
                            End If
                        End If
                    End If

                ElseIf phase = "hunting" Then

                    If Not playerlist(inumber).pullingIn Then
                        If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) Then 'left side
                            For i As Integer = 1 To largePreyPerPeriod
                                If playerlist(inumber).pullingIn Then Exit For

                                If returnDistance(largePrey(i).myX, largePrey(i).myY, playerlist(inumber).myX, playerlist(inumber).myY) <= 700 Then
                                    .handlePanelClick(largePrey(i).myX - .viewPortX, largePrey(i).myY - .viewPortY, MouseButtons.Right)
                                End If
                            Next
                        End If

                        If smallPreyAvailable Then
                            If playerlist(inumber).myX > 1680 * worldWidth * (4 / 6) Then 'right side
                                For i As Integer = 1 To smallPreyPerPeriod
                                    If playerlist(inumber).pullingIn Then Exit For

                                    If returnDistance(smallPrey(i).myX, smallPrey(i).myY, playerlist(inumber).myX, playerlist(inumber).myY) <= 700 Then
                                        .handlePanelClick(smallPrey(i).myX - .viewPortX, smallPrey(i).myY - .viewPortY, MouseButtons.Right)
                                    End If
                                Next
                            End If
                        Else
                            Dim tempTarget As Integer = 0

                            For i As Integer = 1 To largePreyPerPeriod
                                If largePrey(i).status = "free" Then
                                    tempTarget = i
                                    Exit For
                                End If
                            Next

                            If tempTarget <> 0 Then
                                Dim tempX As Integer = .Panel2.Width / 2
                                If playerlist(inumber).myX < largePrey(tempTarget).myX Then
                                    tempX = rand(.Panel2.Width, .Panel2.Width / 2)
                                ElseIf playerlist(inumber).myX > largePrey(tempTarget).myX Then
                                    tempX = rand(.Panel2.Width / 2, 0)
                                End If

                                Dim tempY As Integer = .Panel2.Height / 2
                                If playerlist(inumber).myY < largePrey(tempTarget).myY Then
                                    tempY = rand(.Panel2.Height, .Panel2.Height / 2)
                                ElseIf playerlist(inumber).myY > largePrey(tempTarget).myY Then
                                    tempY = rand(.Panel2.Height / 2, 0)
                                End If

                                .handlePanelClick(tempX, tempY, MouseButtons.Left)
                            End If
                        End If

                        '  testModeNewDestination()
                    Else
                            playerlist(inumber).targetX = playerlist(inumber).myX
                            playerlist(inumber).targetY = playerlist(inumber).myY
                    End If

                ElseIf phase = "rest" Then
                    If playerlist(inumber).targetX = playerlist(inumber).myX And playerlist(inumber).targetY = playerlist(inumber).myY Then
                        Dim tempN As Integer = rand(100, 1)

                        If tempN <= 50 Then 'new destination
                            testModeNewDestination()
                        Else
                            Dim outstr As String = ""
                            For i As Integer = 1 To rand(20, 5)
                                outstr &= Chr(rand(123, 60))
                            Next
                            .txtChat.Text = outstr
                            .cmdChatAction()
                        End If
                    End If
                Else
                    'choose a side
                    If playerlist(inumber).targetX > 1680 * worldWidth * (2 / 6) And playerlist(inumber).targetX < 1680 * worldWidth * (4 / 6) Then
                        testModeNewDestination()
                    End If
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub doTestModeInstructions()
        Try
            With frmMain
                Select Case currentInstruction
                    Case 1
                        frmInstructions.cmdNextAction()
                    Case 2
                        frmInstructions.cmdNextAction()
                    Case 3
                        If playerlist(inumber).myX < 3000 Then
                            frmInstructions.cmdNextAction()
                        Else
                            .handlePanelClick(rand(.Panel2.Width / 2, 0), rand(.Panel2.Height, 0), MouseButtons.Left)
                        End If
                    Case 4
                        If playerlist(inumber).unitsPlayer > 0 Then
                            frmInstructions.cmdNextAction()
                        ElseIf playerlist(inumber).pullingIn Then
                            'wait
                        ElseIf returnDistance(playerlist(inumber).myX, playerlist(inumber).myY, largePrey(1).myX, largePrey(1).myY) <= interactionRadius Then
                            .handlePanelClick(largePrey(1).myX - .viewPortX, largePrey(1).myY - .viewPortY, MouseButtons.Right)
                        ElseIf playerlist(inumber).myX = playerlist(inumber).targetX And playerlist(inumber).myY = playerlist(inumber).targetY Then
                            Dim tempX As Integer = .Panel2.Width / 2
                            If playerlist(inumber).myX < largePrey(1).myX Then
                                tempX = rand(.Panel2.Width, .Panel2.Width / 2)
                            ElseIf playerlist(inumber).myX > largePrey(1).myX Then
                                tempX = rand(.Panel2.Width / 2, 0)
                            End If

                            Dim tempY As Integer = .Panel2.Height / 2
                            If playerlist(inumber).myY < largePrey(1).myY Then
                                tempY = rand(.Panel2.Height, .Panel2.Height / 2)
                            ElseIf playerlist(inumber).myY > largePrey(1).myY Then
                                tempY = rand(.Panel2.Height / 2, 0)
                            End If

                            .handlePanelClick(tempX, tempY, MouseButtons.Left)
                        End If
                    Case 5
                        frmInstructions.cmdNextAction()
                    Case 6
                        If playerlist(9).unitsPlayer > 0 Then
                            frmInstructions.cmdNextAction()
                        ElseIf .pnlPot.Visible Then
                            .cmdSendAction()
                        ElseIf returnDistance(playerlist(inumber).myX, playerlist(inumber).myY, playerlist(9).myX, playerlist(9).myY) <= interactionRadius Then
                            .handlePanelClick(playerlist(9).myX - .viewPortX, playerlist(9).myY - .viewPortY, MouseButtons.Right)
                        End If
                    Case 7
                        If .pnlYield.Visible Then
                            .cmdYieldAction()
                            frmInstructions.cmdNextAction()
                        ElseIf .pnlPot.Visible Then
                            If Not .rb2.Checked Then
                                .rb2.Checked = True
                            Else
                                .cmdSendAction()
                            End If
                        Else
                            .handlePanelClick(playerlist(9).myX - .viewPortX, playerlist(9).myY - .viewPortY, MouseButtons.Right)
                        End If
                    Case 8

                        If playerlist(10).unitsPlayer > 0 Then
                            frmInstructions.cmdNextAction()
                        ElseIf .pnlHelp.Visible Then
                            .cmdHelpAction()
                        ElseIf playerlist(inumber).tugHelping Then
                            'wait
                        ElseIf returnDistance(playerlist(inumber).myX, playerlist(inumber).myY, playerlist(10).myX, playerlist(10).myY) <= interactionRadius Then
                            .handlePanelClick(playerlist(10).myX - .viewPortX, playerlist(10).myY - .viewPortY, MouseButtons.Right)

                        ElseIf playerlist(inumber).myX = playerlist(inumber).targetX And playerlist(inumber).myY = playerlist(inumber).targetY Then
                            Dim tempX As Integer = .Panel2.Width / 2
                            If playerlist(inumber).myX < playerlist(10).myX Then
                                tempX = rand(.Panel2.Width, .Panel2.Width / 2)
                            ElseIf playerlist(inumber).myX > playerlist(10).myX Then
                                tempX = rand(.Panel2.Width / 2, 0)
                            End If

                            Dim tempY As Integer = .Panel2.Height / 2
                            If playerlist(inumber).myY < playerlist(10).myY Then
                                tempY = rand(.Panel2.Height, .Panel2.Height / 2)
                            ElseIf playerlist(inumber).myY > playerlist(10).myY Then
                                tempY = rand(.Panel2.Height / 2, 0)
                            End If

                            .handlePanelClick(tempX, tempY, MouseButtons.Left)
                        End If

                    Case 9
                        frmInstructions.cmdNextAction()
                    Case 10
                        frmInstructions.cmdNextAction()
                    Case 11
                        If frmInstructions.cmdStart.Visible Then frmInstructions.cmdStartAction()
                End Select

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub testModeNewDestination()
        Try
            With frmMain
                If (playerlist(inumber).myX = playerlist(inumber).targetX And playerlist(inumber).myY = playerlist(inumber).targetY) Or _
                            playerlist(inumber).myY < 200 Or playerlist(inumber).myX < 200 _
                                Or playerlist(inumber).myY > 6100 Or playerlist(inumber).myX >= 10000 Then

                    'ElseIf playerlist(inumber).myY < 200 Or playerlist(inumber).myX < 200 _
                    '    Or playerlist(inumber).myY > 6100 Or playerlist(inumber).myX >= 10000 Then
                    '    .handlePanelClick(rand(2000, -2000), rand(2000, -2000), MouseButtons.Left)

                    If playerlist(inumber).myY < 250 Then upDownDirection = "down"
                    If playerlist(inumber).myY > worldHeight * 1050 - 250 Then upDownDirection = "up"
                    If playerlist(inumber).myX < 250 Then leftRightDirection = "right"

                    If smallPreyAvailable Then
                        If playerlist(inumber).myX > worldWidth * 1680 - 200 Then leftRightDirection = "left"
                    Else
                        If playerlist(inumber).myX > worldWidth * 1680 - 200 Then leftRightDirection = "left"
                    End If

                    If rightSideOn = False And playerlist(inumber).myX > 6600 Then leftRightDirection = "left"
                    If leftSideOn = False And playerlist(inumber).myX < 3400 Then leftRightDirection = "right"

                    Dim tempXMin As Integer
                    Dim tempXMax As Integer
                    Dim tempYMin As Integer
                    Dim tempYMax As Integer

                    If upDownDirection = "down" Then
                        tempYMin = .Panel2.Height / 2
                        tempYMax = .Panel2.Height
                    Else
                        tempYMin = 0
                        tempYMax = .Panel2.Height / 2
                    End If

                    If leftRightDirection = "right" Then
                        tempXMin = .Panel2.Width / 2
                        tempXMax = .Panel2.Width
                    Else
                        tempXMin = 0
                        tempXMax = .Panel2.Width / 2
                    End If


                    Dim tempX As Integer = rand(tempXMax, tempXMin)
                    Dim tempY As Integer = rand(tempYMax, tempYMin)

                    .handlePanelClick(tempX, tempY, MouseButtons.Left)
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub


    Public Sub finishedInstructions(ByVal sinstr As String)
        Try
            With frmMain
                showInstructions = False

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).fireX = -1
                    playerlist(i).fireY = -1

                    playerlist(i).fireMMX = -1
                    playerlist(i).fireMMY = -1

                    playerlist(i).showChat = False
                Next

                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).myX = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).myY = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).targetX = playerlist(i).myX
                    playerlist(i).targetY = playerlist(i).myY
                Next

                playerlist(inumber).targetX = playerlist(inumber).myX
                playerlist(inumber).targetY = playerlist(inumber).myY

                frmInstructions.Close()

                leftSideOn = True
                rightSideOn = True

                'setup prey
                smallPanelX = rand(5, 4)
                smallPanelY = rand(6, 1)
                largePanelY = rand(3, 1)

                For i As Integer = 1 To smallPreyPerPeriod
                    smallPrey(i).newLocation()
                Next

                For i As Integer = smallPreyPerPeriod + 1 To 200
                    smallPrey(i).status = "out"
                Next

                For i As Integer = 1 To largePreyPerPeriod
                    largePrey(i).newLocation()
                Next

                .cmdFire.Enabled = True
                periodEarningsShown = False

                .initializeMiniMap()
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub unStick(ByVal sinstr As String)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempN As Integer = msgtokens(nextToken)
            nextToken += 1

            playerlist(tempN).myX = 5040
            playerlist(tempN).myY = 2100

            playerlist(tempN).targetX = playerlist(tempN).myX
            playerlist(tempN).targetY = playerlist(tempN).myY
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeTansfer(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                updateStringTex(.Device, .timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(CInt(msgtokens(nextToken))))
                nextToken += 1

                .txtPeriod.Text = msgtokens(nextToken)
                nextToken += 1

                If Not showInstructions Then phase = msgtokens(nextToken)
                nextToken += 1

                currentHealth = msgtokens(nextToken)
                playerlist(inumber).updateHealthBar()
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    Dim tempUnitsPlayer As Integer = msgtokens(nextToken)
                    nextToken += 1

                    Dim tempUnitsPot As Integer = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).stunned = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).stunning = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).coolDown = msgtokens(nextToken)
                    nextToken += 1

                    If tempUnitsPlayer <> playerlist(i).unitsPlayer Then
                        playerlist(i).unitsPlayer = tempUnitsPlayer
                        playerlist(i).updatePlayerTex()
                    End If

                    If tempUnitsPot <> playerlist(i).unitsPot Then
                        playerlist(i).unitsPot = tempUnitsPot
                        playerlist(i).updatePlayerTex()
                    End If

                    If playerlist(i).coolDown > 0 Then
                        updateStringTex(.Device, playerlist(i).stunnedTex, 12, Drawing.Color.Black, "Cooling..." & playerlist(i).coolDown)
                    End If
                Next

                'transfer animation
                Dim tempDirection As String = msgtokens(nextToken)
                nextToken += 1

                Dim tempAmount As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempPotOwner As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempPlayer As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempIndex As Integer = msgtokens(nextToken)
                nextToken += 1

                If showInstructions Then
                    If tempIndex <> inumber Then Exit Sub
                End If

                Dim tempStartX As Double
                Dim tempStartY As Double
                Dim tempEndX As Double
                Dim tempEndY As Double

                playerlist(tempIndex).stunning = 0

                If playerlist(tempIndex).myGroup <> playerlist(inumber).myGroup Then Exit Sub

                If tempDirection = "Me -> Pot" Then
                    tempStartX = playerlist(tempIndex).myX
                    tempStartY = playerlist(tempIndex).myY

                    tempEndX = playerlist(tempPotOwner).fireX
                    tempEndY = playerlist(tempPotOwner).fireY
                ElseIf tempDirection = "Pot -> Me" Then
                    tempStartX = playerlist(tempPotOwner).fireX
                    tempStartY = playerlist(tempPotOwner).fireY

                    tempEndX = playerlist(tempIndex).myX
                    tempEndY = playerlist(tempIndex).myY
                ElseIf InStr(tempDirection, "Me -> ") > 0 Then
                    tempStartX = playerlist(tempIndex).myX
                    tempStartY = playerlist(tempIndex).myY

                    tempEndX = playerlist(tempPlayer).myX
                    tempEndY = playerlist(tempPlayer).myY

                ElseIf InStr(tempDirection, " -> Me") > 0 Then
                    tempStartX = playerlist(tempPlayer).myX
                    tempStartY = playerlist(tempPlayer).myY

                    tempEndX = playerlist(tempIndex).myX
                    tempEndY = playerlist(tempIndex).myY
                End If

                .transferCount += 1
                .transfers(.transferCount) = New clsTransfer(tempStartX, tempStartY, tempEndX, tempEndY)
                If .transferCount = 1000 Then .transferCount = 0

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeStun(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim tempSource As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempTarget As Integer = msgtokens(nextToken)
                nextToken += 1

                playerlist(tempSource).stunning = tempTarget

                playerlist(tempTarget).stunned = interactionLength

                playerlist(tempTarget).targetX = playerlist(tempTarget).myX
                playerlist(tempTarget).targetY = playerlist(tempTarget).myY

                playerlist(tempSource).targetX = playerlist(tempSource).myX
                playerlist(tempSource).targetY = playerlist(tempSource).myY

                updateStringTex(.Device, playerlist(tempTarget).stunnedTex, 12, Drawing.Color.Black, "zzz..." & playerlist(tempTarget).stunned)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendStun(target As Integer)
        Try
            With frmMain

                If showInstructions Then
                    playerlist(inumber).stunning = target
                    playerlist(target).stunned = interactionLength

                    playerlist(target).targetX = playerlist(target).myX
                    playerlist(target).targetY = playerlist(target).myY

                    playerlist(inumber).targetX = playerlist(inumber).myX
                    playerlist(inumber).targetY = playerlist(inumber).myY

                    updateStringTex(.Device, playerlist(target).stunnedTex, 12, Drawing.Color.Black, "zzz..." & playerlist(target).stunned)
                Else
                    playerlist(inumber).stunning = target

                    Dim outstr As String = ""

                    outstr = target & ";"

                    wskClient.Send("11", outstr)
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeHit(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                'check this
                healthLoss = playerlist(inumber).currentHealth

                Dim tempSource As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempTarget As Integer = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).stunned = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).stunning = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).coolDown = msgtokens(nextToken)
                    nextToken += 1

                    playerlist(i).currentHealth = msgtokens(nextToken)
                    nextToken += 1
                Next

                playerlist(tempSource).updateHealthBar()
                playerlist(tempTarget).updateHealthBar()

                updateStringTex(.Device, playerlist(tempSource).stunnedTex, 12, Drawing.Color.Black, "Cooling..." & playerlist(tempSource).coolDown)

                currentHealth = playerlist(inumber).currentHealth

                healthLoss -= currentHealth

                .healthFadeAmount = healthLoss
                .healthFadeTransparency = 250
                .healthTransitionColor = Drawing.Color.Red

                If playerlist(tempSource).myGroup <> playerlist(inumber).myGroup Then Exit Sub

                'explosion animation
                Dim tempN As Integer = -1
                For i As Integer = 1 To 100
                    If explosionList(i).enabled = False Then
                        tempN = i
                    End If
                Next

                If tempN = -1 Then Exit Sub

                explosionList(tempN).myX = playerlist(tempTarget).myX
                explosionList(tempN).myY = playerlist(tempTarget).myY
                explosionList(tempN).enabled = True
                explosionList(tempN).easOutSpeed = 0.01
                explosionList(tempN).owner = tempSource
                explosionList(tempN).rotation = 0
                explosionList(tempN).size = 0.5
                explosionList(tempN).speed = 0.11
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub addEarningsFountain(text As String,
                                startX As Integer,
                                startY As Integer,
                                endX As Integer,
                                endY As Integer,
                                fontSize As Integer,
                                fontColor As Brush,
                                speed As Integer,
                                easeOutPercent As Double,
                                Optional minimumSpeed As Double = 0,
                                Optional symbol As String = "")
        Try
            With frmMain
                Dim tempEFI As Integer = -1
                For k As Integer = 1 To 100
                    If earningsFountainList(k).enabled = False Then
                        tempEFI = k
                        Exit For
                    End If
                Next

                If tempEFI <> -1 Then
                    earningsFountainList(tempEFI) = New earningsFoutain(text, .Device, fontSize, fontColor, symbol)
                    earningsFountainList(tempEFI).myX = startX
                    earningsFountainList(tempEFI).myY = startY

                    earningsFountainList(tempEFI).targetX = endX
                    earningsFountainList(tempEFI).targetY = endY

                    earningsFountainList(tempEFI).speed = speed
                    earningsFountainList(tempEFI).easOutSpeed = easeOutPercent

                    earningsFountainList(tempEFI).minimumSpeed = minimumSpeed
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeStartTug(ByVal sinstr As String)
        Try
            With frmMain
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim index As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tempPlayer As Integer = msgtokens(nextToken)
                nextToken += 1

                playerlist(index).tugAmount = msgtokens(nextToken)
                playerlist(tempPlayer).tugAmount = playerlist(index).tugAmount
                nextToken += 1

                playerlist(tempPlayer).targetX = msgtokens(nextToken)
                nextToken += 1

                playerlist(tempPlayer).targetY = msgtokens(nextToken)
                nextToken += 1

                playerlist(index).targetX = msgtokens(nextToken)
                nextToken += 1

                playerlist(index).targetY = msgtokens(nextToken)
                nextToken += 1

                playerlist(index).tugging = True
                playerlist(tempPlayer).tugging = True

                playerlist(index).tugger = index
                playerlist(tempPlayer).tugger = index

                playerlist(index).tugTarget = tempPlayer
                playerlist(tempPlayer).tugTarget = tempPlayer

                playerlist(index).stunned = 0
                playerlist(tempPlayer).stunned = 0

                playerlist(index).stunning = 0
                playerlist(tempPlayer).stunning = 0

                playerlist(index).coolDown = 0
                playerlist(tempPlayer).coolDown = 0

                playerlist(index).tugOpponet = tempPlayer
                playerlist(tempPlayer).tugOpponet = index

                updateStringTex(.Device,
                                playerlist(index).tugAmountTex,
                                20,
                                Drawing.Color.Black,
                                playerlist(index).tugAmount)

                If index = inumber Or tempPlayer = inumber Then

                    .pnlYield.Visible = True
                    .pnlYield.Location = New System.Drawing.Point(.Panel2.Width / 2, .Panel2.Height / 2 - 150)

                    If index = inumber Then
                        .ToolTip1.SetToolTip(.cmdYield, "Yield to " & playerlist(tempPlayer).colorName & ".")
                    Else
                        .ToolTip1.SetToolTip(.cmdYield, "Yield to " & playerlist(index).colorName & ".")
                    End If
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeYieldTug(ByVal sinstr As String)
        Try
            With frmMain

                Dim msgtokens() As String = Split(sinstr, ",")
                Dim nextToken As Integer = 0

                Dim index As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim opponet As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tugger As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tugTarget As Integer = msgtokens(nextToken)
                nextToken += 1

                Dim tugAmount As Integer = msgtokens(nextToken)
                nextToken += 1

                If playerlist(index).myGroup <> playerlist(inumber).myGroup Then Exit Sub

                playerlist(index).unitsPlayer = msgtokens(nextToken)
                nextToken += 1

                playerlist(opponet).unitsPlayer = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    playerlist(i).tugHelping = msgtokens(nextToken)
                    nextToken += 1
                Next

                playerlist(index).updatePlayerTex()
                playerlist(opponet).updatePlayerTex()

                playerlist(index).tugging = False
                playerlist(opponet).tugging = False

                If inumber = index Or inumber = opponet Then
                    .pnlYield.Visible = False
                End If

                If playerlist(index).tugger = index Then
                    'tugger yields
                Else
                    addEarningsFountain("+" & tugAmount & " units.",
                                    playerlist(opponet).myX,
                                    playerlist(opponet).myY - 90,
                                    playerlist(opponet).myX,
                                    playerlist(opponet).myY - 140,
                                    20,
                                    Brushes.White,
                                    0.2,
                                    0.99,
                                    0.25,
                                    "units")

                    addEarningsFountain("-" & tugAmount & " units.",
                                    playerlist(index).myX,
                                    playerlist(index).myY - 90,
                                    playerlist(index).myX,
                                    playerlist(index).myY - 140,
                                    20,
                                    Brushes.White,
                                    0.2,
                                    0.99,
                                    0.25,
                                    "units")
                End If

                addEarningsFountain(" ",
                                    playerlist(index).myX,
                                    playerlist(index).myY - 125,
                                    playerlist(index).myX,
                                    playerlist(index).myY - 175,
                                    20,
                                    Brushes.White,
                                    0.2,
                                    0.99,
                                    0.25,
                                    "yield")

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeTugHelp(ByVal sinstr As String)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempIndex As Integer = msgtokens(nextToken)
            nextToken += 1

            Dim tempTarget As Integer = msgtokens(nextToken)
            nextToken += 1

            playerlist(tempIndex).tugHelping = True
            playerlist(tempIndex).tugHelpTarget = tempTarget
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeCancelTugHelp(ByVal sinstr As String)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempIndex As Integer = msgtokens(nextToken)
            nextToken += 1

            playerlist(tempIndex).tugHelping = False
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function returnColorCompliment(c As System.Drawing.Color) As System.Drawing.Color
        Try

            Return System.Drawing.Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B)

        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return System.Drawing.Color.Black
        End Try
    End Function

    Public Function getMyColorName(ByVal index As Integer) As String
        Try
            'appEventLog_Write("get color")

            Select Case index
                Case 1
                    Return "Blue"
                Case 2
                    Return "Red"
                Case 3
                    Return "Yellow"
                Case 4
                    Return "Green"
                Case 5
                    Return "Purple"
                Case 6
                    Return "Orange"
                Case 7
                    Return "Brown"
                Case 8
                    Return "Teal"
                Case 9
                    Return "Gray"
                Case 10
                    Return "Khaki"
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            Return ""
            appEventLog_Write("error getMyColor:", ex)
        End Try
    End Function
End Module
