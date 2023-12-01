Imports System.Drawing.Drawing2D

Public Class player
    Public inumber As Integer            'ID number
    Public sname As String               'name of person
    Public socketNumber As String        'winsock ID number
    Public relativeNumber As Integer     'either buyer or seller number
    Public earnings As Double            'experimental earnings
    Public ipAddress As String           'IP address of player's machine 
    Public myIPAddress As String         'IP address of player's machine 
    Public roundEarnings As Integer      'earnings for a induvidual round/period
    Public exchangeRate As Integer       'conversion rate from experimental dollars to $
    Public colorName As String           'colorName of player

    Public myX As Double
    Public myY As Double

    Public targetX As Double
    Public targetY As Double

    Public unitsPerson As Integer
    Public unitsPot As Integer

    Public fireX As Double
    Public fireY As Double

    Public currentHealth As Double
    Public healthAtPeriodStart As Double

    Public smallPrey(200) As prey
    Public largePrey(200) As prey

    Public mySmallPreyCount As Integer
    Public pullingIn As Boolean
    Public preyTarget As Integer

    'Side,UnitsCaptured,UnitsPerson,UnitsPot,UnitsTransfered"
    Public dataSide As String
    Public dataUnitsCaptured As Integer
    Public dataUnitsPerson As Integer
    Public dataUnitsPot As Integer
    Public dataUnitsSentPerson(8, 100) As Integer    'person,period
    Public dataUnitsSentPot(8, 100) As Integer
    Public dataUnitsTakenPerson(8, 100) As Integer
    Public dataUnitsTakenPot(8, 100) As Integer
    Public dataUnitsTakenCount(8, 100) As Integer
    Public dataFailedCaptures As Integer
    Public wordCount As Integer
    Public dataUnitsCapturedTotalLeft As Integer
    Public dataUnitsCapturedTotalRight As Integer
    Public dataUnitsSentGreaterThan18(8) As Integer

    Public healthLost(8, 100) As Double
    Public tugsStarted(8, 100) As Double
    Public tugsHelped(8, 100) As Double
    Public tugsHurt(8, 100) As Double
    Public healthLostForPerson(8, 100) As Double

    Public dataHitCount(8, 100) As Integer

    'transfer annimation
    Public transferOpacity As Integer = 0
    Public transferSource As Point
    Public transferTarget As Point
    Public transferColor As Color

    Public stunned As Integer
    Public stunning As Integer
    Public coolDown As Integer

    Public isClientViewer As Boolean

    Public tugging As Boolean
    Public tugger As Integer
    Public tugTarget As Integer
    Public tugAmount As Integer
    Public tugOpponet As Integer

    Public tugHelping As Boolean
    Public tugHelpTarget As Integer

    Public myGroup As Integer

    Public Sub New()
        'setup prey
        For i As Integer = 1 To 200
            smallPrey(i) = New prey
            smallPrey(i).myID = i
            smallPrey(i).status = "out"
            smallPrey(i).myType = "small"
        Next

        For i As Integer = 1 To largePreyPerPeriod
            largePrey(i) = New prey
            largePrey(i).myType = "large"
            largePrey(i).myID = i
            largePrey(i).status = "out"
        Next
    End Sub

    Public Sub begin()
        Try
            With frmServer
                'singal to clients to start the experiment

                If inumber > numberOfPlayers Then
                    isClientViewer = True
                Else
                    isClientViewer = False
                End If

                'winsock can send character strings to the clients
                Dim outstr As String = ""

                'create parseable string to send to clients by putting ";" between each value
                outstr = numberOfPeriods & ";"
                outstr &= numberOfPlayers & ";"
                outstr &= showInstructions & ";"
                outstr &= playerSpeed & ";"
                outstr &= bushCount & ";"
                outstr &= largePreyPerPeriod & ";"
                outstr &= smallPreyPerPeriod & ";"
                outstr &= largePreyValue & ";"
                outstr &= smallPreyValue & ";"
                outstr &= preyMovementRate & ";"
                outstr &= huntingLength & ";"
                outstr &= tradingLength & ";"
                outstr &= hearthCapacity & ";"
                outstr &= stepSpeed & ";"
                outstr &= largePreyProbability & ";"
                outstr &= startingHealth & ";"
                outstr &= healthLoss & ";"
                outstr &= interimLength & ";"
                outstr &= testMode & ";"
                outstr &= restPeriodFrequency & ";"
                outstr &= restPeriodLength & ";"
                outstr &= instructionX & ";"
                outstr &= instructionY & ";"
                outstr &= windowX & ";"
                outstr &= windowY & ";"
                outstr &= showHealth & ";"
                outstr &= interactionRadius & ";"
                outstr &= interactionLength & ";"
                outstr &= interactionCoolDown & ";"
                outstr &= allowHit & ";"
                outstr &= hitCost & ";"
                outstr &= hitDamage & ";"
                outstr &= earningsMultiplier & ";"
                outstr &= allowTake & ";"
                outstr &= tugOfWar & ";"
                outstr &= tugOfWarCost & ";"
                outstr &= smallPreyAvailable & ";"
                outstr &= potsAvailable & ";"
                outstr &= playerScale & ";"
                outstr &= worldWidth & ";"
                outstr &= worldHeight & ";"
                outstr &= landMarks & ";"

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).myX & ";"
                    outstr &= playerList(i).myY & ";"
                    outstr &= playerList(i).colorName & ";"
                    outstr &= playerList(i).currentHealth & ";"
                    outstr &= playerList(i).myGroup & ";"
                Next

                For i As Integer = 1 To bushCount
                    outstr &= getINI(sfile, "bushes", CStr(i))
                Next

                outstr &= getPullinDraws(inumber)

                dataSide = ""
                dataUnitsCaptured = 0
                dataUnitsPerson = 0
                dataUnitsPot = 0
                wordCount = 0
                dataUnitsCapturedTotalLeft = 0
                dataUnitsCapturedTotalRight = 0

                For i As Integer = 1 To numberOfPlayers
                    For j As Integer = 1 To 100
                        dataUnitsSentPerson(i, j) = 0
                        dataUnitsSentPot(i, j) = 0
                        dataUnitsTakenPerson(i, j) = 0
                        dataUnitsTakenPot(i, j) = 0
                        dataUnitsTakenCount(i, j) = 0
                        dataHitCount(i, j) = 0

                        healthLost(i, j) = 0
                        tugsStarted(i, j) = 0
                        tugsHelped(i, j) = 0
                        tugsHurt(i, j) = 0
                        healthLostForPerson(i, j) = 0
                    Next

                    dataUnitsSentGreaterThan18(i) = 0
                Next
                dataFailedCaptures = 0

                healthAtPeriodStart = currentHealth

                'call the send command (message ID found in takeMessage function,winsock ID,data) 
                .wsk_Col.Send("02", socketNumber, outstr)
            End With

        Catch ex As Exception
            appEventLog_Write("error player begin:", ex)
        End Try
    End Sub



    Public Sub resetClient()
        Try
            'kill client
            With frmServer
                .wsk_Col.Send("01", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("error resetClient:", ex)
        End Try
    End Sub

    Public Sub requsetIP(ByVal count As Integer)
        Try
            'request the client send it's IP address
            With frmServer
                .wsk_Col.Send("05", socketNumber, CStr(count))
            End With
        Catch ex As Exception
            appEventLog_Write("error requsetIP:", ex)
        End Try
    End Sub

    Public Sub endGame()
        Try
            'tell clients to end the game
            With frmServer
                Dim outstr As String = ""

                outstr &= updatePeriodEarnings() & ";"

                .wsk_Col.Send("06", socketNumber, outstr)

                .DataGridView1.Rows(inumber - 1).Cells(3).Value = Format(earnings / 100, "currency")

                writeSummaryData()
            End With
        Catch ex As Exception
            appEventLog_Write("error endGame:", ex)
        End Try
    End Sub

    Public Sub takeName(ByVal sinstr As String)
        Try
            'get the subject's name

            With frmServer
                sname = sinstr
                .DataGridView1.Rows(inumber - 1).Cells(1).Value = sname
            End With
        Catch ex As Exception
            appEventLog_Write("error takeName:", ex)
        End Try
    End Sub

    Public Sub endEarly()
        Try
            'end experiment early

            With frmServer
                Dim outstr As String

                outstr = numberOfPeriods & ";"
                .wsk_Col.Send("12", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error endEarly:", ex)
        End Try
    End Sub

    Public Sub updateTarget(ByVal outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("04", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateFire(ByVal outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("10", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendChat(ByVal outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("07", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateTime(yeildList As String)
        Try
            With frmServer
                Dim outstr As String = ""
                outstr &= timeRemaining & ";"
                outstr &= currentPeriod & ";"
                outstr &= phase & ";"
                'outstr &= currentHealth & ";"

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).unitsPerson & ";"
                    outstr &= playerList(i).unitsPot & ";"
                    outstr &= playerList(i).currentHealth & ";"
                    outstr &= playerList(i).stunned & ";"
                    outstr &= playerList(i).stunning & ";"
                    outstr &= playerList(i).coolDown & ";"
                    outstr &= playerList(i).tugHelping & ";"
                Next

                outstr &= yeildList

                .wsk_Col.Send("08", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub startInterim()
        Try
            With frmServer
                Dim outstr As String = ""

                outstr &= updatePeriodEarnings() & ";"

                .wsk_Col.Send("11", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function updatePeriodEarnings() As String
        Try
            With frmServer
                Dim outstr As String = ""

                If currentPeriod Mod restPeriodFrequency <> 0 Or phase = "rest" Or currentPeriod - 1 = numberOfPeriods Then

                    Dim unitsAdded As Integer = 0
                    Dim unitsWasted As Integer = 0

                    'If Not currentPeriod Mod restPeriodFrequency = 0 Then

                    If potsAvailable Then
                        currentHealth += unitsPot
                        unitsAdded = unitsPot
                    Else
                        currentHealth += Math.Min(unitsPerson, hearthCapacity)
                        unitsAdded = Math.Min(unitsPerson, hearthCapacity)

                        unitsWasted = Math.Max(unitsPerson - hearthCapacity, 0)
                    End If

                    If currentHealth > 100 Then currentHealth = 100
                    earnings += Math.Round(currentHealth * earningsMultiplier)

                    .DataGridView1.Rows(inumber - 1).Cells(3).Value = Format(earnings / 100, "currency")

                    dataUnitsPerson = unitsPerson
                    dataUnitsPot = unitsPot
                    'End If

                    unitsPerson = 0
                    unitsPot = 0
                    tugging = False

                    outstr &= unitsAdded & ","
                    outstr &= unitsWasted & ","
                    outstr &= currentHealth & ","
                    outstr &= earnings & ","
                End If

                Return outstr
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return ""
        End Try
    End Function

    Public Sub startRest()
        Try
            With frmServer
                Dim outstr As String = ""

                outstr &= updatePeriodEarnings() & ";"

                .wsk_Col.Send("14", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub startNextPeriod()
        Try
            With frmServer

                dataSide = ""

                dataUnitsCaptured = 0
                dataUnitsPerson = 0
                dataUnitsPot = 0
                wordCount = 0
                dataFailedCaptures = 0

                currentHealth -= healthLoss
                If currentHealth < 0 Then currentHealth = 0

                healthAtPeriodStart = currentHealth

                Dim outstr As String = currentPeriod & ";"
                outstr &= smallPreyPerPeriod & ";"
                outstr &= currentHealth & ";"

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).myGroup & ";"
                Next

                outstr &= getPullinDraws(inumber)

                .wsk_Col.Send("09", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub takeUpdate(ByVal sinstr As String)
        Try
            Dim msgtokens() As String = sinstr.Split(";")
            Dim nextToken As Integer = 0

            myX = msgtokens(nextToken)
            nextToken += 1

            myY = msgtokens(nextToken)
            nextToken += 1

            pullingIn = msgtokens(nextToken)
            nextToken += 1

            preyTarget = msgtokens(nextToken)
            nextToken += 1

            mySmallPreyCount = msgtokens(nextToken)
            nextToken += 1

            For i As Integer = 1 To mySmallPreyCount
                smallPrey(i).myX = msgtokens(nextToken)
                nextToken += 1

                smallPrey(i).myY = msgtokens(nextToken)
                nextToken += 1

                smallPrey(i).status = msgtokens(nextToken)
                nextToken += 1
            Next

            For i As Integer = 1 To largePreyPerPeriod
                largePrey(i).myX = msgtokens(nextToken)
                nextToken += 1

                largePrey(i).myY = msgtokens(nextToken)
                nextToken += 1

                largePrey(i).status = msgtokens(nextToken)
                nextToken += 1
            Next

            If phase = "hunting" And dataSide = "" Then
                If myX <= 1680 * 2 Then
                    dataSide = "left"
                ElseIf myX >= 1680 * 4 Then
                    dataSide = "right"
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub writeSummaryData()
        Try
            'period summary data
            Dim outstr As String = ""
            Dim tempGroupSize As Integer = 0

            outstr = currentPeriod - 1 & ","
            outstr &= inumber & ","
            outstr &= myGroup & ","
            outstr &= getMyColorName(inumber) & ","
            outstr &= dataSide & ","
            outstr &= dataUnitsCaptured & ","
            outstr &= dataUnitsPerson & ","
            outstr &= dataUnitsPot & ","
            outstr &= dataFailedCaptures & ","
            outstr &= currentHealth & ","
            outstr &= wordCount & ","

            'TotalShared,Captured>1Unit,NewHealth,PeriodSQ,

            If dataUnitsCaptured > 0 Then
                outstr &= "1,"
            Else
                outstr &= "0,"
            End If

            outstr &= healthAtPeriodStart & ","
            outstr &= (currentPeriod - 1) ^ 2 & ","

            For i As Integer = 1 To numberOfPlayers
                outstr &= dataUnitsSentPerson(i, currentPeriod - 1) & ","
                outstr &= dataUnitsSentPot(i, currentPeriod - 1) & ","
                outstr &= dataUnitsTakenPerson(i, currentPeriod - 1) & ","
                outstr &= dataUnitsTakenPot(i, currentPeriod - 1) & ","

                If playerList(i).fireX <> -1 And fireX <> -1 Then
                    If returnDistance(fireX, fireY, playerList(i).fireX, playerList(i).fireY) < 700 Then
                        outstr &= "true,"
                        tempGroupSize += 1
                    Else
                        outstr &= "false,"
                    End If
                Else
                    outstr &= "false,"
                End If

                outstr &= dataUnitsSentPerson(i, currentPeriod - 1) + dataUnitsSentPot(i, currentPeriod - 1) & ","
                outstr &= playerList(i).dataUnitsSentPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsSentPot(inumber, currentPeriod - 1) & ","

                outstr &= dataUnitsTakenPerson(i, currentPeriod - 1) + dataUnitsTakenPot(i, currentPeriod - 1) & ","
                outstr &= dataUnitsTakenCount(i, currentPeriod - 1) & ","

                outstr &= playerList(i).dataUnitsTakenPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsTakenPot(inumber, currentPeriod - 1) & ","
                outstr &= playerList(i).dataUnitsTakenCount(inumber, currentPeriod - 1) & ","

                outstr &= (dataUnitsTakenPerson(i, currentPeriod - 1) + dataUnitsTakenPot(i, currentPeriod - 1)) -
                          (playerList(i).dataUnitsTakenPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsTakenPot(inumber, currentPeriod - 1)) & ","

                outstr &= dataHitCount(i, currentPeriod - 1) & ","
                outstr &= playerList(i).dataHitCount(inumber, currentPeriod - 1) & ","

                outstr &= healthLost(i, currentPeriod - 1) & ","
                outstr &= playerList(i).healthLost(inumber, currentPeriod - 1) & ","

                outstr &= healthLostForPerson(i, currentPeriod - 1) & ","
                outstr &= playerList(i).healthLostForPerson(inumber, currentPeriod - 1) & ","

                outstr &= tugsStarted(i, currentPeriod - 1) & ","
                outstr &= playerList(i).tugsStarted(inumber, currentPeriod - 1) & ","

                outstr &= playerList(i).tugsHelped(inumber, currentPeriod - 1) & ","
                outstr &= tugsHelped(i, currentPeriod - 1) & ","

                outstr &= playerList(i).tugsHurt(inumber, currentPeriod - 1) & ","
                outstr &= tugsHurt(i, currentPeriod - 1) & ","
            Next

            outstr &= calcSentToOthers(currentPeriod - 1) & ","
            outstr &= calcOthersSentToMe(currentPeriod - 1) & ","
            outstr &= calcTakenFromOthers(currentPeriod - 1) & ","
            outstr &= calcTimesTakenFromOthers(currentPeriod - 1) & ","
            outstr &= calcOthersTakenFromMe(currentPeriod - 1) & ","
            outstr &= calcTimesOthersTakenFromMe(currentPeriod - 1) & ","
            outstr &= calcTimesHitOthers(currentPeriod - 1) & ","
            outstr &= calcTimesOthersHitMe(currentPeriod - 1) & ","
            outstr &= tempGroupSize & ","

            summaryDf.WriteLine(outstr)

            'pair wise data
            '"Period,PlayerA,PlayerB," & _
            '"AmountSentA->B,AmountTakeA<-B,TotalAmountSentA->B,TotalAmountTakeA<-B,AmountCapturedA,SideA,HealthA,TotalSideLeftA,TotalSideRightA,TotalTimeSentA->B>18,AmountSentA->Others,AmountTakeA<-Others,AmountSentOthers->A,AmountTakeOthers<-A,TotalAmountSentA->Others,TotalAmountTakeA<-Others,TotalAmountSentOthers->A,TotalAmountTakeOthers<-A," & _
            '"AmountSentB->A,AmountTakeB<-A,TotalAmountSentB->A,TotalAmountTakeB<-A,AmountCapturedB,SideB,HealthB,TotalSideLeftB,TotalSideRightB,TotalTimeSentB->A>18,AmountSentB->Others,AmountTakeB<-Others,AmountSentOthers->B,AmountTakeOthers<-B,TotalAmountSentB->Others,TotalAmountTakeB<-Others,TotalAmountSentOthers->B,TotalAmountTakeOthers<-B,APotNearBPot"


            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then
                    outstr = ""

                    outstr = currentPeriod - 1 & ","
                    outstr &= inumber & ","
                    outstr &= i & ","
                    outstr &= getMyColorName(inumber) & ","
                    outstr &= getMyColorName(i) & ","

                    'A person
                    outstr &= dataUnitsSentPerson(i, currentPeriod - 1) + dataUnitsSentPot(i, currentPeriod - 1) & ","
                    outstr &= dataUnitsTakenPerson(i, currentPeriod - 1) + dataUnitsTakenPot(i, currentPeriod - 1) & ","
                    outstr &= dataUnitsTakenCount(i, currentPeriod - 1) & ","
                    outstr &= calcTotalSentToPlayer(i) - (dataUnitsSentPerson(i, currentPeriod - 1) + dataUnitsSentPot(i, currentPeriod - 1)) & ","
                    outstr &= calcTotalTakenFromPlayer(i) - (dataUnitsTakenPerson(i, currentPeriod - 1) + dataUnitsTakenPot(i, currentPeriod - 1)) & ","
                    outstr &= calcTotalTimesTakenFromPlayer(i) - dataUnitsTakenCount(i, currentPeriod - 1) & ","
                    outstr &= dataUnitsCaptured & ","
                    outstr &= dataSide & ","
                    outstr &= currentHealth & ","

                    If dataSide = "left" Then
                        outstr &= dataUnitsCapturedTotalLeft - dataUnitsCaptured & ","
                        outstr &= dataUnitsCapturedTotalRight & ","
                    ElseIf dataSide = "right" Then
                        outstr &= dataUnitsCapturedTotalLeft & ","
                        outstr &= dataUnitsCapturedTotalRight - dataUnitsCaptured & ","
                    Else
                        outstr &= dataUnitsCapturedTotalLeft & ","
                        outstr &= dataUnitsCapturedTotalRight & ","
                    End If

                    outstr &= dataUnitsSentGreaterThan18(i) & ","

                    outstr &= calcSentToOthers(currentPeriod - 1) & ","
                    outstr &= calcTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= calcTimesTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= calcOthersSentToMe(currentPeriod - 1) & ","
                    outstr &= calcOthersTakenFromMe(currentPeriod - 1) & ","
                    outstr &= calcTimesOthersTakenFromMe(currentPeriod - 1) & ","

                    outstr &= calcTotalSentToOthers() - calcSentToOthers(currentPeriod - 1) & ","
                    outstr &= calcTotalTakenFromOthers() - calcTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= calcTotalTimesTakenFromOthers() - calcTimesTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= calcTotalOthersSentToMe() - calcOthersSentToMe(currentPeriod - 1) & ","
                    outstr &= calcTotalOthersTakenFromMe() - calcOthersTakenFromMe(currentPeriod - 1) & ","
                    outstr &= calcTotalTimesOthersTakenFromMe() - calcTimesOthersTakenFromMe(currentPeriod - 1) & ","

                    'B person
                    outstr &= playerList(i).dataUnitsSentPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsSentPot(inumber, currentPeriod - 1) & ","
                    outstr &= playerList(i).dataUnitsTakenPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsTakenPot(inumber, currentPeriod - 1) & ","
                    outstr &= playerList(i).dataUnitsTakenCount(inumber, currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalSentToPlayer(inumber) - (playerList(i).dataUnitsSentPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsSentPot(inumber, currentPeriod - 1)) & ","
                    outstr &= playerList(i).calcTotalTakenFromPlayer(inumber) - (playerList(i).dataUnitsTakenPerson(inumber, currentPeriod - 1) + playerList(i).dataUnitsTakenPot(inumber, currentPeriod - 1)) & ","
                    outstr &= playerList(i).calcTotalTimesTakenFromPlayer(inumber) - playerList(i).dataUnitsTakenCount(inumber, currentPeriod - 1) & ","
                    outstr &= playerList(i).dataUnitsCaptured & ","
                    outstr &= playerList(i).dataSide & ","
                    outstr &= playerList(i).currentHealth & ","

                    If playerList(i).dataSide = "left" Then
                        outstr &= playerList(i).dataUnitsCapturedTotalLeft - playerList(i).dataUnitsCaptured & ","
                        outstr &= playerList(i).dataUnitsCapturedTotalRight & ","
                    ElseIf playerList(i).dataSide = "right" Then
                        outstr &= playerList(i).dataUnitsCapturedTotalLeft & ","
                        outstr &= playerList(i).dataUnitsCapturedTotalRight - playerList(i).dataUnitsCaptured & ","
                    Else
                        outstr &= playerList(i).dataUnitsCapturedTotalLeft & ","
                        outstr &= playerList(i).dataUnitsCapturedTotalRight & ","
                    End If

                    outstr &= playerList(i).dataUnitsSentGreaterThan18(inumber) & ","
                    outstr &= playerList(i).calcSentToOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTimesTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcOthersSentToMe(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcOthersTakenFromMe(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTimesOthersTakenFromMe(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalSentToOthers() - playerList(i).calcSentToOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalTakenFromOthers() - playerList(i).calcTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalTimesTakenFromOthers() - playerList(i).calcTimesTakenFromOthers(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalOthersSentToMe() - playerList(i).calcOthersSentToMe(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalOthersTakenFromMe() - playerList(i).calcOthersTakenFromMe(currentPeriod - 1) & ","
                    outstr &= playerList(i).calcTotalTimesOthersTakenFromMe() - playerList(i).calcTimesOthersTakenFromMe(currentPeriod - 1) & ","

                    If playerList(i).fireX <> -1 And fireX <> -1 Then
                        If returnDistance(fireX, fireY, playerList(i).fireX, playerList(i).fireY) < 700 Then
                            outstr &= "true,"
                        Else
                            outstr &= "false,"
                        End If
                    Else
                        outstr &= "false,"
                    End If

                    'tug info
                    outstr &= healthLost(i, currentPeriod - 1) & ","
                    outstr &= playerList(i).healthLost(inumber, currentPeriod - 1) & ","

                    outstr &= tugsStarted(i, currentPeriod - 1) & ","
                    outstr &= playerList(i).tugsStarted(inumber, currentPeriod - 1) & ","

                    outstr &= tugsHelped(i, currentPeriod - 1) & ","
                    outstr &= playerList(i).tugsHelped(inumber, currentPeriod - 1) & ","

                    outstr &= tugsHurt(i, currentPeriod - 1) & ","
                    outstr &= playerList(i).tugsHurt(inumber, currentPeriod - 1) & ","

                    outstr &= healthLostForPerson(i, currentPeriod - 1) & ","
                    outstr &= playerList(i).healthLostForPerson(inumber, currentPeriod - 1) & ","

                    pairwiseDF.WriteLine(outstr)
                End If
            Next



        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub writeEventData(ByVal eventName As String, ByVal eventData As String)
        Try
            '"Period,Phase,GameTime,SecondsSinceStart,Player,XLoc,YLoc,EventName,ChatText,BlueNear,RedNear,YellowNear,GreenNear," & _
            '       "PurpleNear,OrangeNear,BrownNear,TealNear,CaptureType,CaptureResult,DestinationX,DestinationY,PotX,PotY,TransferType,TransferTarget,TransferAmount,"

            If showInstructions Then Exit Sub

            Dim outstr As String = ""
            Dim commaOffset As String = ""
            Dim chatOffset As String = ""

            For i As Integer = 1 To numberOfPlayers
                chatOffset &= ","
            Next

            Select Case eventName
                Case "Capture"
                    commaOffset = "," & chatOffset
                Case "Chat"
                    commaOffset = ""
                Case "New Destination"
                    commaOffset = ",,," & chatOffset
                Case "Place Pot"
                    commaOffset = ",,,,," & chatOffset
                Case "Transfer Units"
                    commaOffset = ",,,,,,," & chatOffset
                Case Else
                    commaOffset = ",,,,,,,," & chatOffset
            End Select

            Dim tS As TimeSpan = Now - startTime

            outstr = currentPeriod & ","
            outstr &= phase & ","
            outstr &= timeRemaining & ","
            outstr &= tS.TotalSeconds & ","
            outstr &= getMyColorName(inumber) & ","
            outstr &= myGroup & ","
            outstr &= myX & ","
            outstr &= myY & ","
            outstr &= eventName & ","
            outstr &= commaOffset
            outstr &= eventData & ","

            eventsDf.WriteLine(outstr)

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub writeReplayData()
        Try
            'str = "Period,Phase,Time,Player,XLoc,YLoc,XFire,YFire,PullingIn,PreyTarget,SmallPreyCount,PreyInfo"
            If showInstructions Then Exit Sub

            Dim outstr As String = ""

            outstr = currentPeriod & ","
            outstr &= phase & ","
            outstr &= timeRemaining & ","
            outstr &= inumber & ","
            outstr &= myX & ","
            outstr &= myY & ","
            outstr &= fireX & ","
            outstr &= fireY & ","
            outstr &= currentHealth & ","
            outstr &= unitsPerson & ","
            outstr &= unitsPot & ","

            outstr &= tugging & ","
            outstr &= tugger & ","
            outstr &= tugTarget & ","
            outstr &= tugAmount & ","
            outstr &= tugOpponet & ","
            outstr &= tugHelping & ","
            outstr &= tugHelpTarget & ","

            outstr &= pullingIn & ","
            outstr &= preyTarget & ","

            If phase = "hunting" Then
                outstr &= smallPreyPerPeriod & ","

                For i As Integer = 1 To smallPreyPerPeriod
                    outstr &= smallPrey(i).myX & ","
                    outstr &= smallPrey(i).myY & ","
                    outstr &= smallPrey(i).status & ","
                Next

                For i As Integer = 1 To largePreyPerPeriod
                    outstr &= largePrey(i).myX & ","
                    outstr &= largePrey(i).myY & ","
                    outstr &= largePrey(i).status & ","
                Next
            End If

            replayDf.WriteLine(outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub finishedInstructions()
        Try
            With frmServer
                Dim outstr As String = ""

                unitsPerson = 0
                unitsPot = 0

                fireX = -1
                fireY = -1

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).myX & ";"
                    outstr &= playerList(i).myY & ";"
                Next

                'reset data collection
                dataSide = ""
                dataUnitsCaptured = 0
                dataUnitsPerson = 0
                dataUnitsPot = 0
                wordCount = 0
                dataUnitsCapturedTotalLeft = 0
                dataUnitsCapturedTotalRight = 0

                For i As Integer = 1 To numberOfPlayers
                    For j As Integer = 1 To 100
                        dataUnitsSentPerson(i, j) = 0
                        dataUnitsSentPot(i, j) = 0
                        dataUnitsTakenPerson(i, j) = 0
                        dataUnitsTakenPot(i, j) = 0
                        dataUnitsTakenCount(i, j) = 0
                        dataHitCount(i, j) = 0
                    Next

                    dataUnitsSentGreaterThan18(i) = 0
                Next
                dataFailedCaptures = 0

                healthAtPeriodStart = currentHealth


                .wsk_Col.Send("13", socketNumber, outstr)

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function isOverPlayer(ByVal tempX As Integer, ByVal tempY As Integer) As Boolean
        Try
            Dim XOffset As Integer = 0
            If phase = "hunting" Then
                XOffset = 0
            Else
                XOffset = 1680 * 2
            End If

            With frmServer
                If tempX >= (myX - XOffset) * .xScreenAdj - 20 And tempX < (myX - XOffset) * .xScreenAdj + 20 And _
                   tempY >= myY * .yScreenAdj - 20 And tempY < myY * .yScreenAdj + 20 Then

                    Return True
                Else
                    Return False
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return False
        End Try
    End Function

    Public Sub unStick(ByVal index As Integer)
        Try
            With frmServer
                Dim outstr As String = index & ";"

                .wsk_Col.Send("15", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendTransfer(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim outstr As String = ""
                outstr &= timeRemaining & ";"
                outstr &= currentPeriod & ";"
                outstr &= phase & ";"
                outstr &= currentHealth & ";"

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).unitsPerson & ";"
                    outstr &= playerList(i).unitsPot & ";"
                    outstr &= playerList(i).stunned & ";"
                    outstr &= playerList(i).stunning & ";"
                    outstr &= playerList(i).coolDown & ";"
                Next

                outstr &= sinstr
                outstr &= index & ";"

                .wsk_Col.Send("16", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendStun(source As Integer, target As Integer)
        Try
            With frmServer
                Dim outstr As String = ""

                outstr = source & ";"
                outstr &= target & ";"

                .wsk_Col.Send("17", socketNumber, outstr)

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendHit(source As Integer, target As Integer)
        Try
            With frmServer
                Dim outstr As String = ""

                outstr = source & ";"
                outstr &= target & ";"

                For i As Integer = 1 To numberOfPlayers
                    outstr &= playerList(i).stunned & ";"
                    outstr &= playerList(i).stunning & ";"
                    outstr &= playerList(i).coolDown & ";"
                    outstr &= playerList(i).currentHealth & ";"
                Next

                .wsk_Col.Send("18", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function calcTotalSentToOthers() As Integer
        Try
            Dim tempN As Integer = 0

            For j As Integer = 1 To numberOfPlayers
                If j <> inumber Then
                    For i As Integer = 1 To 100
                        tempN += dataUnitsSentPerson(j, i)
                        tempN += dataUnitsSentPot(j, i)
                    Next
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalOthersSentToMe() As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).calcTotalSentToPlayer(inumber)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcSentToOthers(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += dataUnitsSentPerson(i, tempPeriod)
                    tempN += dataUnitsSentPot(i, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcOthersSentToMe(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).dataUnitsSentPerson(inumber, tempPeriod)
                    tempN += playerList(i).dataUnitsSentPot(inumber, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalSentToPlayer(tempP As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To 100
                tempN += dataUnitsSentPerson(tempP, i)
                tempN += dataUnitsSentPot(tempP, i)
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalTakenFromOthers() As Integer
        Try
            Dim tempN As Integer = 0

            For j As Integer = 1 To numberOfPlayers
                If j <> inumber Then
                    For i As Integer = 1 To 100
                        tempN += dataUnitsTakenPerson(j, i)
                        tempN += dataUnitsTakenPot(j, i)
                    Next
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalOthersTakenFromMe() As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).calcTotalTakenFromPlayer(inumber)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTakenFromOthers(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += dataUnitsTakenPerson(i, tempPeriod)
                    tempN += dataUnitsTakenPot(i, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTimesTakenFromOthers(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += dataUnitsTakenCount(i, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalTimesTakenFromOthers() As Integer
        Try
            Dim tempN As Integer = 0

            For j As Integer = 1 To numberOfPlayers
                If j <> inumber Then
                    For i As Integer = 1 To 100
                        tempN += playerList(inumber).dataUnitsTakenCount(j, i)
                    Next
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTimesOthersTakenFromMe(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).dataUnitsTakenCount(inumber, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalTimesOthersTakenFromMe() As Integer
        Try
            Dim tempN As Integer = 0

            For j As Integer = 1 To numberOfPlayers
                If j <> inumber Then
                    For i As Integer = 1 To 100
                        tempN += playerList(j).dataUnitsTakenCount(inumber, i)
                    Next
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function


    Public Function calcTimesHitOthers(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += dataHitCount(i, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTimesOthersHitMe(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).dataHitCount(inumber, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcOthersTakenFromMe(tempPeriod As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To numberOfPlayers
                If i <> inumber Then

                    tempN += playerList(i).dataUnitsTakenPerson(inumber, tempPeriod)
                    tempN += playerList(i).dataUnitsTakenPot(inumber, tempPeriod)
                End If
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalTakenFromPlayer(tempP As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To 100
                tempN += dataUnitsTakenPerson(tempP, i)
                tempN += dataUnitsTakenPot(tempP, i)
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Function calcTotalTimesTakenFromPlayer(tempP As Integer) As Integer
        Try
            Dim tempN As Integer = 0

            For i As Integer = 1 To 100
                tempN += dataUnitsTakenCount(tempP, i)
            Next

            Return tempN
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 0
        End Try
    End Function

    Public Sub sendStartTug(outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("19", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendYieldTug(outstr)
        Try
            With frmServer
                .wsk_Col.Send("20", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendHelpTug(outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("21", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub sendCancelHelpTug(outstr As String)
        Try
            With frmServer
                .wsk_Col.Send("22", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function getTugCount() As Integer
        Try
            Dim tempTugAmount As Integer = 1

            For j As Integer = 1 To numberOfPlayers
                If playerList(j).tugHelping And playerList(j).tugHelpTarget = tugOpponet Then
                    tempTugAmount += 1
                End If
            Next

            Return tempTugAmount
        Catch ex As Exception
            appEventLog_Write("error :", ex)
            Return 1
        End Try
    End Function
End Class
