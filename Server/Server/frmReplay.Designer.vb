<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReplay
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReplay))
        Me.txtSpeed = New System.Windows.Forms.DomainUpDown()
        Me.cmdPauseData = New System.Windows.Forms.Button()
        Me.cmdPlayData = New System.Windows.Forms.Button()
        Me.cmdLoadData = New System.Windows.Forms.Button()
        Me.tbData = New System.Windows.Forms.TrackBar()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.tbData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtSpeed
        '
        Me.txtSpeed.BackColor = System.Drawing.Color.White
        Me.txtSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSpeed.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.txtSpeed.Items.Add("4x")
        Me.txtSpeed.Items.Add("3x")
        Me.txtSpeed.Items.Add("2x")
        Me.txtSpeed.Items.Add("1x")
        Me.txtSpeed.Location = New System.Drawing.Point(469, 12)
        Me.txtSpeed.Name = "txtSpeed"
        Me.txtSpeed.ReadOnly = True
        Me.txtSpeed.Size = New System.Drawing.Size(55, 29)
        Me.txtSpeed.TabIndex = 48
        Me.txtSpeed.TabStop = False
        Me.txtSpeed.Text = "1x"
        Me.txtSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdPauseData
        '
        Me.cmdPauseData.Enabled = False
        Me.cmdPauseData.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPauseData.Image = CType(resources.GetObject("cmdPauseData.Image"), System.Drawing.Image)
        Me.cmdPauseData.Location = New System.Drawing.Point(106, 9)
        Me.cmdPauseData.Name = "cmdPauseData"
        Me.cmdPauseData.Size = New System.Drawing.Size(44, 39)
        Me.cmdPauseData.TabIndex = 46
        Me.cmdPauseData.UseVisualStyleBackColor = True
        '
        'cmdPlayData
        '
        Me.cmdPlayData.Enabled = False
        Me.cmdPlayData.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPlayData.Image = CType(resources.GetObject("cmdPlayData.Image"), System.Drawing.Image)
        Me.cmdPlayData.Location = New System.Drawing.Point(56, 9)
        Me.cmdPlayData.Name = "cmdPlayData"
        Me.cmdPlayData.Size = New System.Drawing.Size(44, 38)
        Me.cmdPlayData.TabIndex = 45
        Me.cmdPlayData.UseVisualStyleBackColor = True
        '
        'cmdLoadData
        '
        Me.cmdLoadData.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLoadData.Image = CType(resources.GetObject("cmdLoadData.Image"), System.Drawing.Image)
        Me.cmdLoadData.Location = New System.Drawing.Point(6, 9)
        Me.cmdLoadData.Name = "cmdLoadData"
        Me.cmdLoadData.Size = New System.Drawing.Size(44, 38)
        Me.cmdLoadData.TabIndex = 44
        Me.cmdLoadData.UseVisualStyleBackColor = True
        '
        'tbData
        '
        Me.tbData.Enabled = False
        Me.tbData.LargeChange = 1
        Me.tbData.Location = New System.Drawing.Point(156, 12)
        Me.tbData.Name = "tbData"
        Me.tbData.Size = New System.Drawing.Size(307, 45)
        Me.tbData.TabIndex = 47
        Me.tbData.TickFrequency = 10
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmReplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 56)
        Me.Controls.Add(Me.txtSpeed)
        Me.Controls.Add(Me.cmdPauseData)
        Me.Controls.Add(Me.cmdPlayData)
        Me.Controls.Add(Me.cmdLoadData)
        Me.Controls.Add(Me.tbData)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReplay"
        Me.Text = "Replay"
        CType(Me.tbData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSpeed As System.Windows.Forms.DomainUpDown
    Friend WithEvents cmdPauseData As System.Windows.Forms.Button
    Friend WithEvents cmdPlayData As System.Windows.Forms.Button
    Friend WithEvents cmdLoadData As System.Windows.Forms.Button
    Friend WithEvents tbData As System.Windows.Forms.TrackBar
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
