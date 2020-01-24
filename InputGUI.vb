Imports System.Threading

Public Class InputGUI

    Private sim As Simulator
    Private numSteps As Integer

    Private Sub runButton_Click(sender As Object, e As EventArgs) Handles runButton.Click

        Dim simThread As Thread = New Thread(AddressOf RunSimWork)
        simThread.IsBackground = True
        runButton.Enabled = False
        simThread.Start()

    End Sub

    Private Sub RunSimWork()

        Dim progress, oldProgress As Integer

        Try
            numSteps = Convert.ToInt32(numMoves.Text)
            If numSteps < 1 Then Throw New ArgumentOutOfRangeException("Numsteps must be greater than 0")
        Catch ex As Exception
            MsgBox("Bad input value: " + ex.ToString())
        End Try

        sim = New Simulator()

        oldProgress = -1
        progress = 0
        While sim.StepCount < numSteps
            sim.StepSim()
            progress = CInt(sim.StepCount * 100 / numSteps)
            If progress > oldProgress Then
                oldProgress = progress
                Me.Invoke(Sub() UpdateProgress(progress))
            End If
            If RealTimeUpdates.Checked Then
                System.Threading.Thread.Sleep(1)
            End If
            If sim.StepCount Mod 100 = 0 Then
                Me.Invoke(Sub() UpdateTextBoxes())
            End If
        End While

        Me.Invoke(Sub() UpdateTextBoxes())
        Me.Invoke(Sub() enableRunBtn(True))
    End Sub

    Private Sub UpdateProgress(percent As Integer)
        simProgress.Value = percent
    End Sub

    Private Sub InputGUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sim = New Simulator()
        numMoves.Text = "10000"
        UpdateTextBoxes()
        simProgress.Maximum = 100
    End Sub

    Private Sub UpdateTextBoxes()
        'get the index values
        Dim hitCounts As New List(Of Integer)
        For Each tile In sim.board
            hitCounts.Add(tile.NumHits)
        Next
        hitCounts.Sort()
        Dim controlString As String
        Dim myControlToFind As Label
        For i As Integer = 0 To 39
            controlString = "lab" + i.ToString()
            myControlToFind = Me.Controls.Find(controlString, True).FirstOrDefault()
            myControlToFind.Text = i.ToString() + Environment.NewLine + (sim.board.ElementAt(i).NumHits * 100 / sim.StepCount).ToString("0.00") + "%"
            If sim.board.ElementAt(i).NumHits < hitCounts(8) Then
                myControlToFind.BackColor = Color.Red
            ElseIf sim.board.ElementAt(i).NumHits < hitCounts(32) Then
                myControlToFind.BackColor = Color.Yellow
            Else
                myControlToFind.BackColor = Color.Chartreuse
            End If
        Next

    End Sub

    Private Sub enableRunBtn(enabled As Boolean)
        runButton.Enabled = enabled
    End Sub

End Class
