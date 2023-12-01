Public Class frmReplay

    Public replayDfData() As String
    Public summaryDfData() As String
    Public eventsDfData() As String

    Public dataPointer As Integer
    Public dataPointerOffset As Integer

    Private Sub cmdLoadData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadData.Click
        Try
            With frmServer
                Dim sinstr As String = ""

                OpenFileDialog1.FileName = ""
                OpenFileDialog1.Filter = "Data Files (*.csv)|*.csv"
                OpenFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath

                OpenFileDialog1.ShowDialog()

                If OpenFileDialog1.FileName = "" Then
                    Exit Sub
                End If

                sinstr = OpenFileDialog1.FileName

                Dim d(5) As String
                d(0) = "HG_Replay_"
                d(1) = "HG_Period_Summary_"
                d(2) = "HG_Events_"

                Dim msgtokens2() As String = OpenFileDialog1.FileName.Split(d, StringSplitOptions.RemoveEmptyEntries)

                Dim tempFileName As String
                'read player data
                tempFileName = msgtokens2(0) & "HG_Replay_" & msgtokens2(1)
                replayDfData = My.Computer.FileSystem.ReadAllText(tempFileName).Split(vbCrLf)

                'read shrub data
                tempFileName = msgtokens2(0) & "HG_Period_Summary_" & msgtokens2(1)
                summaryDfData = My.Computer.FileSystem.ReadAllText(tempFileName).Split(vbCrLf)

                'read action data
                tempFileName = msgtokens2(0) & "HG_Events_" & msgtokens2(1)
                eventsDfData = My.Computer.FileSystem.ReadAllText(tempFileName).Split(vbCrLf)

                'inital setup
                Dim msgtokens() As String

                msgtokens = replayDfData(0).Split(",")
                numberOfPlayers = msgtokens(1)

                msgtokens = replayDfData(1).Split(",")
                numberOfPeriods = msgtokens(1)

                msgtokens = replayDfData(2).Split(",")
                huntingLength = msgtokens(1)

                msgtokens = replayDfData(3).Split(",")
                tradingLength = msgtokens(1)

                msgtokens = replayDfData(4).Split(",")
                interimLength = msgtokens(1)

                msgtokens = replayDfData(5).Split(",")
                largePreyPerPeriod = msgtokens(1)

                msgtokens = replayDfData(6).Split(",")
                restPeriodFrequency = msgtokens(1)

                msgtokens = replayDfData(7).Split(",")
                restPeriodLength = msgtokens(1)

                msgtokens = replayDfData(8).Split(",")
                tugOfWar = msgtokens(1)

                msgtokens = replayDfData(9).Split(",")
                smallPreyAvailable = msgtokens(1)

                msgtokens = replayDfData(10).Split(",")
                potsAvailable = msgtokens(1)

                msgtokens = replayDfData(11).Split(",")
                bushCount = msgtokens(1)

                Dim tempN As Integer = 12
                For i As Integer = 1 To bushCount
                    msgtokens = replayDfData(tempN).Split(",")

                    bushLocations(i) = New Point(msgtokens(0), msgtokens(1))
                    treeLocations(i) = New Point(msgtokens(2), msgtokens(3))
                    rockLocations(i) = New Point(msgtokens(4), msgtokens(5))

                    tempN += 1
                Next

                tempN += 1

                dataPointer = tempN
                dataPointerOffset = tempN

                For i As Integer = 1 To numberOfPlayers
                    playerList(i) = New player()
                Next

                loadNextSecond("second")

                .Timer2.Enabled = True

                For i As Integer = 1 To numberOfPlayers
                    .rbPrey(i).Enabled = True
                Next

                For i As Integer = numberOfPlayers + 1 To 8
                    .rbPrey(i).Enabled = False
                Next

                cmdPlayData.Enabled = True
                cmdPauseData.Enabled = False

                tbData.Maximum = (numberOfPeriods * (huntingLength + tradingLength + interimLength)) + ((numberOfPeriods \ restPeriodFrequency) * restPeriodLength)
                tbData.Minimum = 1
                tbData.Value = 1

                tbData.Enabled = True
                .RichTextBox1.Text = ""
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdPlayData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPlayData.Click
        Try
            Timer1.Enabled = True
            cmdPlayData.Enabled = False
            cmdPauseData.Enabled = True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdPauseData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPauseData.Click
        Try
            Timer1.Enabled = False
            cmdPlayData.Enabled = True
            cmdPauseData.Enabled = False
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub tbData_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbData.Scroll
        Try
            Application.DoEvents()

            dataPointer = tbData.Value * numberOfPlayers + dataPointerOffset - numberOfPlayers
            loadNextSecond("full")

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub txtSpeed_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSpeed.SelectedItemChanged
        Try
            If txtSpeed.Text = "1x" Then
                Timer1.Interval = 1000
            ElseIf txtSpeed.Text = "2x" Then
                Timer1.Interval = 500
            ElseIf txtSpeed.Text = "3x" Then
                Timer1.Interval = 333
            ElseIf txtSpeed.Text = "4x" Then
                Timer1.Interval = 250
            End If
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            With frmMain
                loadNextSecond("second")

                If tbData.Value = tbData.Maximum Then
                    Timer1.Enabled = False
                Else
                    tbData.Value += 1
                End If

            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub loadNextSecond(ByVal chatData As String)
        Try
            With frmServer
                Dim msgtokens() As String

                'update player/prey
                For i As Integer = 1 To numberOfPlayers

                    If dataPointer > replayDfData.Length Then Exit Sub

                    msgtokens = replayDfData(dataPointer).Split(",")

                    If msgtokens.Length = 1 Then Exit Sub

                    If i = 1 Then
                        currentPeriod = msgtokens(0)
                        phase = msgtokens(1)
                        timeRemaining = msgtokens(2)

                        .txtPeriod.Text = currentPeriod
                        .txtPhase.Text = phase
                        .txtTime.Text = timeConversion(timeRemaining)
                    End If

                    Dim nextToken As Integer = 4

                    playerList(i).myX = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).myY = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).fireX = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).fireY = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).currentHealth = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).unitsPerson = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).unitsPot = msgtokens(nextToken)
                    nextToken += 1

                    'tugging
                    playerList(i).tugging = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugger = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugTarget = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugAmount = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugOpponet = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugHelping = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).tugHelpTarget = msgtokens(nextToken)
                    nextToken += 1

                    'prey info
                    playerList(i).pullingIn = msgtokens(nextToken)
                    nextToken += 1

                    playerList(i).preyTarget = msgtokens(nextToken)
                    nextToken += 1

                    If i = 1 Then
                        smallPreyPerPeriod = playerList(i).mySmallPreyCount
                    End If

                    If phase = "hunting" Then

                        playerList(i).mySmallPreyCount = msgtokens(nextToken)
                        nextToken += 1

                        For j As Integer = 1 To playerList(i).mySmallPreyCount
                            playerList(i).smallPrey(j).myX = msgtokens(nextToken)
                            nextToken += 1

                            playerList(i).smallPrey(j).myY = msgtokens(nextToken)
                            nextToken += 1

                            playerList(i).smallPrey(j).status = msgtokens(nextToken)
                            nextToken += 1
                        Next

                        For j As Integer = playerList(i).mySmallPreyCount + 1 To 200
                            playerList(i).smallPrey(j).status = "out"
                        Next

                        For j As Integer = 1 To largePreyPerPeriod
                            playerList(i).largePrey(j).myX = msgtokens(nextToken)
                            nextToken += 1

                            playerList(i).largePrey(j).myY = msgtokens(nextToken)
                            nextToken += 1

                            playerList(i).largePrey(j).status = msgtokens(nextToken)
                            nextToken += 1
                        Next
                    Else
                        For j As Integer = 1 To 200
                            playerList(i).smallPrey(j).status = "out"
                        Next

                        For j As Integer = 1 To largePreyPerPeriod
                            playerList(i).largePrey(j).status = "out"
                        Next
                    End If

                    dataPointer += 1
                Next

                'update chat/transfers

                If chatData = "full" Then .RichTextBox1.Text = ""

                For i As Integer = 2 To eventsDfData.Length
                    msgtokens = eventsDfData(i - 1).Split(",")

                    Dim tempS As Boolean = False

                    If msgtokens.Length > 1 Then
                        If chatData = "second" Then
                            If msgtokens(0).Trim = .txtPeriod.Text And _
                               msgtokens(1).Trim = .txtPhase.Text And _
                              CInt(msgtokens(2).Trim) = timeRemaining Then

                                tempS = True
                            End If
                        Else
                            If CInt(msgtokens(0).Trim) < currentPeriod Then
                                tempS = True
                            Else
                                If CInt(msgtokens(0).Trim) = currentPeriod Then
                                    If .txtPhase.Text = "interim" And (msgtokens(1) = "hunting" Or msgtokens(1) = "trading") Then
                                        tempS = True
                                    ElseIf phase = "trading" And msgtokens(1) = "hunting" Then
                                        tempS = True
                                    ElseIf .txtPhase.Text = msgtokens(1) Then
                                        If CInt(msgtokens(2)) >= timeRemaining Then
                                            tempS = True
                                        End If
                                    End If
                                End If
                            End If

                        End If

                        If tempS Then
                            If msgtokens(8) = "Chat" Then

                                If frmServer.rbPrey(colorToId(msgtokens(4))).Checked Then

                                    .RichTextBox1.SelectionLength = 0
                                    .RichTextBox1.SelectionStart = .RichTextBox1.TextLength

                                    .RichTextBox1.SelectionColor = getMyColor(colorToId(msgtokens(4)))
                                    .RichTextBox1.SelectedText &= msgtokens(4)

                                    .RichTextBox1.SelectionLength = 0
                                    .RichTextBox1.SelectionColor = Color.Black
                                    .RichTextBox1.SelectedText = ": " & msgtokens(9) & vbCrLf

                                    .RichTextBox1.ScrollToCaret()
                                End If
                            ElseIf msgtokens(8) = "Transfer Units" Then
                                    If chatData = "second" Then
                                    Dim tempIndex As Integer = colorToId(msgtokens(4))
                                    If msgtokens(16 + numberOfPlayers) = "Me -> Pot" Then
                                        Dim tempPotOwner As Integer = colorToId(msgtokens(16 + numberOfPlayers))

                                        playerList(tempIndex).transferColor = Color.White

                                        playerList(tempIndex).transferSource = New Point(playerList(tempIndex).myX, playerList(tempIndex).myY)
                                        playerList(tempIndex).transferTarget = New Point(playerList(tempPotOwner).fireX, playerList(tempPotOwner).fireY)
                                    ElseIf msgtokens(16 + numberOfPlayers) = "Pot -> Me" Then
                                        Dim tempPotOwner As Integer = colorToId(msgtokens(17 + numberOfPlayers))

                                        If tempIndex <> tempPotOwner Then
                                            playerList(tempIndex).transferColor = Color.Maroon
                                        Else
                                            playerList(tempIndex).transferColor = Color.White
                                        End If

                                        playerList(tempIndex).transferSource = New Point(playerList(tempPotOwner).fireX, playerList(tempPotOwner).fireY)
                                        playerList(tempIndex).transferTarget = New Point(playerList(tempIndex).myX, playerList(tempIndex).myY)
                                    ElseIf InStr(msgtokens(16 + numberOfPlayers), "Me -> ") > 0 Then
                                        Dim msgtokens2() As String = msgtokens(16 + numberOfPlayers).Split(" ")
                                        Dim tempPlayer As Integer = colorToId(msgtokens2(2))

                                        playerList(tempIndex).transferSource = New Point(playerList(tempIndex).myX, playerList(tempIndex).myY)
                                        playerList(tempIndex).transferTarget = New Point(playerList(tempPlayer).myX, playerList(tempPlayer).myY)
                                    ElseIf InStr(msgtokens(16 + numberOfPlayers), " -> Me") > 0 Then
                                        Dim msgtokens2() As String = msgtokens(16 + numberOfPlayers).Split(" ")
                                        Dim tempPlayer As Integer = colorToId(msgtokens2(0))
                                        playerList(tempIndex).transferSource = New Point(playerList(tempPlayer).myX, playerList(tempPlayer).myY)
                                        playerList(tempIndex).transferTarget = New Point(playerList(tempIndex).myX, playerList(tempIndex).myY)
                                        playerList(tempIndex).transferColor = Color.Maroon
                                    End If

                                    playerList(tempIndex).transferOpacity = 250
                                End If
                            End If

                        End If
                    End If
                Next


            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmReplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            txtSpeed.SelectedIndex = 3
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class