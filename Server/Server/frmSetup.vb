Public Class frmSetup

    Private Sub frmSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'load paremeters into text boxes from server.ini
            txtPlayers.Text = getINI(sfile, "gameSettings", "numberOfPlayers")
            chkShowInstructions.Checked = getINI(sfile, "gameSettings", "showInstructions")
            txtPort.Text = getINI(sfile, "gameSettings", "port")
            txtPeriods.Text = getINI(sfile, "gameSettings", "numberOfPeriods")
            txtPlayerSpeed.Text = getINI(sfile, "gameSettings", "playerSpeed")
            txtLandMarks.Text = getINI(sfile, "gameSettings", "bushCount")
            txtLargePreyPerPeriod.Text = getINI(sfile, "gameSettings", "largePreyPerPeriod")
            txtLargePreyValue.Text = getINI(sfile, "gameSettings", "largePreyValue")
            txtSmallPreyValue.Text = getINI(sfile, "gameSettings", "smallPreyValue")
            txtPreySpeed.Text = getINI(sfile, "gameSettings", "preyMovementRate")
            txtHuntingLength.Text = getINI(sfile, "gameSettings", "huntingLength")
            txtTradingLength.Text = getINI(sfile, "gameSettings", "tradingLength")
            txtHearthCapacity.Text = getINI(sfile, "gameSettings", "hearthCapacity")
            txtStepSpeed.Text = getINI(sfile, "gameSettings", "stepSpeed")
            txtLargePreyProbability.Text = getINI(sfile, "gameSettings", "largePreyProbability")
            txtStartingHealth.Text = getINI(sfile, "gameSettings", "startingHealth")
            txtHealthLoss.Text = getINI(sfile, "gameSettings", "healthLoss")
            txtInterimLength.Text = getINI(sfile, "gameSettings", "interimLength")
            cbTestMode.Checked = getINI(sfile, "gameSettings", "testMode")
            txtRestPeriodFrequency.Text = getINI(sfile, "gameSettings", "restPeriodFrequency")
            txtRestPeriodLength.Text = getINI(sfile, "gameSettings", "restPeriodLength")
            txtInstructionX.Text = getINI(sfile, "gameSettings", "instructionX")
            txtInstructionY.Text = getINI(sfile, "gameSettings", "instructionY")
            txtWindowX.Text = getINI(sfile, "gameSettings", "windowX")
            txtWindowY.Text = getINI(sfile, "gameSettings", "windowY")
            cbShowHealth.Checked = getINI(sfile, "gameSettings", "showHealth")
            cbAllowTake.Checked = getINI(sfile, "gameSettings", "allowTake")

            txtRadius.Text = getINI(sfile, "gameSettings", "interactionRadius")
            txtInteractionLength.Text = getINI(sfile, "gameSettings", "interactionLength")
            txtInteractionCoolDown.Text = getINI(sfile, "gameSettings", "interactionCoolDown")
            cbAllowHit.Checked = getINI(sfile, "gameSettings", "allowHit")
            txtHitDamage.Text = getINI(sfile, "gameSettings", "hitDamage")
            txtHitCost.Text = getINI(sfile, "gameSettings", "hitCost")
            txtEarningsMultiplier.Text = getINI(sfile, "gameSettings", "earningsMultiplier")

            txtPlayerScale.Text = getINI(sfile, "gameSettings", "playerScale")

            cbTugOfWar.Checked = getINI(sfile, "gameSettings", "tugOfWar")
            txtTugOfWarCost.Text = getINI(sfile, "gameSettings", "tugOfWarCost")
            cbSmallPrey.Checked = getINI(sfile, "gameSettings", "smallPreyAvailable")
            cbPots.Checked = getINI(sfile, "gameSettings", "potsAvailable")

            txtWorldWidth.Text = getINI(sfile, "gameSettings", "worldWidth")
            txtWorldHeight.Text = getINI(sfile, "gameSettings", "worldHeight")
        Catch ex As Exception
            appEventLog_Write("error frmSetup_Load:", ex)
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            'write parameter from text boxes into server.ini
            writeINI(sfile, "gameSettings", "numberOfPlayers", txtPlayers.Text)
            writeINI(sfile, "gameSettings", "showInstructions", chkShowInstructions.Checked)
            writeINI(sfile, "gameSettings", "port", txtPort.Text)
            writeINI(sfile, "gameSettings", "numberOfPeriods", txtPeriods.Text)
            writeINI(sfile, "gameSettings", "playerSpeed", txtPlayerSpeed.Text)
            writeINI(sfile, "gameSettings", "bushCount", txtLandMarks.Text)
            writeINI(sfile, "gameSettings", "largePreyPerPeriod", txtLargePreyPerPeriod.Text)
            writeINI(sfile, "gameSettings", "largePreyValue", txtLargePreyValue.Text)
            writeINI(sfile, "gameSettings", "smallPreyValue", txtSmallPreyValue.Text)
            writeINI(sfile, "gameSettings", "preyMovementRate", txtPreySpeed.Text)
            writeINI(sfile, "gameSettings", "huntingLength", txtHuntingLength.Text)
            writeINI(sfile, "gameSettings", "tradingLength", txtTradingLength.Text)
            writeINI(sfile, "gameSettings", "hearthCapacity", txtHearthCapacity.Text)
            writeINI(sfile, "gameSettings", "stepSpeed", txtStepSpeed.Text)
            writeINI(sfile, "gameSettings", "largePreyProbability", txtLargePreyProbability.Text)
            writeINI(sfile, "gameSettings", "startingHealth", txtStartingHealth.Text)
            writeINI(sfile, "gameSettings", "healthLoss", txtHealthLoss.Text)
            writeINI(sfile, "gameSettings", "interimLength", txtInterimLength.Text)
            writeINI(sfile, "gameSettings", "testMode", cbTestMode.Checked)
            writeINI(sfile, "gameSettings", "restPeriodFrequency", txtRestPeriodFrequency.Text)
            writeINI(sfile, "gameSettings", "restPeriodLength", txtRestPeriodLength.Text)
            writeINI(sfile, "gameSettings", "instructionX", txtInstructionX.Text)
            writeINI(sfile, "gameSettings", "instructionY", txtInstructionY.Text)
            writeINI(sfile, "gameSettings", "windowX", txtWindowX.Text)
            writeINI(sfile, "gameSettings", "windowY", txtWindowY.Text)
            writeINI(sfile, "gameSettings", "showHealth", cbShowHealth.Checked)
            writeINI(sfile, "gameSettings", "allowTake", cbAllowTake.Checked)

            writeINI(sfile, "gameSettings", "interactionRadius", txtRadius.Text)
            writeINI(sfile, "gameSettings", "interactionLength", txtInteractionLength.Text)
            writeINI(sfile, "gameSettings", "interactionCoolDown", txtInteractionCoolDown.Text)
            writeINI(sfile, "gameSettings", "allowHit", cbAllowHit.Checked)
            writeINI(sfile, "gameSettings", "hitDamage", txtHitDamage.Text)
            writeINI(sfile, "gameSettings", "hitCost", txtHitCost.Text)
            writeINI(sfile, "gameSettings", "earningsMultiplier", txtEarningsMultiplier.Text)

            writeINI(sfile, "gameSettings", "playerScale", txtPlayerScale.Text)

            writeINI(sfile, "gameSettings", "tugOfWar", cbTugOfWar.Checked)
            writeINI(sfile, "gameSettings", "tugOfWarCost", txtTugOfWarCost.Text)
            writeINI(sfile, "gameSettings", "smallPreyAvailable", cbSmallPrey.Checked)
            writeINI(sfile, "gameSettings", "potsAvailable", cbPots.Checked)

            writeINI(sfile, "gameSettings", "worldWidth", txtWorldWidth.Text)
            writeINI(sfile, "gameSettings", "worldHeight", txtWorldHeight.Text)

            loadParameters()

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error cmdSave_Click:", ex)
        End Try
    End Sub
End Class