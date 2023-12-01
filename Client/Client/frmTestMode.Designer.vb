<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTestMode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTestMode))
        Me.cmdTakeControl = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmdTakeControl
        '
        Me.cmdTakeControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTakeControl.Location = New System.Drawing.Point(12, 12)
        Me.cmdTakeControl.Name = "cmdTakeControl"
        Me.cmdTakeControl.Size = New System.Drawing.Size(139, 79)
        Me.cmdTakeControl.TabIndex = 1
        Me.cmdTakeControl.Text = "Take Control"
        Me.cmdTakeControl.UseVisualStyleBackColor = True
        '
        'frmTestMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(160, 107)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdTakeControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTestMode"
        Me.Text = "Test Mode"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdTakeControl As System.Windows.Forms.Button
End Class
