'Programmed by Jeffrey Kirchner
'kirchner@chapman.edu/jkirchner@gmail.com
'Economic Science Institute, Chapman University 2008-2011 ©

Imports System.IO

Module modMain
#Region " General Variables "
    Public playerList(30) As player                  'array of players
    Public playerCount As Integer                    'number of players connected
    Public numberOfPlayers As Integer                'number of desired players
    Public sfile As String                           'location of intialization file  
    Public checkin As Integer                        'global counter 
    Public connectionCount As Integer                'total number of connections made since server start 
    Public portNumber As Integer                     'port number sockect traffic is operation on 
    Public summaryDf As StreamWriter                 'data file
    Public frmServer As New frmMain                  'main form 
    Public filename As String                        'location of data file
    Public filename2 As String                       'location of data file
    Public showInstructions As Boolean               'show client instructions  
    Public currentInstruction As Integer             'current page of instructions 
#End Region

    'global variables here
    Public numberOfPeriods As Integer     'number of periods
    Public currentPeriod As Integer       'current period 
    Public playerSpeed As Double
    Public bushCount As Integer
    Public largePreyPerPeriod As Integer
    Public smallPreyPerPeriod As Integer
    Public largePreyValue As Integer
    Public smallPreyValue As Integer
    Public preyMovementRate As Integer
    Public huntingLength As Integer
    Public tradingLength As Integer
    Public timeRemaining As Integer
    Public phase As String
    Public hearthCapacity As Integer
    Public stepSpeed As Integer
    Public largePreyProbability As Integer
    Public startingHealth As Integer
    Public healthLoss As Integer
    Public interimLength As Integer

    Public bushLocations(100) As Point
    Public treeLocations(100) As Point
    Public rockLocations(100) As Point

    Public eventsDf As StreamWriter                 'data file
    Public replayDf As StreamWriter                 'data file
    Public pairwiseDF As StreamWriter
    Public testMode As Boolean

    Public restPeriodFrequency As Integer
    Public restPeriodLength As Integer

    Public instructionX As Integer            'start up locations of windows
    Public instructionY As Integer
    Public windowX As Integer
    Public windowY As Integer

    Public startTime As DateTime

    Public showHealth As Boolean

    Public interactionRadius As Integer
    Public interactionLength As Integer
    Public interactionCoolDown As Integer

    Public allowHit As Boolean
    Public hitDamage As Integer
    Public hitCost As Integer
    Public earningsMultiplier As Double
    'Public clientViewers As Integer
    Public allowTake As Boolean

    Public smallPanelX As Integer
    Public smallPanelY As Integer
    Public largePanelY As Integer

    Public tugOfWar As Boolean
    Public tugOfWarCost As Double

    Public smallPreyAvailable As Boolean
    Public potsAvailable As Boolean

    Public playerScale As Double
    Public worldWidth As Integer
    Public worldHeight As Integer

    Public landMarks As Boolean

#Region " General Functions "
    Public Sub main(ByVal args() As String)
        connectionCount = 0

        AppEventLog_Init()
        appEventLog_Write("Load")

        ToggleScreenSaverActive(False)

        Application.EnableVisualStyles()
        Application.Run(frmServer)

        ToggleScreenSaverActive(True)

        appEventLog_Write("Exit")
        AppEventLog_Close()
    End Sub

    Public Sub takeIP(ByVal sinstr As String, ByVal index As Integer)
        Try
            playerList(index).ipAddress = sinstr
        Catch ex As Exception
            appEventLog_Write("error takeIP:", ex)
        End Try
    End Sub

    Public Function roundUp(ByVal value As Double) As Integer
        Try
            Dim msgtokens() As String

            If InStr(CStr(value), ".") Then
                msgtokens = CStr(value).Split(".")

                roundUp = msgtokens(0)
                roundUp += 1
            Else
                roundUp = value
            End If
        Catch ex As Exception
            appEventLog_Write("error roundUp:", ex)
            Return 0
        End Try
    End Function

    Public Function getMyColor(ByVal index As Integer) As Color
        Try
            'appEventLog_Write("get color")

            Select Case index
                Case 1
                    getMyColor = Color.Blue
                Case 2
                    getMyColor = Color.Red
                Case 3
                    getMyColor = Color.Yellow
                Case 4
                    getMyColor = Color.Green
                Case 5
                    getMyColor = Color.Purple
                Case 6
                    getMyColor = Color.Orange
                Case 7
                    getMyColor = Color.Brown
                Case 8
                    getMyColor = Color.Teal
            End Select
        Catch ex As Exception
            appEventLog_Write("error getMyColor:", ex)
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
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            Return ""
            appEventLog_Write("error getMyColor:", ex)
        End Try
    End Function

    Public Function colorToId(ByVal str As String) As Integer
        Try
            'Dim i As Integer

            'appEventLog_Write("color to id :" & str)

            Select Case str
                Case "Blue"
                    Return 1
                Case "Red"
                    Return 2
                Case "Yellow"
                    Return 3
                Case "Green"
                    Return 4
                Case "Purple"
                    Return 5
                Case "Orange"
                    Return 6
                Case "Brown"
                    Return 7
                Case "Teal"
                    Return 8
                Case Else
                    Return 0
            End Select
        Catch ex As Exception
            appEventLog_Write("error colorToId:", ex)
            Return 0
        End Try
    End Function
