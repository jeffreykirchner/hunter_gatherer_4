Imports System
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.IO

Public Class frmMain
#Region " Winsock Code "
    Public WithEvents wsk_Col As New WinsockCollection
    Private _users As New UserCollection
    Public WithEvents wskListener As Winsock

    Private Sub wskListener_ConnectionRequest(ByVal sender As Object, ByVal e As WinsockClientReceivedEventArgs) Handles wskListener.ConnectionRequest
        Try
            'Log("Connection received from: " & e.ClientIP)
            Dim y As New clsUser
            Dim i As Integer
            Dim ID As String = connectionCount + 1

            connectionCount += 1

            _users.Add(y)
            Dim x As New Winsock(Me)
            wsk_Col.Add(x, ID)
            x.Accept(e.Client)

            If cmdBegin.Enabled = False Then
                For i = 1 To playerCount
                    If playerList(i).myIPAddress = e.ClientIP Then
                        playerList(i).socketNumber = ID 'wsk_Col.Count - 1
                        Exit For
                    End If
                Next

                Exit Sub
            End If

            playerCount += 1
            playerList(playerCount) = New player()
            playerList(playerCount).inumber = playerCount
            playerList(playerCount).socketNumber = ID 'wsk_Col.Count - 1
            playerList(playerCount).myIPAddress = e.ClientIP

            playerList(playerCount).requsetIP(playerCount)

            lblConnections.Text = CInt(lblConnections.Text) + 1

            'appEventLog_Write("connection request: " & e.ClientIP)
        Catch ex As Exception
            appEventLog_Write("error wskListener_ConnectionRequest:", ex)
        End Try
    End Sub

    Private Sub wskListener_ErrorReceived(ByVal sender As System.Object, ByVal e As WinsockErrorEventArgs) Handles wskListener.ErrorReceived
        Try
            appEventLog_Write("winsock error: " & e.Message)
        Catch ex As Exception
            appEventLog_Write("error wskListener_ErrorReceived:", ex)
        End Try
    End Sub

    Private Sub wskListener_StateChanged(ByVal sender As Object, ByVal e As WinsockStateChangingEventArgs) Handles wskListener.StateChanged
        'Log("Listener state changed from " & e.Old_State.ToString & " to " & e.New_State.ToString)
        'lblListenState.Text = "State: " & e.New_State.ToString
        'cmdListen.Enabled = False
        'cmdClose.Enabled = False
        'Select Case e.New_State
        '    Case WinsockStates.Closed
        '        cmdListen.Enabled = True
        '    Case WinsockStates.Listening
        '        cmdClose.Enabled = True
        'End Select
    End Sub

    'Private Sub Log(ByVal val As String)
    '    lstLog.SelectedIndex = lstLog.Items.Add(val)
    '    lstLog.SelectedIndex = -1
    'End Sub

    Private Sub Wsk_DataArrival(ByVal sender As Object, ByVal e As WinsockDataArrivalEventArgs) Handles wsk_Col.DataArrival
        Try
            Dim sender_key As String = wsk_Col.GetKey(sender)
            Dim buf As String = Nothing
            CType(sender, Winsock).Get(buf)

            Dim msgtokens() As String = buf.Split("#")
            Dim i As Integer

            'appEventLog_Write("data arrival: " & buf)

            For i = 1 To msgtokens.Length - 1
                takeMessage(msgtokens(i - 1))
            Next

        Catch ex As Exception
            appEventLog_Write("error Wsk_DataArrival:", ex)
        End Try
    End Sub

    Private Sub Wsk_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsk_Col.Disconnected
        Try
            wsk_Col.Remove(sender)
            If cmdBegin.Enabled Then Exit Sub
            MsgBox("A client has been disconnected.", MsgBoxStyle.Critical)
            appEventLog_Write("client disconnected")
            'lblConNum.Text = "Connected: " & wsk_Col.Count
        Catch ex As Exception
            appEventLog_Write("error Wsk_Disconnected:", ex)
        End Try
    End Sub
    Private Sub Wsk_Connected(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsk_Col.Connected
        'lblConNum.Text = "Connected: " & wsk_Col.Count
    End Sub

    Private Sub ShutDownServer()
        Try
            GC.Collect()
        Catch ex As Exception
            appEventLog_Write("error ShutDownServer:", ex)
        End Try

    End Sub
#End Region    'communication code

#Region " Extra Functions "
    Public Function convertY(ByVal p As Integer, ByVal graphMin As Integer, ByVal graphMax As Integer, _
                                 ByVal panelHeight As Integer, ByVal bottomOffset As Integer, ByVal topOffset As Integer) As Double
        Try
            Dim tempD As Double

            tempD = p - graphMin

            tempD = (tempD * (panelHeight - bottomOffset - topOffset)) / (graphMax - graphMin)
            tempD = panelHeight - (bottomOffset + topOffset) - tempD

            convertY = tempD + topOffset
        Catch ex As Exception
            appEventLog_Write("error convertY:", ex)
            Return 0
        End Try
    End Function

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Try

            If PrintDialog1.ShowDialog = DialogResult.OK Then
                PrintDocument1.Print()
            End If

        Catch ex As Exception
            appEventLog_Write("error cmdPrint_Click:", ex)
        End Try
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Try
            Dim i As Integer
            Dim f As New Font("Arial", 8, FontStyle.Bold)
            Dim tempN As Integer

            e.Graphics.DrawString(filename2, f, Brushes.Black, 10, 10)

            f = New Font("Arial", 15, FontStyle.Bold)

            e.Graphics.DrawString("Name", f, Brushes.Black, 10, 30)
            e.Graphics.DrawString("Earnings", f, Brushes.Black, 400, 30)

            f = New Font("Arial", 12, FontStyle.Bold)

            tempN = 55

            For i = 1 To DataGridView1.RowCount
                If i Mod 2 = 0 Then
                    e.Graphics.FillRectangle(Brushes.Aqua, 0, tempN, 500, 19)
                End If
                e.Graphics.DrawString(DataGridView1.Rows(i - 1).Cells(1).Value, f, Brushes.Black, 10, tempN)
                e.Graphics.DrawString(DataGridView1.Rows(i - 1).Cells(3).Value, f, Brushes.Black, 400, tempN)

                tempN += 20
            Next

        Catch ex As Exception
            appEventLog_Write("error PrintDocument1_PrintPage:", ex)
        End Try

    End Sub
#End Region

    Public tempTime As String 'time stamp at start of experiment
    Private mainScreen As Screen
    Public xScreenAdj As Double
    Public yScreenAdj As Double

    Public imgBush As Image = Image.FromFile(System.Windows.Forms.Application.StartupPath & "\graphics\bush1.png")
    Public imgTree As Image = Image.FromFile(System.Windows.Forms.Application.StartupPath & "\graphics\tree1.png")
    Public imgRock As Image = Image.FromFile(System.Windows.Forms.Application.StartupPath & "\graphics\moutain.png")

    Public rbPrey(8) As CheckBox
    Public topMostClient As Integer = 1

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            sfile = System.Windows.Forms.Application.StartupPath & "\server.ini"
            loadParameters()

            Dim i As Integer           ', j As Integer

            For i = 1 To 4
                DataGridView1.Columns(i - 1).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            'setup communication on load
            wskListener = New Winsock
            wskListener.BufferSize = 8192
            wskListener.LegacySupport = False
            wskListener.LocalPort = portNumber
            wskListener.MaxPendingConnections = 1
            wskListener.Protocol = WinsockProtocols.Tcp
            wskListener.RemotePort = 8080
            wskListener.RemoteServer = "localhost"
            wskListener.SynchronizingObject = Me

            wskListener.Listen()

            playerCount = 0

            lblIP.Text = wskListener.LocalIP
            lblLocalHost.Text = SystemInformation.ComputerName

            Dim r As New System.Drawing.Rectangle(0, 0, Panel2.Width, Panel2.Height)
            mainScreen = New Screen(Panel2, r)

            xScreenAdj = Panel2.Width / (1680 * 6)
            yScreenAdj = Panel2.Height / (1050 * 6)

            rbPrey(1) = CheckBox1
            rbPrey(2) = CheckBox2
            rbPrey(3) = CheckBox3
            rbPrey(4) = CheckBox4
            rbPrey(5) = CheckBox5
            rbPrey(6) = CheckBox6
            rbPrey(7) = CheckBox7
            rbPrey(8) = CheckBox8

        Catch ex As Exception
            appEventLog_Write("error frmSvr_Load:", ex)
        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If Not showInstructions Then
                For i As Integer = 1 To numberOfPlayers
                    playerList(i).writeReplayData()
                Next

                'stuns
                For i As Integer = 1 To numberOfPlayers
                    If playerList(i).stunned > 0 Then
                        playerList(i).stunned -= 1
                    End If

                    If playerList(i).coolDown > 0 Then
                        playerList(i).coolDown -= 1
                    End If
                Next

                'tugs
                Dim tugYeildCount As Integer = 0
                Dim tugYeildList As String = ""

                If timeRemaining > 1 Then
                    For i As Integer = 1 To numberOfPlayers
                        If playerList(i).tugging Then

                            Dim tempTugAmount As Integer = playerList(i).getTugCount

                            For j As Integer = 1 To numberOfPlayers
                                If playerList(j).tugHelping And playerList(j).tugHelpTarget = i Then
                                    playerList(j).currentHealth -= (tempTugAmount * tugOfWarCost)

                                    playerList(j).healthLost(playerList(i).tugOpponet, currentPeriod) += (tempTugAmount * tugOfWarCost)
                                    playerList(j).healthLostForPerson(i, currentPeriod) += (tempTugAmount * tugOfWarCost)

                                    If playerList(j).currentHealth < 0 Then

                                        playerList(j).currentHealth = 0
                                        playerList(j).tugHelping = False
                                    End If

                                End If
                            Next

                            playerList(i).healthLost(playerList(i).tugOpponet, currentPeriod) += (tempTugAmount * tugOfWarCost)

                            playerList(i).currentHealth -= (tempTugAmount * tugOfWarCost)

                            If playerList(i).currentHealth < 0 Then
                                playerList(i).currentHealth = 0

                                If (playerList(playerList(i).tugOpponet).currentHealth = 0 Or
                                   (playerList(playerList(i).tugOpponet).currentHealth - (playerList(playerList(i).tugOpponet).getTugCount * tugOfWarCost) <= 0 And
                                   playerList(i).tugOpponet > i)) And
                                   playerList(playerList(i).tugOpponet).tugger = playerList(i).tugOpponet Then

                                    'do not yeild if tugger will hit zero on the same period
                                Else
                                    tugYeildCount += 1
                                    tugYeildList &= calcYeildTug("", i) & ";"
                                End If

                            End If
                        End If
                    Next
                End If

                For i As Integer = 1 To numberOfPlayers
                    If playerList(i).stunning > 0 Then
                        If playerList(playerList(i).stunning).stunned = 0 Then
                            playerList(i).stunning = 0
                            playerList(i).coolDown = interactionCoolDown
                        End If
                    End If
                Next

                If timeRemaining = 1 Then
                    If phase = "hunting" Then
                        timeRemaining = tradingLength
                        phase = "trading"

                        For i As Integer = 1 To numberOfPlayers
                            playerList(i).updateTime("0")
                        Next
                    ElseIf phase = "trading" Then

                        For i As Integer = 1 To numberOfPlayers
                            playerList(i).tugging = False
                            playerList(i).tugHelping = False
                        Next

                        If currentPeriod = numberOfPeriods Then
                            currentPeriod += 1

                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).endGame()
                            Next
                            Timer1.Enabled = False
                        ElseIf currentPeriod Mod restPeriodFrequency = 0 Then
                            phase = "rest"
                            timeRemaining = restPeriodLength

                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).startRest()
                            Next
                        Else
                            phase = "interim"
                            timeRemaining = interimLength

                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).startInterim()
                            Next
                        End If
                    ElseIf phase = "rest" Then
                        If currentPeriod = numberOfPeriods Then
                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).endGame()
                            Next
                            Timer1.Enabled = False
                        Else
                            phase = "interim"
                            timeRemaining = interimLength

                            For i As Integer = 1 To numberOfPlayers
                                playerList(i).startInterim()
                            Next
                        End If
                    Else
                        currentPeriod += 1
                        smallPreyPerPeriod = getINI(sfile, "smallPrey", CStr(currentPeriod))

                        timeRemaining = huntingLength
                        phase = "hunting"

                        updateGroups()

                        For i As Integer = 1 To numberOfPlayers
                            playerList(i).writeSummaryData()
                        Next

                        For i As Integer = 1 To numberOfPlayers
                            playerList(i).startNextPeriod()
                        Next
                    End If
                Else
                    timeRemaining -= 1

                    For i As Integer = 1 To numberOfPlayers
                        playerList(i).updateTime(tugYeildCount & ";" & tugYeildList)
                    Next
                End If

                txtPeriod.Text = currentPeriod
                txtTime.Text = timeConversion(timeRemaining)
                txtPhase.Text = phase
            Else
                For i As Integer = 1 To numberOfPlayers
                    playerList(i).updateTime("0")
                Next
            End If
        Catch ex As Exception
            appEventLog_Write("error Timer1_Tick:", ex)
        End Try
    End Sub

    Private Sub cmdBegin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBegin.Click
        Try

            'when a button is pressed it's click event is fired

            loadParameters()

            Dim nextToken As Integer = 0
            Dim str As String

            If CInt(lblConnections.Text) <> numberOfPlayers Then
                MsgBox("Incorrect number of connections.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'define timestamp for recording data
            tempTime = DateTime.Now.Month & "-" & DateTime.Now.Day & "-" & DateTime.Now.Year & "_" & DateTime.Now.Hour & _
                     "_" & DateTime.Now.Minute & "_" & DateTime.Now.Second

            'create unique file name for storing data
            filename = "HG_Period_Summary_" & tempTime & ".csv" '&  &
            filename = System.Windows.Forms.Application.StartupPath & "\datafiles\" & filename

            summaryDf = File.CreateText(filename)
            str = "Period,Player,Group,Color,Side,UnitsCaptured,UnitsPerson,UnitsPot,FailedCaptures,CurrentHealth,WordCount,Captured>=1Unit,NewHealth,PeriodSQ,"
            For i As Integer = 1 To numberOfPlayers
                str &= "SentToPlayer" & getMyColorName(i) & ","
                str &= "SentToPot" & getMyColorName(i) & ","
                str &= "TakenFromPlayer" & getMyColorName(i) & ","
                str &= "TakenFromPot" & getMyColorName(i) & ","
                str &= "PotNearby" & getMyColorName(i) & ","

                str &= "TotalGivenToPlayer" & getMyColorName(i) & ","
                str &= "TotalReceivedFromPlayer" & getMyColorName(i) & ","

                str &= "TotalAmountTakenFromPlayer" & getMyColorName(i) & ","
                str &= "TotalTimesTakenFromPlayer" & getMyColorName(i) & ","

                str &= "TotalAmountTakenByPlayer" & getMyColorName(i) & ","
                str &= "TotalTimesTakenByPlayer" & getMyColorName(i) & ","

                str &= "NetAmountTakenFrom" & getMyColorName(i) & ","

                str &= "HitsReceivedFrom" & getMyColorName(i) & ","
                str &= "HitsGivenTo" & getMyColorName(i) & ","

                str &= "HealthLostFrom" & getMyColorName(i) & ","
                str &= "HealthTakenFrom" & getMyColorName(i) & ","

                str &= "HealthLostFor" & getMyColorName(i) & ","
                str &= "Health" & getMyColorName(i) & "LostForMe,"

                str &= "TugsStartedAgainst" & getMyColorName(i) & ","
                str &= "TugsStartedFrom" & getMyColorName(i) & ","

                str &= "TugHelpFrom" & getMyColorName(i) & ","
                str &= "TugHelpTo" & getMyColorName(i) & ","

                str &= "TugHurtFrom" & getMyColorName(i) & ","
                str &= "TugHurtTo" & getMyColorName(i) & ","
            Next

            str &= "TotalShared,TotalAmountReceived,TotalAmountTakenByMe,TotalTimesTakenByMe,TotalAmountTakenFromMe,TotalTimesTakenFromMe,TotalHitsGiven,TotalHistReceived,"

            str &= "GroupSize,"
            summaryDf.WriteLine(str)

            'create unique file name for storing events
            filename = "HG_Events_" & tempTime & ".csv" '&  &
            filename = System.Windows.Forms.Application.StartupPath & "\datafiles\" & filename

            eventsDf = File.CreateText(filename)
            str = "Period,Phase,GameTime,SecondsSinceStart,Player,Group,XLoc,YLoc,EventName,ChatText,"

            For i As Integer = 1 To numberOfPlayers
                str &= getMyColorName(i) & "Near,"
            Next

            str &= "CaptureType,CaptureResult,DestinationX,DestinationY,PotX,PotY,TransferType,TransferTarget,TransferAmount,OtherInfo"
            eventsDf.WriteLine(str)

            'create unique file name for storing events
            filename = "HG_Replay_" & tempTime & ".csv" '&  &
            filename = System.Windows.Forms.Application.StartupPath & "\datafiles\" & filename

            replayDf = File.CreateText(filename)
            replayDf.WriteLine("numberOfPlayers," & numberOfPlayers)
            replayDf.WriteLine("numberOfPeriods," & numberOfPeriods)
            replayDf.WriteLine("huntingLength," & huntingLength)
            replayDf.WriteLine("tradingLength," & tradingLength)
            replayDf.WriteLine("interimLength," & interimLength)
            replayDf.WriteLine("largePreyPerPeriod," & largePreyPerPeriod)
            replayDf.WriteLine("restPeriodFrequency," & restPeriodFrequency)
            replayDf.WriteLine("restPeriodLength," & restPeriodLength)
            replayDf.WriteLine("tugOfWar," & tugOfWar)
            replayDf.WriteLine("smallPreyAvailable," & smallPreyAvailable)
            replayDf.WriteLine("potsAvailable," & potsAvailable)

            replayDf.WriteLine("landMarks," & bushCount)
            For i As Integer = 1 To bushCount
                replayDf.WriteLine(getINI(sfile, "bushes", CStr(i)).Replace(";", ","))
            Next

            str = "Period,Phase,Time,Player,XLoc,YLoc,XFire,YFire,Health,UnitsPerson,UnitsPot,Tugging,Tugger,TugTarget,TugAmount,TugOpponet,TugHelping,TugHelpingTarget,PullingIn,PreyTarget,SmallPreyCount,PreyInfo"
            replayDf.WriteLine(str)

            filename = "HG_PairWise_" & tempTime & ".csv" '&  &
            filename = System.Windows.Forms.Application.StartupPath & "\datafiles\" & filename
            pairwiseDF = File.CreateText(filename)

            str = "Period,PlayerA,PlayerB,ColorA,ColorB," & _
                  "AmountSentA->B,AmountTakeA<-B,TimesTakenA<-B,TotalAmountSentA->B,TotalAmountTakeA<-B,TotalTimesTakenA<-B,AmountCapturedA,SideA,HealthA,TotalSideLeftA,TotalSideRightA,TotalTimeSentA->B>18,AmountSentA->Others,AmountTakeA<-Others,TimesTakenA<-Others,AmountSentOthers->A,AmountTakeOthers<-A,TimesTakenOthers<-A,TotalAmountSentA->Others,TotalAmountTakeA<-Others,TotalTimesTakenA<-Others,TotalAmountSentOthers->A,TotalAmountTakeOthers<-A,TotalTimesTakenOthers<-A," & _
                  "AmountSentB->A,AmountTakeB<-A,TimesTakenB<-A,TotalAmountSentB->A,TotalAmountTakeB<-A,TotalTimesTakenB<-A,AmountCapturedB,SideB,HealthB,TotalSideLeftB,TotalSideRightB,TotalTimeSentB->A>18,AmountSentB->Others,AmountTakeB<-Others,TimesTakenB<-Others,AmountSentOthers->B,AmountTakeOthers<-B,TimesTakenOthers<-B,TotalAmountSentB->Others,TotalAmountTakeB<-Others,TotalTimesTakenB<-Others,TotalAmountSentOthers->B,TotalAmountTakeOthers<-B,TotalTimesTakeOthers<-B,APotNearBPot," & _
                  "HealthLostAFromB,HealthLostBFromA,TugsStartedA->B,TugsStartedA<-B,TimesAHelpedBTug,TimesBHelpedATug,TimesAHurtBTug,TimesBHurtATug,HealthALostForB,HealthBLostForA"

            pairwiseDF.WriteLine(str)

            DataGridView1.RowCount = numberOfPlayers

            showInstructions = getINI(sfile, "gameSettings", "showInstructions")

            'setup for display results
            'setup player type
            For i As Integer = 1 To numberOfPlayers
                DataGridView1.Rows(i - 1).Cells(0).Value = i

                playerList(i).earnings = 0
                playerList(i).roundEarnings = 0
                playerList(i).exchangeRate = getINI(sfile, "exchangeRate", CStr(i))

                DataGridView1.Rows(i - 1).Cells(0).Value = i
                DataGridView1.Rows(i - 1).Cells(1).Value = playerList(i).myIPAddress
                If showInstructions Then
                    DataGridView1.Rows(i - 1).Cells(2).Value = "Page 1"
                Else
                    DataGridView1.Rows(i - 1).Cells(2).Value = "Playing"
                End If

                DataGridView1.Rows(i - 1).Cells(3).Value = "0"
                DataGridView1.Rows(i - 1).Cells(4).Value = getMyColorName(i)

                Dim msgtokens() As String = getINI(sfile, "players", CStr(i)).Split(";")
                playerList(i).myX = msgtokens(0)
                playerList(i).myY = msgtokens(1)

                playerList(i).targetX = playerList(i).myX
                playerList(i).targetY = playerList(i).myY

                playerList(i).colorName = getMyColorName(i)
                playerList(i).unitsPerson = 0
                playerList(i).unitsPot = 0

                playerList(i).fireX = -1
                playerList(i).fireY = -1

                playerList(i).currentHealth = startingHealth

                playerList(i).mySmallPreyCount = 0

                playerList(i).tugging = False
                playerList(i).tugHelping = False
            Next

            currentPeriod = 1
            txtPeriod.Text = currentPeriod
            checkin = 0
            phase = "hunting"
            timeRemaining = huntingLength + interimLength

            'disable/enable buttons needed when the experiment starts
            cmdLoad.Enabled = False
            cmdGameSetup.Enabled = False
            cmdExchange.Enabled = False
            cmdSetup2.Enabled = False
            cmdExit.Enabled = False
            cmdBegin.Enabled = False
            cmdEnd.Enabled = True
            cmdExchange.Enabled = False
            cmdSmallPrey.Enabled = False
            cmdPlayerSetup.Enabled = False
            cmdReplay.Enabled = False
            cmdUnstick.Enabled = True
            cmdGroupSetup.Enabled = False

            frmReplay.Close()
            frmSetup.Close()
            frmSetup2.Close()
            frmSetup3.Close()
            frmSetup4.Close()

            filename2 = filename

            showInstructions = getINI(sfile, "gameSettings", "showInstructions")
            smallPreyPerPeriod = getINI(sfile, "smallPrey", "1")

            updateGroups()

            'signal clients to begin
            startTime = Now
            For i As Integer = 1 To numberOfPlayers
                playerList(i).begin()
            Next

            For i As Integer = 1 To bushCount
                Dim msgtokens() As String = getINI(sfile, "bushes", CStr(i)).Split(";")

                bushLocations(i) = New Point(msgtokens(0), msgtokens(1))
                treeLocations(i) = New Point(msgtokens(2), msgtokens(3))
                rockLocations(i) = New Point(msgtokens(4), msgtokens(5))
            Next

            Timer1.Enabled = True
            Timer2.Enabled = True

            For i As Integer = 1 To numberOfPlayers
                rbPrey(i).Enabled = True
            Next

            For i As Integer = numberOfPlayers + 1 To 8
                rbPrey(i).Enabled = False
            Next
        Catch ex As Exception
            appEventLog_Write("error cmdBegin_Click:", ex)
        End Try

    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Try
            'when reset is pressed bring server back to state to start another experiment

            'disable timers
            Timer1.Enabled = False
            Timer2.Enabled = False
            Timer3.Enabled = False

            'close data files
            If summaryDf IsNot Nothing Then summaryDf.Close()
            If eventsDf IsNot Nothing Then eventsDf.Close()
            If replayDf IsNot Nothing Then replayDf.Close()
            If pairwiseDF IsNot Nothing Then pairwiseDF.Close()

            'shut down clients
            Dim i As Integer
            For i = 1 To CInt(lblConnections.Text)
                playerList(i).resetClient()
            Next

            'enable/disable buttons
            cmdLoad.Enabled = True
            cmdGameSetup.Enabled = True
            cmdBegin.Enabled = True
            cmdExit.Enabled = True
            cmdEnd.Enabled = False
            cmdExchange.Enabled = True
            cmdSetup2.Enabled = True
            cmdExchange.Enabled = True
            cmdSmallPrey.Enabled = True
            cmdPlayerSetup.Enabled = True
            cmdReplay.Enabled = True
            cmdUnstick.Enabled = False
            cmdGroupSetup.Enabled = True

            lblConnections.Text = 0
            playerCount = 0

            DataGridView1.RowCount = 0

            frmInstructions.Close()

            RichTextBox1.Text = ""

        Catch ex As Exception
            appEventLog_Write("error cmdReset_Click:", ex)
        End Try
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Try
            'exit program

            Timer1.Enabled = False
            ShutDownServer()

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error cmdExit_Click:", ex)
        End Try
    End Sub

    Private Sub cmdGameSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGameSetup.Click
        Try
            frmSetup.Show()
        Catch ex As Exception
            appEventLog_Write("error cmdGameSetup_Click:", ex)
        End Try
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try
            drawScreen()
        Catch ex As Exception
            appEventLog_Write("error Timer2_Tick:", ex)
        End Try
    End Sub

    Dim f As New Font("Microsoft Sans Serif", 14, FontStyle.Bold) ' The font that the text will be written in
    Dim f2 As New Font("Microsoft Sans Serif", 10, FontStyle.Bold) ' The font that the text will be written in

    Dim p As New Pen(Color.White, 3)
    Dim rect As Rectangle
    Dim fmt As New StringFormat
    Dim StringSize As SizeF

    Public Sub drawScreen()
        Try
            mainScreen.erase1()

            yScreenAdj = Panel2.Height / (1050 * worldHeight)

            p.EndCap = LineCap.ArrowAnchor

            Dim g As Drawing.Graphics = mainScreen.GetGraphics

            'background colors
            If phase = "hunting" And smallPreyAvailable Then
                rect = New Rectangle(0, 0, Panel2.Width / 3, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.IndianRed)), rect)

                rect = New Rectangle(Panel2.Width / 3, 0, Panel2.Width / 3, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.Olive)), rect)

                rect = New Rectangle(Panel2.Width / 3 * 2, 0, Panel2.Width / 3, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.CadetBlue)), rect)

                xScreenAdj = Panel2.Width / (1680 * worldWidth)
            ElseIf phase = "hunting" And Not smallPreyAvailable Then
                rect = New Rectangle(0, 0, Panel2.Width / 2, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.IndianRed)), rect)

                rect = New Rectangle(Panel2.Width / 2, 0, Panel2.Width / 2, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.Olive)), rect)

                xScreenAdj = Panel2.Width / (1680 * worldWidth * (4 / 6))
            Else
                rect = New Rectangle(0, 0, Panel2.Width, Panel2.Height)
                g.FillRectangle(New SolidBrush(Color.FromArgb(225, Color.Olive)), rect)

                xScreenAdj = Panel2.Width / (1680 * worldWidth * (2 / 6))
            End If

            Dim XOffset As Integer = 0
            If phase = "hunting" Then
                XOffset = 0
            Else
                XOffset = 1680 * worldWidth * (2 / 6)
            End If

            For i As Integer = 1 To numberOfPlayers
                If rbPrey(i).Checked Then
                    g.FillRectangle(New SolidBrush(Color.FromArgb(100, getMyColor(i))),
                                New Rectangle((playerList(i).myX - 565 - XOffset) * xScreenAdj,
                                              (playerList(i).myY - 445) * yScreenAdj,
                                              1129 * xScreenAdj,
                                              890 * yScreenAdj))
                End If
            Next

            'draw landmarks
            If landMarks Then
                For i As Integer = 1 To bushCount
                    Dim rectF As New RectangleF(CSng((bushLocations(i).X - XOffset) * xScreenAdj) - (imgBush.Width * 0.25) / 2, _
                                                CSng(bushLocations(i).Y * yScreenAdj) - (imgBush.Height * 0.25) / 2, _
                                                imgBush.Width * 0.25, _
                                                imgBush.Height * 0.25)
                    g.DrawImage(imgBush, rectF)

                    rectF = New RectangleF(CSng((treeLocations(i).X - XOffset) * xScreenAdj) - (imgTree.Width * 0.25) / 2, _
                                                CSng(treeLocations(i).Y * yScreenAdj) - (imgTree.Height * 0.25) / 2, _
                                                imgTree.Width * 0.25, _
                                                imgTree.Height * 0.25)
                    g.DrawImage(imgTree, rectF)

                    rectF = New RectangleF(CSng((rockLocations(i).X - XOffset) * xScreenAdj) - (imgRock.Width * 0.25) / 2, _
                                                CSng(rockLocations(i).Y * yScreenAdj) - (imgRock.Height * 0.25) / 2, _
                                                imgRock.Width * 0.25, _
                                                imgRock.Height * 0.25)
                    g.DrawImage(imgRock, rectF)
                Next
            End If

            'draw pots

            fmt.Alignment = StringAlignment.Center

            StringSize = g.MeasureString("00", f) 'The length of the text

            For i As Integer = 1 To numberOfPlayers
                If playerList(i).fireX <> -1 And rbPrey(i).Checked Then
                    drawTriangle(g, (playerList(i).fireX - XOffset) * xScreenAdj, playerList(i).fireY * yScreenAdj, getMyColor(i), 60, 30)
                    g.DrawString(playerList(i).unitsPot, f, Brushes.Black, _
                                 (playerList(i).fireX - XOffset) * xScreenAdj, playerList(i).fireY * yScreenAdj - 7, fmt)
                End If
            Next

            'draw players
            For i As Integer = 1 To numberOfPlayers
                If rbPrey(i).Checked Then
                    g.FillRectangle(New SolidBrush(getMyColor(i)), CInt((playerList(i).myX - XOffset) * xScreenAdj) - 20, CInt(playerList(i).myY * yScreenAdj) - 20, 40, 40)
                    g.DrawRectangle(Pens.Black, CInt((playerList(i).myX - XOffset) * xScreenAdj) - 20, CInt(playerList(i).myY * yScreenAdj) - 20, 40, 40)

                    g.DrawString(playerList(i).unitsPerson, f, Brushes.Black, _
                                 (playerList(i).myX - XOffset) * xScreenAdj, playerList(i).myY * yScreenAdj - 22, fmt)

                    g.DrawString(Math.Round(playerList(i).currentHealth, 1) & "%", f2, Brushes.Black, _
                                 (playerList(i).myX - XOffset) * xScreenAdj, playerList(i).myY * yScreenAdj + 3, fmt)
                End If
            Next

            'draw top most player
            'pot
            If playerList(topMostClient).fireX <> -1 And rbPrey(topMostClient).Checked Then
                drawTriangle(g, (playerList(topMostClient).fireX - XOffset) * xScreenAdj, playerList(topMostClient).fireY * yScreenAdj, getMyColor(topMostClient), 60, 30)
                g.DrawString(playerList(topMostClient).unitsPot, f, Brushes.Black, _
                             (playerList(topMostClient).fireX - XOffset) * xScreenAdj, playerList(topMostClient).fireY * yScreenAdj - 7, fmt)
            End If

            'player
            If rbPrey(topMostClient).Checked Then
                g.FillRectangle(New SolidBrush(getMyColor(topMostClient)), CInt((playerList(topMostClient).myX - XOffset) * xScreenAdj) - 20, CInt(playerList(topMostClient).myY * yScreenAdj) - 20, 40, 40)
                g.DrawRectangle(Pens.Black, CInt((playerList(topMostClient).myX - XOffset) * xScreenAdj) - 20, CInt(playerList(topMostClient).myY * yScreenAdj) - 20, 40, 40)

                g.DrawString(playerList(topMostClient).unitsPerson, f, Brushes.Black, _
                             (playerList(topMostClient).myX - XOffset) * xScreenAdj, playerList(topMostClient).myY * yScreenAdj - 22, fmt)

                g.DrawString(Math.Round(playerList(topMostClient).currentHealth, 1) & "%", f2, Brushes.Black, _
                             (playerList(topMostClient).myX - XOffset) * xScreenAdj, playerList(topMostClient).myY * yScreenAdj + 3, fmt)
            End If

            'draw transfers
            For i As Integer = 1 To numberOfPlayers
                If playerList(i).transferOpacity > 0 And rbPrey(i).Checked Then
                    Dim p2 As New Pen(Color.FromArgb(playerList(i).transferOpacity, playerList(i).transferColor), 3)
                    p2.EndCap = LineCap.ArrowAnchor
                    g.DrawLine(p2, CInt((playerList(i).transferSource.X - XOffset) * xScreenAdj), CInt(playerList(i).transferSource.Y * yScreenAdj), _
                               CInt((playerList(i).transferTarget.X - XOffset) * xScreenAdj), CInt(playerList(i).transferTarget.Y * yScreenAdj))
                    playerList(i).transferOpacity -= 25
                End If
            Next

            If phase = "hunting" Then
                'draw prey

                'small prey
                For i As Integer = 1 To numberOfPlayers
                    If rbPrey(i).Checked And playerList(i).myX > 1680 * 4 Then
                        For j As Integer = 1 To playerList(i).mySmallPreyCount
                            If playerList(i).smallPrey(j).status <> "caught" Then
                                g.FillEllipse(Brushes.White, CInt(playerList(i).smallPrey(j).myX * xScreenAdj) - 8, _
                                CInt(playerList(i).smallPrey(j).myY * yScreenAdj) - 8, 16, 16)

                                g.FillEllipse(New SolidBrush(Color.FromArgb(150, getMyColor(i))), CInt(playerList(i).smallPrey(j).myX * xScreenAdj) - 8, _
                                CInt(playerList(i).smallPrey(j).myY * yScreenAdj) - 8, 16, 16)

                                g.DrawEllipse(Pens.Black, CInt(playerList(i).smallPrey(j).myX * xScreenAdj) - 8, _
                               CInt(playerList(i).smallPrey(j).myY * yScreenAdj) - 8, 16, 16)
                            End If
                        Next
                    End If
                Next

                'large prey
                For i As Integer = 1 To numberOfPlayers
                    If rbPrey(i).Checked And playerList(i).myX < 1680 * 2 Then
                        For j As Integer = 1 To largePreyPerPeriod
                            If playerList(i).largePrey(j).status <> "caught" Then
                                g.FillEllipse(Brushes.White, CInt(playerList(i).largePrey(j).myX * xScreenAdj) - 15, _
                                CInt(playerList(i).largePrey(j).myY * yScreenAdj) - 15, 30, 30)

                                g.FillEllipse(New SolidBrush(Color.FromArgb(150, getMyColor(i))), CInt(playerList(i).largePrey(j).myX * xScreenAdj) - 15, _
                                CInt(playerList(i).largePrey(j).myY * yScreenAdj) - 15, 30, 30)

                                g.DrawEllipse(Pens.Black, CInt(playerList(i).largePrey(j).myX * xScreenAdj) - 15, _
                               CInt(playerList(i).largePrey(j).myY * yScreenAdj) - 15, 30, 30)
                            End If
                        Next

                    End If
                Next

                'pulling in lines      
                For i As Integer = 1 To numberOfPlayers
                    If playerList(i).pullingIn = True And rbPrey(i).Checked Then
                        If playerList(i).myX > 1680 * 4 Then             'right side
                            g.DrawLine(p, CInt(playerList(i).myX * xScreenAdj), CInt(playerList(i).myY * yScreenAdj),
                                       CInt(playerList(i).smallPrey(playerList(i).preyTarget).myX * xScreenAdj),
                                       CInt(playerList(i).smallPrey(playerList(i).preyTarget).myY * yScreenAdj))

                        Else 'left side
                            g.DrawLine(p, CInt(playerList(i).myX * xScreenAdj), CInt(playerList(i).myY * yScreenAdj),
                                      CInt(playerList(i).largePrey(playerList(i).preyTarget).myX * xScreenAdj),
                                      CInt(playerList(i).largePrey(playerList(i).preyTarget).myY * yScreenAdj))
                        End If
                    End If
                Next
            End If

            'tug of war
            p.StartCap = LineCap.ArrowAnchor

            For i As Integer = 1 To numberOfPlayers
                If playerList(i).tugging And playerList(i).tugger = i And rbPrey(i).Checked Then

                    g.DrawLine(p,
                               CInt((playerList(i).myX - XOffset) * xScreenAdj),
                               CInt(playerList(i).myY * yScreenAdj),
                               CInt((playerList(playerList(i).tugOpponet).myX - XOffset) * xScreenAdj),
                               CInt(playerList(playerList(i).tugOpponet).myY * yScreenAdj))


                    Dim tempD As Integer = (playerList(i).myX - playerList(playerList(i).tugOpponet).myX) / 2

                    g.FillEllipse(New SolidBrush(Color.FromArgb(255, getMyColor(playerList(i).tugOpponet))), CInt((playerList(i).myX - XOffset - tempD) * xScreenAdj) - 15,
                                  CInt(playerList(i).myY * yScreenAdj) - 15,
                                  30,
                                  30)

                    g.DrawEllipse(Pens.Black,
                                  CInt((playerList(i).myX - XOffset - tempD) * xScreenAdj) - 15,
                                  CInt(playerList(i).myY * yScreenAdj) - 15,
                                  30,
                                  30)

                    g.DrawString(playerList(i).tugAmount, f2, Brushes.Black, (playerList(i).myX - XOffset - tempD) * xScreenAdj, playerList(i).myY * yScreenAdj - 8, fmt)
                End If
            Next

            'tug helpers
            p.StartCap = LineCap.NoAnchor

            For i As Integer = 1 To numberOfPlayers
                If playerList(i).tugHelping And rbPrey(i).Checked Then
                    g.DrawLine(p,
                               CInt((playerList(i).myX - XOffset) * xScreenAdj),
                               CInt(playerList(i).myY * yScreenAdj),
                               CInt((playerList(playerList(i).tugHelpTarget).myX - XOffset) * xScreenAdj),
                               CInt(playerList(playerList(i).tugHelpTarget).myY * yScreenAdj))
                End If
            Next

            mainScreen.flip()
        Catch ex As Exception
            appEventLog_Write("error Timer2_Tick:", ex)
        End Try
    End Sub

    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Try

            Dim tempS As String
            Dim sinstr As String

            'dispaly open file dialog to select file
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Filter = "Parameter Files (*.txt)|*.txt"
            OpenFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath

            OpenFileDialog1.ShowDialog()

            'if filename is not empty then continue with load
            If OpenFileDialog1.FileName = "" Then
                Exit Sub
            End If

            tempS = OpenFileDialog1.FileName

            sinstr = getINI(tempS, "gameSettings", "gameName")

            'check that this is correct type of file to load
            If sinstr <> "programName" Then
                MsgBox("Invalid file", vbExclamation)
                Exit Sub
            End If

            'copy file to be loaded into server.ini
            FileCopy(OpenFileDialog1.FileName, sfile)

            'load new parameters into server
            loadParameters()
        Catch ex As Exception
            appEventLog_Write("error cmdLoad_Click:", ex)
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            'save current parameters to a text file so they can be loaded at a later time

            SaveFileDialog1.FileName = ""
            SaveFileDialog1.Filter = "Parameter Files (*.txt)|*.txt"
            SaveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath
            SaveFileDialog1.ShowDialog()

            If SaveFileDialog1.FileName = "" Then
                Exit Sub
            End If

            FileCopy(sfile, SaveFileDialog1.FileName)

        Catch ex As Exception
            appEventLog_Write("error cmdSave_Click:", ex)
        End Try
    End Sub



    Private Sub cmdEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        Try
            'end experiment early

            Dim i As Integer
            cmdEnd.Enabled = False

            numberOfPeriods = currentPeriod

            For i = 1 To numberOfPlayers
                playerList(i).endEarly()
            Next
        Catch ex As Exception
            appEventLog_Write("error cmdEnd_Click:", ex)
        End Try
    End Sub

    Private Sub txtExchange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExchange.Click
        Try
            frmExchange.Show()
        Catch ex As Exception
            appEventLog_Write("error txtExchange:", ex)
        End Try
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Try

        Catch ex As Exception
            appEventLog_Write("error timer3 tick:", ex)
        End Try
    End Sub

    Private Sub cmdSetup2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSetup2.Click
        Try
            frmSetup2.Show()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub llESI_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llESI.LinkClicked
        Try
            System.Diagnostics.Process.Start("http://www.chapman.edu/esi/")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdSmallPrey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSmallPrey.Click
        Try
            Cursor = Cursors.WaitCursor

            frmSetup3.Show()

            Cursor = Cursors.Default
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdPlayerSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPlayerSetup.Click
        Try
            frmSetup4.Show()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdReplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReplay.Click
        Try
            frmReplay.Show()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub Panel2_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseClick
        Try

            For i As Integer = 1 To numberOfPlayers
                If playerList(i).isOverPlayer(e.X, e.Y) Then
                    topMostClient = i
                    drawScreen()
                End If

            Next
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdUnstick_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUnstick.Click
        Try
            Dim tempn As Integer = DataGridView1.CurrentCell.RowIndex

            Dim outstr2 As String = ""
            outstr2 &= playerList(tempn + 1).myX & ","
            outstr2 &= playerList(tempn + 1).myY & ","
            outstr2 &= playerList(tempn + 1).targetX & ","
            outstr2 &= playerList(tempn + 1).targetY & ","

            playerList(tempn + 1).writeEventData("Un-Stick", outstr2)

            playerList(tempn + 1).myX = 5040
            playerList(tempn + 1).myY = 1000

            playerList(tempn + 1).targetX = playerList(tempn + 1).myX
            playerList(tempn + 1).targetY = playerList(tempn + 1).myY

            For i As Integer = 1 To numberOfPlayers
                playerList(i).unStick(tempn + 1)
            Next

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub frmMain_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Try
            Dim r As New System.Drawing.Rectangle(0, 0, Panel2.Width, Panel2.Height)
            mainScreen = New Screen(Panel2, r)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdGroupSetup_Click(sender As Object, e As EventArgs) Handles cmdGroupSetup.Click
        Try
            frmSetup5.Show()
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub
End Class
