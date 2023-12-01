Public Class frmInstructions
    Dim tempS As Boolean
    Dim maxPage As Integer = 3
    Dim startPressed As Boolean

    Public pagesDone(11) As Boolean

    Public page8counter As Integer

    Public Sub nextInstruction()
        Try
            'load the next page of instructions

            RichTextBox1.LoadFile(System.Windows.Forms.Application.StartupPath & _
                 "\instructions\page" & currentInstruction & ".rtf")

            variables()

            RichTextBox1.SelectionStart = 1
            RichTextBox1.ScrollToCaret()

            If Not startPressed Then wskClient.Send("10", currentInstruction & ";")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub variables()
        Try
            'load variables into instructions

            Dim tempN As Integer = 0
            Dim outstr As String = ""
            Select Case currentInstruction
                Case 1
                    If Not pagesDone(currentInstruction) = True Then
                        pagesDone(currentInstruction) = True
                        phase = "hunting"
                    End If

                    Call RepRTBfield("playerCount-1", numberOfPlayers - 1)
                    Call RepRTBfield("color", playerlist(inumber).colorName)
                    RichTextBox1.Find(playerlist(inumber).colorName)
                    RichTextBox1.SelectionColor = getMyColor(inumber)

                    Call RepRTBfield("periodLength", huntingLength + tradingLength + interimLength)
                Case 2
                    If Not pagesDone(currentInstruction) = True Then
                        pagesDone(currentInstruction) = True
                        phase = "hunting"
                    End If
                Case 3
                    If Not pagesDone(currentInstruction) = True Then
                        pagesDone(currentInstruction) = True
                        phase = "hunting"
                    End If
                Case 4
                    If Not pagesDone(currentInstruction) = True Then
                        phase = "hunting"
                    End If
                Case 5
                    pagesDone(currentInstruction) = True
                    phase = "trading"
                    updateStringTex(frmMain.Device, frmMain.timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(CInt(tradingLength)))

                    Call RepRTBfield("xx", hearthCapacity)
                    Call RepRTBfield("xx", hearthCapacity)

                    leftSideOn = False
                    rightSideOn = False

                    For i As Integer = 1 To 200
                        smallPrey(i).status = "out"
                    Next

                    For i As Integer = 1 To largePreyPerPeriod
                        largePrey(i).status = "out"
                    Next

                    playerlist(inumber).pullingIn = False

                    frmMain.initializeMiniMap()

                    If Not leftSideOn Then
                        If playerlist(inumber).myX < 1680 * worldWidth * (2 / 6) + 50 Then
                            playerlist(inumber).targetX = 1680 * worldWidth * (2 / 6) + 200
                            'playerlist(inumber).targetY = 1050 * 3

                            sendUpdateTarget()

                            frmMain.txtMessages.Text = "Auto Return."
                        End If
                    End If

                    If Not rightSideOn Then
                        If playerlist(inumber).myX > 1680 * worldWidth * (4 / 6) - 50 Then
                            playerlist(inumber).targetX = 1680 * worldWidth * (4 / 6) - 200
                            'playerlist(inumber).targetY = 1050 * 3

                            sendUpdateTarget()

                            frmMain.txtMessages.Text = "Auto Return."
                        End If
                    End If
                Case 6
                    'Call RepRTBfield("cooldown", interactionCoolDown)
                    If Not pagesDone(currentInstruction) Then
                        playerlist(9).targetX = playerlist(inumber).targetX + 200
                        playerlist(9).targetY = playerlist(inumber).targetY
                    End If
                Case 7
                    'Call RepRTBfield("multiplier", earningsMultiplier)
                    'Call RepRTBfield("multiplier*54", earningsMultiplier * 54)

                    phase = "trading"
                    updateStringTex(frmMain.Device, frmMain.timeRemainingTex, 30, Drawing.Color.DimGray, timeConversion(CInt(interimLength)))
                    Call RepRTBfield("loss", tugOfWarCost)
                    Call RepRTBfield("cooldown", interactionCoolDown)
                Case 8
                    phase = "trading"

                    Call RepRTBfield("loss", tugOfWarCost)
                    Call RepRTBfield("2*loss", tugOfWarCost * 2)

                    If Not pagesDone(currentInstruction) Then

                        page8counter = 0

                        playerlist(9).myX = 3600
                        playerlist(9).myY = 500
                        playerlist(9).targetX = playerlist(9).myX
                        playerlist(9).targetY = playerlist(9).myY

                        playerlist(10).myX = 4000
                        playerlist(10).myY = 500
                        playerlist(10).targetX = playerlist(10).myX
                        playerlist(10).targetY = playerlist(10).myY

                        playerlist(10).tugAmount = 1
                        playerlist(9).tugAmount = 1

                        playerlist(10).tugging = True
                        playerlist(9).tugging = True

                        playerlist(10).tugger = 10
                        playerlist(9).tugger = 10

                        playerlist(10).tugTarget = 9
                        playerlist(9).tugTarget = 9

                        playerlist(10).stunned = 0
                        playerlist(9).stunned = 0

                        playerlist(10).stunning = 0
                        playerlist(9).stunning = 0

                        playerlist(10).coolDown = 0
                        playerlist(9).coolDown = 0

                        playerlist(10).tugOpponet = 9
                        playerlist(9).tugOpponet = 10

                        updateStringTex(frmMain.Device,
                                        playerlist(10).tugAmountTex,
                                        20,
                                        Drawing.Color.Black,
                                        playerlist(10).tugAmount)
                    End If
                Case 9
                    Call RepRTBfield("xx", healthLoss)

                    Call RepRTBfield("multiplier", earningsMultiplier)
                    Call RepRTBfield("multiplier*54", earningsMultiplier * 54)
                    phase = "trading"

                    If Not pagesDone(currentInstruction) Then
                        pagesDone(currentInstruction) = True
                    End If
                Case 10
                    If Not pagesDone(currentInstruction) Then
                        pagesDone(currentInstruction) = True
                    End If
                Case 11
                    Call RepRTBfield("startinghealth", startingHealth)
                    Call RepRTBfield("break", restPeriodFrequency)
                    Call RepRTBfield("time", restPeriodLength)
                    Call RepRTBfield("xx", healthLoss)
                    Call RepRTBfield("multiplier", earningsMultiplier)

                    If Not pagesDone(currentInstruction) Then
                        pagesDone(currentInstruction) = True
                    End If
            End Select

            Me.Text = "Instructions " & currentInstruction & "/11"
        Catch ex As Exception
            appEventLog_Write("error variables:", ex)
        End Try
    End Sub

    Public Sub RepRTBfield(ByVal sField As String, ByVal sValue As String)
        Try
            'when the instructions are loaded into the rich text box control this function will
            'replace the variable place holders with variables.

            RichTextBox1.Find("#" & sField & "#")
            RichTextBox1.SelectedText = sValue
        Catch ex As Exception
            appEventLog_Write("error RepRTBfield:", ex)
        End Try
    End Sub

    Private Sub frmInstructions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            startPressed = False
            currentInstruction = 1
            nextInstruction()
            tempS = False
        Catch ex As Exception
            appEventLog_Write("error frmInstructions_Load:", ex)
        End Try
    End Sub

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        Try
            'client done with instructions
           cmdStartAction
        Catch ex As Exception
            appEventLog_Write("error instructinos start:", ex)
        End Try
    End Sub

    Public Sub cmdStartAction()
        Try
            'client done with instructions
            wskClient.Send("02", "")
            cmdStart.Visible = False
            startPressed = True
        Catch ex As Exception
            appEventLog_Write("error instructinos start:", ex)
        End Try
    End Sub

    Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Try
            cmdNextAction()
        Catch ex As Exception
            appEventLog_Write("error cmdNext_Click:", ex)
        End Try
    End Sub

    Public Sub cmdNextAction()
        Try
            'load next page of instructions

            If pagesDone(currentInstruction) = False Then
                If Not testMode Then MessageBox.Show("Please take the requested action for before continuing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If currentInstruction = 11 Then Exit Sub

            currentInstruction += 1
            If maxPage < currentInstruction Then maxPage = currentInstruction

            If currentInstruction = 11 And Not tempS Then
                cmdStart.Visible = True
                tempS = True
            End If

            If currentInstruction = 11 Then
                cmdNext.Visible = False
            End If

            cmdBack.Visible = True

            nextInstruction()
        Catch ex As Exception
            appEventLog_Write("error cmdNext_Click:", ex)
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            'previous page of instructions

            If currentInstruction = 1 Then Exit Sub

            currentInstruction -= 1

            'If maxPage >= 5 And currentInstruction < 5 Then currentInstruction = 5

            If currentInstruction = 1 Then cmdBack.Visible = False
            cmdNext.Visible = True

            nextInstruction()
        Catch ex As Exception
            appEventLog_Write("error cmdBack_Click :", ex)
        End Try
    End Sub
End Class