#End Region

    Public Sub takeMessage(ByVal sinstr As String)
        'when a message is received from a client it is parsed here
        'msgtokens(1) has type of message sent, having different types of messages allows you to send different formats for different actions.
        'msgtokens(2) has the semicolon delimited data that is to be parsed and acted upon.  
        'index has the client ID that sent the data.  Client ID is assigned by connection order, indexed from 1.

        Try
            With frmServer
                Dim msgtokens() As String
                Dim outstr As String

                msgtokens = sinstr.Split("|")

                Dim index As Integer
                index = msgtokens(0)

                Application.DoEvents()

                Select Case msgtokens(1) 'case statement to handle each of the different types of messages
                    Case "01"
                        updateTarget(msgtokens(2), index)
                    Case "02"
                        checkin += 1
                        .DataGridView1.Rows(index - 1).Cells(2).Value = "Waiting"

                        If checkin = numberOfPlayers Then
                            MessageBox.Show("Begin Game.", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            startTime = Now

                            showInstructions = False
                            checkin = 0
                            .Timer1.Enabled = True

                            For i As Integer = 1 To numberOfPlayers
                                Dim msgtokens2() As String = getINI(sfile, "players", CStr(i)).Split(";")
                                playerList(i).myX = msgtokens2(0)
                                playerList(i).myY = msgtokens2(1)

                                playerList(i).targetX = playerList(i).myX
                                playerList(i).targetY = playerList(i).myY
                            Next i

                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).finishedInstructions()

                                If i <= numberOfPlayers Then .DataGridView1.Rows(i - 1).Cells(2).Value = "Playing"
                            Next

                        End If
                    Case "03"
                        takeIP(msgtokens(2), index)
                    Case "04"
                        takeChat(msgtokens(2), index)
                    Case "05"
                        takeCatch(msgtokens(2), index)
                    Case "06"
                        takeFireLocation(msgtokens(2), index)
                    Case "07"
                        playerList(index).takeName(msgtokens(2))
                        checkin += 1

                        If checkin = numberOfPlayers Then
                            summaryDf.WriteLine(",")
                            summaryDf.WriteLine(",")

                            outstr = "Name,Earnings,"
                            summaryDf.WriteLine(outstr)

                            For i As Integer = 1 To numberOfPlayers
                                outstr = .DataGridView1.Rows(i - 1).Cells(1).Value & ","
                                outstr &= .DataGridView1.Rows(i - 1).Cells(3).Value & ","

                                summaryDf.WriteLine(outstr)
                            Next

                            summaryDf.Close()
                            eventsDf.Close()
                            replayDf.Close()
                            pairwiseDF.Close()
                        End If
                    Case "08"
                        takeTransfer(msgtokens(2), index)
                    Case "09"
                        playerList(index).takeUpdate(msgtokens(2))
                    Case "10"
                        updateInstructionDisplay(msgtokens(2), index)
                    Case "11"
                        takeStun(msgtokens(2), index)
                    Case "12"
                        takeHit(msgtokens(2), index)
                    Case "13"
                        takeYieldTug(msgtokens(2), index)
                    Case "14"
                        takeTugHelp(msgtokens(2), index)
                    Case "15"
                        takeCancelTugHelp(msgtokens(2), index)
                End Select

                Application.DoEvents()

            End With
            'all subs/functions should have an error trap
        Catch ex As Exception
            appEventLog_Write("error takeMessage: " & sinstr & " : ", ex)
        End Try

    End Sub

    Public Sub loadParameters()
        Try
            'load parameters from server.ini

            numberOfPlayers = getINI(sfile, "gameSettings", "numberOfPlayers")
            numberOfPeriods = getINI(sfile, "gameSettings", "numberOfPeriods")
            showInstructions = getINI(sfile, "gameSettings", "showInstructions")
            portNumber = getINI(sfile, "gameSettings", "port")
            playerSpeed = getINI(sfile, "gameSettings", "playerSpeed")
            bushCount = getINI(sfile, "gameSettings", "bushCount")
            largePreyPerPeriod = getINI(sfile, "gameSettings", "largePreyPerPeriod")
            largePreyValue = getINI(sfile, "gameSettings", "largePreyValue")
            smallPreyValue = getINI(sfile, "gameSettings", "smallPreyValue")
            preyMovementRate = getINI(sfile, "gameSettings", "preyMovementRate")
            huntingLength = getINI(sfile, "gameSettings", "huntingLength")
            tradingLength = getINI(sfile, "gameSettings", "tradingLength")
            hearthCapacity = getINI(sfile, "gameSettings", "hearthCapacity")
            stepSpeed = getINI(sfile, "gameSettings", "stepSpeed")
            largePreyProbability = getINI(sfile, "gameSettings", "largePreyProbability")
            startingHealth = getINI(sfile, "gameSettings", "startingHealth")
            healthLoss = getINI(sfile, "gameSettings", "healthLoss")
            interimLength = getINI(sfile, "gameSettings", "interimLength")
            testMode = getINI(sfile, "gameSettings", "testMode")
            restPeriodFrequency = getINI(sfile, "gameSettings", "restPeriodFrequency")
            restPeriodLength = getINI(sfile, "gameSettings", "restPeriodLength")
            instructionX = getINI(sfile, "gameSettings", "instructionX")
            instructionY = getINI(sfile, "gameSettings", "instructionY")
            windowX = getINI(sfile, "gameSettings", "windowX")
            windowY = getINI(sfile, "gameSettings", "windowY")
            showHealth = getINI(sfile, "gameSettings", "showHealth")

            interactionRadius = getINI(sfile, "gameSettings", "interactionRadius")
            interactionLength = getINI(sfile, "gameSettings", "interactionLength")
            interactionCoolDown = getINI(sfile, "gameSettings", "interactionCoolDown")

            allowHit = getINI(sfile, "gameSettings", "allowHit")
            hitCost = getINI(sfile, "gameSettings", "hitCost")
            hitDamage = getINI(sfile, "gameSettings", "hitDamage")
            earningsMultiplier = getINI(sfile, "gameSettings", "earningsMultiplier")
            allowTake = getINI(sfile, "gameSettings", "allowTake")

            tugOfWar = getINI(sfile, "gameSettings", "tugOfWar")
            tugOfWarCost = getINI(sfile, "gameSettings", "tugOfWarCost")
            smallPreyAvailable = getINI(sfile, "gameSettings", "smallPreyAvailable")

            potsAvailable = getINI(sfile, "gameSettings", "potsAvailable")

            playerScale = getINI(sfile, "gameSettings", "playerScale")
            worldWidth = getINI(sfile, "gameSettings", "worldWidth")
            worldHeight = getINI(sfile, "gameSettings", "worldHeight")
            landMarks = getINI(sfile, "gameSettings", "landMarks")
        Catch ex As Exception
            appEventLog_Write("error loadParameters:", ex)
        End Try
    End Sub

    Public Sub updateTarget(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = sinstr.Split(";")
            Dim nextToken As Integer = 0

            playerList(index).targetX = msgtokens(nextToken)
            nextToken += 1

            playerList(index).targetY = msgtokens(nextToken)
            nextToken += 1

            If playerList(index).stunning > 0 Then
                playerList(playerList(index).stunning).stunned = 0
                playerList(index).stunning = 0
                playerList(index).coolDown = interactionCoolDown
            End If

            Dim outstr As String = ""

            outstr = index & ";"
            outstr &= playerList(index).targetX & ";"
            outstr &= playerList(index).targetY & ";"

            For i As Integer = 1 To numberOfPlayers
                outstr &= playerList(i).stunned & ";"
                outstr &= playerList(i).stunning & ";"
                outstr &= playerList(i).coolDown & ";"
            Next

            If showInstructions Then
                playerList(index).updateTarget(outstr)
            Else
                For i As Integer = 1 To numberOfPlayers
                    playerList(i).updateTarget(outstr)
                Next
            End If


            playerList(index).writeEventData("New Destination", playerList(index).targetX & "," & playerList(index).targetY)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeChat(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim outstr As String = ""
                Dim chatText As String

                If playerList(index).stunning > 0 Then
                    playerList(playerList(index).stunning).stunned = 0
                    playerList(index).stunning = 0
                    playerList(index).coolDown = interactionCoolDown
                End If

                outstr = index & ";"
                outstr &= msgtokens(nextToken) & ";"
                chatText = msgtokens(nextToken)
                nextToken += 1

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).stunned & ";"
                    outstr &= playerList(i).stunning & ";"
                    outstr &= playerList(i).coolDown & ";"
                Next

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).sendChat(outstr)
                Next

                'record word count
                Dim msgtokens2() As String = chatText.Split(" ")
                playerList(index).wordCount += msgtokens2.Length

                'display chat
                .RichTextBox1.SelectionLength = 0
                .RichTextBox1.SelectionStart = .RichTextBox1.TextLength

                .RichTextBox1.SelectionColor = getMyColor(index)
                .RichTextBox1.SelectedText &= getMyColorName(index)

                .RichTextBox1.SelectionLength = 0
                .RichTextBox1.SelectionColor = Color.Black
                .RichTextBox1.SelectedText = ": " & chatText & vbCrLf

                .RichTextBox1.ScrollToCaret()

                'record data
                Dim nearbyPlayers As String = ""

                For i As Integer = 1 To numberOfPlayers
                    If i <> index Then
                        If returnDistance(playerList(index).myX, playerList(index).myY, playerList(i).myX, playerList(i).myY) < 700 Then
                            nearbyPlayers &= "1,"
                        Else
                            nearbyPlayers &= "0,"
                        End If
                    Else
                        nearbyPlayers &= "1,"
                    End If
                Next

                playerList(index).writeEventData("Chat", chatText.Replace(",", "<comma>") & "," & nearbyPlayers)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeCatch(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempPreyId As Integer = msgtokens(nextToken)
            nextToken += 1

            Dim tempType As String = msgtokens(nextToken)
            nextToken += 1

            Dim tempStatus As String = msgtokens(nextToken)
            nextToken += 1

            If tempStatus = "caught" Then
                If tempType = "large" Then
                    playerList(index).unitsPerson += largePreyValue
                    playerList(index).dataUnitsCaptured += largePreyValue
                    playerList(index).dataUnitsCapturedTotalLeft += largePreyValue

                    playerList(index).writeEventData("Capture", "Large,Success")
                Else
                    playerList(index).unitsPerson += smallPreyValue
                    playerList(index).dataUnitsCaptured += smallPreyValue
                    playerList(index).dataUnitsCapturedTotalRight += smallPreyValue

                    playerList(index).writeEventData("Capture", "Small")
                End If
            Else
                playerList(index).dataFailedCaptures += 1
                playerList(index).writeEventData("Capture", "Large,Fail")
            End If

            playerList(index).updateTime("0")

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeFireLocation(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = sinstr.Split(";")
            Dim nextToken As Integer = 0

            playerList(index).fireX = msgtokens(nextToken)
            nextToken += 1

            playerList(index).fireY = msgtokens(nextToken)
            nextToken += 1

            Dim outstr As String = ""

            outstr = index & ";"
            outstr &= playerList(index).fireX & ";"
            outstr &= playerList(index).fireY & ";"

            For i As Integer = 1 To numberOfPlayers
                playerList(i).updateFire(outstr)
            Next

            playerList(index).writeEventData("Place Pot", playerList(index).fireX & "," & playerList(index).fireY)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeTransfer(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = sinstr.Split(";")
            Dim nextToken As Integer = 0

            Dim tempDirection As String = msgtokens(nextToken)
            nextToken += 1

            Dim tempAmount As Integer = msgtokens(nextToken)
            nextToken += 1

            Dim tempPotOwner As Integer = msgtokens(nextToken)
            nextToken += 1

            Dim tempPlayer As Integer = msgtokens(nextToken)
            nextToken += 1

            If tempAmount = 0 Then Exit Sub

            Dim transferType As String = ""

            If tempDirection = "Me -> Pot" Then
                If playerList(index).unitsPerson < tempAmount Then Exit Sub
                If tempPotOwner = 0 Then Exit Sub
                If playerList(tempPotOwner).unitsPot + tempAmount > hearthCapacity Then Exit Sub

                playerList(index).unitsPerson -= tempAmount
                playerList(tempPotOwner).unitsPot += tempAmount

                If index <> tempPotOwner Then
                    playerList(index).dataUnitsSentPot(tempPotOwner, currentPeriod) += tempAmount
                    playerList(index).transferColor = Color.White

                    If tempAmount > 18 Then
                        playerList(index).dataUnitsSentGreaterThan18(tempPotOwner) += 1
                    End If
                Else
                    playerList(index).transferColor = Color.White
                End If

                playerList(index).writeEventData("Transfer Units", tempDirection & "," & getMyColorName(tempPotOwner) & "," & tempAmount & ",")

                playerList(index).transferSource = New Point(playerList(index).myX, playerList(index).myY)
                playerList(index).transferTarget = New Point(playerList(tempPotOwner).fireX, playerList(tempPotOwner).fireY)

                transferType = "transfer"
            ElseIf tempDirection = "Pot -> Me" Then
                If tempPotOwner = 0 Then Exit Sub
                If playerList(tempPotOwner).unitsPot < tempAmount Then Exit Sub

                playerList(tempPotOwner).unitsPot -= tempAmount
                playerList(index).unitsPerson += tempAmount

                If index <> tempPotOwner Then
                    playerList(index).dataUnitsTakenPot(tempPotOwner, currentPeriod) += tempAmount
                    playerList(index).dataUnitsTakenCount(tempPotOwner, currentPeriod) += 1
                    playerList(index).transferColor = Color.Maroon
                Else
                    playerList(index).transferColor = Color.White
                End If

                playerList(index).writeEventData("Transfer Units", tempDirection & "," & getMyColorName(tempPotOwner) & "," & tempAmount & ",")

                playerList(index).transferSource = New Point(playerList(tempPotOwner).fireX, playerList(tempPotOwner).fireY)
                playerList(index).transferTarget = New Point(playerList(index).myX, playerList(index).myY)

                transferType = "transfer"
            ElseIf InStr(tempDirection, "Me -> ") > 0 Then
                If playerList(index).unitsPerson < tempAmount Then Exit Sub
                If tempPlayer = 0 Then Exit Sub

                playerList(index).unitsPerson -= tempAmount
                playerList(tempPlayer).unitsPerson += tempAmount

                playerList(index).dataUnitsSentPerson(tempPlayer, currentPeriod) += tempAmount

                If tempAmount > 18 Then
                    playerList(index).dataUnitsSentGreaterThan18(tempPlayer) += 1
                End If

                playerList(index).writeEventData("Transfer Units", tempDirection & "," & getMyColorName(tempPlayer) & "," & tempAmount & ",")

                playerList(index).transferSource = New Point(playerList(index).myX, playerList(index).myY)
                playerList(index).transferTarget = New Point(playerList(tempPlayer).myX, playerList(tempPlayer).myY)
                playerList(index).transferColor = Color.White

                playerList(index).stunning = 0
                playerList(tempPlayer).stunned = 0

                transferType = "transfer"
            ElseIf InStr(tempDirection, " -> Me") > 0 Then
                If tempPlayer = 0 Then Exit Sub
                If playerList(tempPlayer).unitsPerson < tempAmount Then Exit Sub

                If tugOfWar Then
                    'start tug of war
                    playerList(index).tugging = True
                    playerList(tempPlayer).tugging = True

                    playerList(index).tugger = index
                    playerList(tempPlayer).tugger = index

                    playerList(index).tugTarget = tempPlayer
                    playerList(tempPlayer).tugTarget = tempPlayer

                    If playerList(index).myX < 1680 * 2 + 500 Then
                        playerList(index).targetX = 1680 * 2 + 500
                    End If

                    playerList(tempPlayer).targetX = playerList(index).targetX - 400
                    playerList(tempPlayer).targetY = playerList(index).targetY

                    playerList(index).tugAmount = tempAmount
                    playerList(tempPlayer).tugAmount = tempAmount

                    playerList(index).tugOpponet = tempPlayer
                    playerList(tempPlayer).tugOpponet = index

                    playerList(index).stunning = 0
                    playerList(tempPlayer).stunning = 0

                    playerList(index).coolDown = 0
                    playerList(tempPlayer).coolDown = 0

                    playerList(index).stunned = 0
                    playerList(tempPlayer).stunned = 0

                    transferType = "tug"

                    playerList(index).writeEventData("Tug Start", getMyColorName(tempPlayer) & ",")

                    playerList(index).tugsStarted(tempPlayer, currentPeriod) += 1
                Else
                    playerList(tempPlayer).unitsPerson -= tempAmount
                    playerList(index).unitsPerson += tempAmount

                    playerList(index).dataUnitsTakenPerson(tempPlayer, currentPeriod) += tempAmount
                    playerList(index).dataUnitsTakenCount(tempPlayer, currentPeriod) += 1

                    playerList(index).writeEventData("Transfer Units", tempDirection & "," & getMyColorName(tempPlayer) & "," & tempAmount & ",")

                    playerList(index).transferSource = New Point(playerList(tempPlayer).myX, playerList(tempPlayer).myY)
                    playerList(index).transferTarget = New Point(playerList(index).myX, playerList(index).myY)
                    playerList(index).transferColor = Color.Maroon

                    playerList(index).stunning = 0
                    playerList(index).coolDown = interactionCoolDown

                    transferType = "transfer"
                End If

            End If

            If transferType = "transfer" Then
                playerList(index).transferOpacity = 250

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).sendTransfer(sinstr, index)
                Next
            ElseIf transferType = "tug" Then
                Dim outstr As String = ""

                outstr = index & ";"
                outstr &= tempPlayer & ";"
                outstr &= tempAmount & ";"
                outstr &= playerList(tempPlayer).targetX & ";"
                outstr &= playerList(tempPlayer).targetY & ";"
                outstr &= playerList(index).targetX & ";"
                outstr &= playerList(index).targetY & ";"

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).sendStartTug(outstr)
                Next
            End If
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

    Public Sub updateInstructionDisplay(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim msgtokens() As String = sinstr.Split(";")
                Dim nextToken As Integer = 0

                .DataGridView1.Rows(index - 1).Cells(2).Value = "Page " & msgtokens(nextToken)
                nextToken += 1
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeStun(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                Dim tempTarget As Integer = msgtokens(nextToken)
                nextToken += 1

                If playerList(tempTarget).stunned > 0 Or
                   playerList(tempTarget).stunning > 0 Then Exit Sub

                If playerList(tempTarget).tugging Or
                   playerList(tempTarget).tugHelping Then Exit Sub

                If Not showInstructions Then
                    If playerList(index).unitsPerson = 0 And playerList(tempTarget).unitsPerson = 0 Then Exit Sub
                End If

                playerList(index).stunning = tempTarget
                playerList(tempTarget).stunned = interactionLength

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).sendStun(index, tempTarget)
                Next

                playerList(index).writeEventData("Stun", getMyColorName(tempTarget) & ",")
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub


    Public Sub takeHit(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim msgtokens() As String = Split(sinstr, ";")
                Dim nextToken As Integer = 0

                If playerList(index).currentHealth < hitCost Then Exit Sub

                Dim tempTarget As Integer = msgtokens(nextToken)
                nextToken += 1

                playerList(index).currentHealth -= hitCost
                If playerList(index).currentHealth < 0 Then playerList(index).currentHealth = 0

                playerList(tempTarget).currentHealth -= hitDamage
                If playerList(tempTarget).currentHealth < 0 Then playerList(tempTarget).currentHealth = 0

                playerList(index).stunning = 0
                playerList(index).coolDown = interactionCoolDown

                playerList(index).dataHitCount(tempTarget, currentPeriod) += 1

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).sendHit(index, tempTarget)
                Next

                playerList(index).transferSource = New Point(playerList(index).myX, playerList(index).myY)
                playerList(index).transferTarget = New Point(playerList(tempTarget).myX, playerList(tempTarget).myY)
                playerList(index).transferOpacity = 250
                playerList(index).transferColor = Color.Magenta

                playerList(index).writeEventData("Stun", tempTarget & ",")
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeYieldTug(ByVal sinstr As String, ByVal index As Integer)
        Try

            If Not playerList(index).tugging Then Exit Sub

            Dim outstr As String = calcYeildTug(sinstr, index)

            For i As Integer = 1 To numberOfPlayers
                playerList(i).sendYieldTug(outstr)
            Next

            playerList(index).writeEventData("Tug Yield", getMyColorName(playerList(index).tugOpponet) & ",")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function calcYeildTug(ByVal sinstr As String, ByVal index As Integer) As String
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0


            If playerList(index).tugger = index Then
                'tugger yields
            Else
                If playerList(index).unitsPerson >= playerList(index).tugAmount Then
                    playerList(index).unitsPerson -= playerList(index).tugAmount
                    playerList(playerList(index).tugOpponet).unitsPerson += playerList(index).tugAmount

                    playerList(playerList(index).tugOpponet).dataUnitsTakenPerson(index, currentPeriod) += playerList(index).tugAmount
                    playerList(playerList(index).tugOpponet).dataUnitsTakenCount(index, currentPeriod) += 1

                    playerList(playerList(index).tugOpponet).writeEventData("Transfer Units", getMyColorName(index) & " -> Me" & "," _
                                                     & getMyColorName(index) & "," _
                                                     & playerList(index).tugAmount & ",")

                    playerList(playerList(index).tugOpponet).transferSource = New Point(playerList(index).myX, playerList(index).myY)
                    playerList(playerList(index).tugOpponet).transferTarget = New Point(playerList(playerList(index).tugOpponet).myX, playerList(playerList(index).tugOpponet).myY)
                    playerList(playerList(index).tugOpponet).transferColor = Color.Maroon

                    playerList(playerList(index).tugOpponet).transferOpacity = 250
                End If
            End If

            playerList(index).tugging = False
            playerList(playerList(index).tugOpponet).tugging = False

            For i As Integer = 1 To numberOfPlayers
                If playerList(i).tugHelping And
                   (playerList(i).tugHelpTarget = index Or playerList(i).tugHelpTarget = playerList(index).tugOpponet) Then

                    playerList(i).tugHelping = False
                End If
            Next

            Dim outstr As String = ""

            outstr &= index & ","
            outstr &= playerList(index).tugOpponet & ","
            outstr &= playerList(index).tugger & ","
            outstr &= playerList(index).tugTarget & ","
            outstr &= playerList(index).tugAmount & ","

            outstr &= playerList(index).unitsPerson & ","
            outstr &= playerList(playerList(index).tugOpponet).unitsPerson & ","

            For i As Integer = 1 To numberOfPlayers
                outstr &= playerList(i).tugHelping & ","
            Next

            Return outstr
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return ""
        End Try
    End Function

    Public Sub takeTugHelp(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            Dim tempTarget As Integer = msgtokens(nextToken)
            nextToken += 1

            If Not playerList(tempTarget).tugging Then Exit Sub

            playerList(index).tugHelping = True
            playerList(index).tugHelpTarget = tempTarget

            playerList(index).tugsHelped(tempTarget, currentPeriod) += 1
            playerList(index).tugsHurt(playerList(tempTarget).tugOpponet, currentPeriod) += 1

            Dim outstr As String = ""

            outstr = index & ";"
            outstr &= tempTarget & ";"

            For i As Integer = 1 To numberOfPlayers
                playerList(i).sendHelpTug(outstr)
            Next

            playerList(index).writeEventData("Tug Help", getMyColorName(tempTarget) & ",")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeCancelTugHelp(ByVal sinstr As String, ByVal index As Integer)
        Try
            Dim msgtokens() As String = Split(sinstr, ";")
            Dim nextToken As Integer = 0

            playerList(index).tugHelping = False

            Dim outstr As String = ""

            outstr &= index & ";"

            For i As Integer = 1 To numberOfPlayers
                playerList(i).sendCancelHelpTug(outstr)
            Next

            playerList(index).writeEventData("Tug Help Cancel", playerList(index).tugOpponet & ",")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateGroups()
        Try
            With frmMain
                Dim msgtokens() As String = getINI(sfile, "groups", currentPeriod).Split(";")

                For i As Integer = 1 To numberOfPlayers
                    playerList(i).myGroup = msgtokens(i - 1)
                Next
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function getPullinDraws(index As Integer) As String
        Try
            Dim outstr As String = ""

            Dim msgtokens() As String = getINI(sfile, "largePrey", currentPeriod & "-" & index).Split(";")

            For i As Integer = 1 To largePreyPerPeriod
                outstr &= msgtokens(i - 1) & ";"
            Next

            Return outstr
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return ""
        End Try
    End Function
End Module
