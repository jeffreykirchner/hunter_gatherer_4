<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlPot = New System.Windows.Forms.Panel()
        Me.rb2 = New System.Windows.Forms.RadioButton()
        Me.rb1 = New System.Windows.Forms.RadioButton()
        Me.pnlClub = New System.Windows.Forms.Panel()
        Me.cmdStrikePlayer = New System.Windows.Forms.Button()
        Me.cmdSend = New System.Windows.Forms.Button()
        Me.nudPot = New System.Windows.Forms.NumericUpDown()
        Me.gbChat = New System.Windows.Forms.GroupBox()
        Me.cmdChat = New System.Windows.Forms.Button()
        Me.txtChat = New System.Windows.Forms.TextBox()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.txtChat8 = New System.Windows.Forms.TextBox()
        Me.txtChat7 = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtMessages = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdFire = New System.Windows.Forms.Button()
        Me.cmdYield = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtChat6 = New System.Windows.Forms.TextBox()
        Me.txtChat5 = New System.Windows.Forms.TextBox()
        Me.txtChat4 = New System.Windows.Forms.TextBox()
        Me.txtPeriod = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.txtProfit = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlHelp = New System.Windows.Forms.Panel()
        Me.pnlYield = New System.Windows.Forms.Panel()
        Me.txtChat3 = New System.Windows.Forms.TextBox()
        Me.txtChat2 = New System.Windows.Forms.TextBox()
        Me.txtChat1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbStatus = New System.Windows.Forms.GroupBox()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlPot.SuspendLayout()
        CType(Me.nudPot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbChat.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlHelp.SuspendLayout()
        Me.pnlYield.SuspendLayout()
        Me.gbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'pnlPot
        '
        Me.pnlPot.BackColor = System.Drawing.Color.White
        Me.pnlPot.Controls.Add(Me.rb2)
        Me.pnlPot.Controls.Add(Me.rb1)
        Me.pnlPot.Controls.Add(Me.pnlClub)
        Me.pnlPot.Controls.Add(Me.cmdStrikePlayer)
        Me.pnlPot.Controls.Add(Me.cmdSend)
        Me.pnlPot.Controls.Add(Me.nudPot)
        Me.pnlPot.Location = New System.Drawing.Point(620, 57)
        Me.pnlPot.Name = "pnlPot"
        Me.pnlPot.Size = New System.Drawing.Size(180, 86)
        Me.pnlPot.TabIndex = 8
        Me.pnlPot.Visible = False
        '
        'rb2
        '
        Me.rb2.AutoSize = True
        Me.rb2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rb2.Location = New System.Drawing.Point(7, 23)
        Me.rb2.Name = "rb2"
        Me.rb2.Size = New System.Drawing.Size(119, 20)
        Me.rb2.TabIndex = 6
        Me.rb2.TabStop = True
        Me.rb2.Text = "RadioButton2"
        Me.rb2.UseVisualStyleBackColor = True
        '
        'rb1
        '
        Me.rb1.AutoSize = True
        Me.rb1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rb1.Location = New System.Drawing.Point(7, 4)
        Me.rb1.Name = "rb1"
        Me.rb1.Size = New System.Drawing.Size(119, 20)
        Me.rb1.TabIndex = 5
        Me.rb1.TabStop = True
        Me.rb1.Text = "RadioButton1"
        Me.rb1.UseVisualStyleBackColor = True
        '
        'pnlClub
        '
        Me.pnlClub.BackColor = System.Drawing.Color.Black
        Me.pnlClub.Location = New System.Drawing.Point(130, 7)
        Me.pnlClub.Name = "pnlClub"
        Me.pnlClub.Size = New System.Drawing.Size(2, 72)
        Me.pnlClub.TabIndex = 4
        '
        'cmdStrikePlayer
        '
        Me.cmdStrikePlayer.Image = CType(resources.GetObject("cmdStrikePlayer.Image"), System.Drawing.Image)
        Me.cmdStrikePlayer.Location = New System.Drawing.Point(138, 8)
        Me.cmdStrikePlayer.Name = "cmdStrikePlayer"
        Me.cmdStrikePlayer.Size = New System.Drawing.Size(39, 71)
        Me.cmdStrikePlayer.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.cmdStrikePlayer, "Strike Player.")
        Me.cmdStrikePlayer.UseVisualStyleBackColor = True
        '
        'cmdSend
        '
        Me.cmdSend.Image = CType(resources.GetObject("cmdSend.Image"), System.Drawing.Image)
        Me.cmdSend.Location = New System.Drawing.Point(3, 43)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(61, 40)
        Me.cmdSend.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cmdSend, "Transfer units between the above selection.")
        Me.cmdSend.UseVisualStyleBackColor = True
        '
        'nudPot
        '
        Me.nudPot.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudPot.Location = New System.Drawing.Point(70, 48)
        Me.nudPot.Name = "nudPot"
        Me.nudPot.Size = New System.Drawing.Size(54, 31)
        Me.nudPot.TabIndex = 0
        Me.nudPot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'gbChat
        '
        Me.gbChat.Controls.Add(Me.cmdChat)
        Me.gbChat.Controls.Add(Me.txtChat)
        Me.gbChat.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbChat.Location = New System.Drawing.Point(530, 906)
        Me.gbChat.Name = "gbChat"
        Me.gbChat.Size = New System.Drawing.Size(829, 50)
        Me.gbChat.TabIndex = 41
        Me.gbChat.TabStop = False
        '
        'cmdChat
        '
        Me.cmdChat.Image = CType(resources.GetObject("cmdChat.Image"), System.Drawing.Image)
        Me.cmdChat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdChat.Location = New System.Drawing.Point(7, 16)
        Me.cmdChat.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdChat.Name = "cmdChat"
        Me.cmdChat.Size = New System.Drawing.Size(68, 27)
        Me.cmdChat.TabIndex = 2
        Me.cmdChat.Text = "    Chat"
        Me.ToolTip1.SetToolTip(Me.cmdChat, "Chat will appear in a bubble next to your Avatar.")
        Me.cmdChat.UseVisualStyleBackColor = True
        '
        'txtChat
        '
        Me.txtChat.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat.Location = New System.Drawing.Point(82, 17)
        Me.txtChat.Name = "txtChat"
        Me.txtChat.Size = New System.Drawing.Size(741, 26)
        Me.txtChat.TabIndex = 1
        '
        'BackgroundWorker1
        '
        '
        'txtChat8
        '
        Me.txtChat8.BackColor = System.Drawing.Color.White
        Me.txtChat8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat8.Location = New System.Drawing.Point(191, 311)
        Me.txtChat8.Multiline = True
        Me.txtChat8.Name = "txtChat8"
        Me.txtChat8.ReadOnly = True
        Me.txtChat8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat8.Size = New System.Drawing.Size(176, 90)
        Me.txtChat8.TabIndex = 7
        Me.txtChat8.Visible = False
        '
        'txtChat7
        '
        Me.txtChat7.BackColor = System.Drawing.Color.White
        Me.txtChat7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat7.Location = New System.Drawing.Point(191, 215)
        Me.txtChat7.Multiline = True
        Me.txtChat7.Name = "txtChat7"
        Me.txtChat7.ReadOnly = True
        Me.txtChat7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat7.Size = New System.Drawing.Size(176, 90)
        Me.txtChat7.TabIndex = 6
        Me.txtChat7.Visible = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Panel1)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(1365, 1)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(114, 699)
        Me.GroupBox3.TabIndex = 44
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Health"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Location = New System.Drawing.Point(6, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(97, 672)
        Me.Panel1.TabIndex = 32
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtMessages)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 906)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(512, 50)
        Me.GroupBox2.TabIndex = 42
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Notifications"
        '
        'txtMessages
        '
        Me.txtMessages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessages.BackColor = System.Drawing.Color.White
        Me.txtMessages.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMessages.ForeColor = System.Drawing.Color.IndianRed
        Me.txtMessages.Location = New System.Drawing.Point(6, 16)
        Me.txtMessages.Name = "txtMessages"
        Me.txtMessages.ReadOnly = True
        Me.txtMessages.Size = New System.Drawing.Size(500, 29)
        Me.txtMessages.TabIndex = 5
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 1000
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 100
        '
        'cmdFire
        '
        Me.cmdFire.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFire.ForeColor = System.Drawing.Color.Red
        Me.cmdFire.Image = CType(resources.GetObject("cmdFire.Image"), System.Drawing.Image)
        Me.cmdFire.Location = New System.Drawing.Point(1365, 706)
        Me.cmdFire.Name = "cmdFire"
        Me.cmdFire.Size = New System.Drawing.Size(114, 81)
        Me.cmdFire.TabIndex = 43
        Me.ToolTip1.SetToolTip(Me.cmdFire, "Place your Hearth in a new location.")
        Me.cmdFire.UseVisualStyleBackColor = True
        '
        'cmdYield
        '
        Me.cmdYield.Image = CType(resources.GetObject("cmdYield.Image"), System.Drawing.Image)
        Me.cmdYield.Location = New System.Drawing.Point(4, 4)
        Me.cmdYield.Name = "cmdYield"
        Me.cmdYield.Size = New System.Drawing.Size(61, 54)
        Me.cmdYield.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cmdYield, "Yield tug of war to your opponent.")
        Me.cmdYield.UseVisualStyleBackColor = True
        '
        'cmdHelp
        '
        Me.cmdHelp.Image = CType(resources.GetObject("cmdHelp.Image"), System.Drawing.Image)
        Me.cmdHelp.Location = New System.Drawing.Point(4, 4)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(61, 54)
        Me.cmdHelp.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cmdHelp, "Help player in tug of war.")
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.Location = New System.Drawing.Point(71, 4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(61, 54)
        Me.cmdCancel.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel help.")
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'txtChat6
        '
        Me.txtChat6.BackColor = System.Drawing.Color.White
        Me.txtChat6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat6.Location = New System.Drawing.Point(191, 110)
        Me.txtChat6.Multiline = True
        Me.txtChat6.Name = "txtChat6"
        Me.txtChat6.ReadOnly = True
        Me.txtChat6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat6.Size = New System.Drawing.Size(176, 90)
        Me.txtChat6.TabIndex = 5
        Me.txtChat6.Visible = False
        '
        'txtChat5
        '
        Me.txtChat5.BackColor = System.Drawing.Color.White
        Me.txtChat5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat5.Location = New System.Drawing.Point(190, 3)
        Me.txtChat5.Multiline = True
        Me.txtChat5.Name = "txtChat5"
        Me.txtChat5.ReadOnly = True
        Me.txtChat5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat5.Size = New System.Drawing.Size(177, 90)
        Me.txtChat5.TabIndex = 4
        Me.txtChat5.Visible = False
        '
        'txtChat4
        '
        Me.txtChat4.BackColor = System.Drawing.Color.White
        Me.txtChat4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat4.Location = New System.Drawing.Point(4, 311)
        Me.txtChat4.Multiline = True
        Me.txtChat4.Name = "txtChat4"
        Me.txtChat4.ReadOnly = True
        Me.txtChat4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat4.Size = New System.Drawing.Size(176, 90)
        Me.txtChat4.TabIndex = 3
        Me.txtChat4.Visible = False
        '
        'txtPeriod
        '
        Me.txtPeriod.BackColor = System.Drawing.Color.White
        Me.txtPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeriod.Location = New System.Drawing.Point(21, 52)
        Me.txtPeriod.Name = "txtPeriod"
        Me.txtPeriod.ReadOnly = True
        Me.txtPeriod.Size = New System.Drawing.Size(73, 26)
        Me.txtPeriod.TabIndex = 9
        Me.txtPeriod.Text = "1"
        Me.txtPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 20)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Period"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer2
        '
        Me.Timer2.Interval = 10
        '
        'txtProfit
        '
        Me.txtProfit.BackColor = System.Drawing.Color.White
        Me.txtProfit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProfit.Location = New System.Drawing.Point(21, 117)
        Me.txtProfit.Name = "txtProfit"
        Me.txtProfit.ReadOnly = True
        Me.txtProfit.Size = New System.Drawing.Size(73, 26)
        Me.txtProfit.TabIndex = 7
        Me.txtProfit.Text = "0"
        Me.txtProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.pnlHelp)
        Me.Panel2.Controls.Add(Me.pnlYield)
        Me.Panel2.Controls.Add(Me.pnlPot)
        Me.Panel2.Controls.Add(Me.txtChat8)
        Me.Panel2.Controls.Add(Me.txtChat7)
        Me.Panel2.Controls.Add(Me.txtChat6)
        Me.Panel2.Controls.Add(Me.txtChat5)
        Me.Panel2.Controls.Add(Me.txtChat4)
        Me.Panel2.Controls.Add(Me.txtChat3)
        Me.Panel2.Controls.Add(Me.txtChat2)
        Me.Panel2.Controls.Add(Me.txtChat1)
        Me.Panel2.Location = New System.Drawing.Point(12, 10)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1347, 890)
        Me.Panel2.TabIndex = 40
        '
        'pnlHelp
        '
        Me.pnlHelp.BackColor = System.Drawing.Color.White
        Me.pnlHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlHelp.Controls.Add(Me.cmdCancel)
        Me.pnlHelp.Controls.Add(Me.cmdHelp)
        Me.pnlHelp.Location = New System.Drawing.Point(700, 149)
        Me.pnlHelp.Name = "pnlHelp"
        Me.pnlHelp.Size = New System.Drawing.Size(140, 63)
        Me.pnlHelp.TabIndex = 10
        Me.pnlHelp.Visible = False
        '
        'pnlYield
        '
        Me.pnlYield.BackColor = System.Drawing.Color.White
        Me.pnlYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlYield.Controls.Add(Me.cmdYield)
        Me.pnlYield.Location = New System.Drawing.Point(623, 149)
        Me.pnlYield.Name = "pnlYield"
        Me.pnlYield.Size = New System.Drawing.Size(71, 63)
        Me.pnlYield.TabIndex = 9
        Me.pnlYield.Visible = False
        '
        'txtChat3
        '
        Me.txtChat3.BackColor = System.Drawing.Color.White
        Me.txtChat3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat3.Location = New System.Drawing.Point(4, 215)
        Me.txtChat3.Multiline = True
        Me.txtChat3.Name = "txtChat3"
        Me.txtChat3.ReadOnly = True
        Me.txtChat3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat3.Size = New System.Drawing.Size(176, 90)
        Me.txtChat3.TabIndex = 2
        Me.txtChat3.Visible = False
        '
        'txtChat2
        '
        Me.txtChat2.BackColor = System.Drawing.Color.White
        Me.txtChat2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat2.Location = New System.Drawing.Point(4, 110)
        Me.txtChat2.Multiline = True
        Me.txtChat2.Name = "txtChat2"
        Me.txtChat2.ReadOnly = True
        Me.txtChat2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat2.Size = New System.Drawing.Size(176, 90)
        Me.txtChat2.TabIndex = 1
        Me.txtChat2.Visible = False
        '
        'txtChat1
        '
        Me.txtChat1.BackColor = System.Drawing.Color.White
        Me.txtChat1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtChat1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChat1.Location = New System.Drawing.Point(3, 3)
        Me.txtChat1.Multiline = True
        Me.txtChat1.Name = "txtChat1"
        Me.txtChat1.ReadOnly = True
        Me.txtChat1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChat1.Size = New System.Drawing.Size(177, 90)
        Me.txtChat1.TabIndex = 0
        Me.txtChat1.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Earnings $"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbStatus
        '
        Me.gbStatus.Controls.Add(Me.txtPeriod)
        Me.gbStatus.Controls.Add(Me.Label2)
        Me.gbStatus.Controls.Add(Me.txtProfit)
        Me.gbStatus.Controls.Add(Me.Label1)
        Me.gbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbStatus.Location = New System.Drawing.Point(1365, 793)
        Me.gbStatus.Name = "gbStatus"
        Me.gbStatus.Size = New System.Drawing.Size(114, 154)
        Me.gbStatus.TabIndex = 39
        Me.gbStatus.TabStop = False
        Me.gbStatus.Text = "Status"
        '
        'Timer3
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1491, 962)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbChat)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdFire)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.gbStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmMain"
        Me.Text = "Client"
        Me.pnlPot.ResumeLayout(False)
        Me.pnlPot.PerformLayout()
        CType(Me.nudPot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbChat.ResumeLayout(False)
        Me.gbChat.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlHelp.ResumeLayout(False)
        Me.pnlYield.ResumeLayout(False)
        Me.gbStatus.ResumeLayout(False)
        Me.gbStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents pnlPot As System.Windows.Forms.Panel
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents nudPot As System.Windows.Forms.NumericUpDown
    Friend WithEvents gbChat As System.Windows.Forms.GroupBox
    Friend WithEvents cmdChat As System.Windows.Forms.Button
    Friend WithEvents txtChat As System.Windows.Forms.TextBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents txtChat8 As System.Windows.Forms.TextBox
    Friend WithEvents txtChat7 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMessages As System.Windows.Forms.TextBox
    Friend WithEvents cmdFire As System.Windows.Forms.Button
    Friend WithEvents txtChat6 As System.Windows.Forms.TextBox
    Friend WithEvents txtChat5 As System.Windows.Forms.TextBox
    Friend WithEvents txtChat4 As System.Windows.Forms.TextBox
    Friend WithEvents txtPeriod As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents txtProfit As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtChat3 As System.Windows.Forms.TextBox
    Friend WithEvents txtChat2 As System.Windows.Forms.TextBox
    Friend WithEvents txtChat1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbStatus As System.Windows.Forms.GroupBox
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents pnlClub As System.Windows.Forms.Panel
    Friend WithEvents cmdStrikePlayer As System.Windows.Forms.Button
    Friend WithEvents rb2 As System.Windows.Forms.RadioButton
    Friend WithEvents rb1 As System.Windows.Forms.RadioButton
    Friend WithEvents pnlYield As System.Windows.Forms.Panel
    Friend WithEvents cmdYield As System.Windows.Forms.Button
    Friend WithEvents pnlHelp As System.Windows.Forms.Panel
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button

End Class
