Public Class InputGUI

    Private sim As Simulator
    Private numSteps As Integer

    Private Sub runButton_Click(sender As Object, e As EventArgs) Handles runButton.Click
        Try
            numSteps = Convert.ToInt32(numMoves.Text)
        Catch ex As Exception
            MsgBox("Bad input value")
            Exit Sub
        End Try

        sim = New Simulator()

        While sim.StepCount < numSteps
            sim.StepSim()
        End While

        UpdateTextBoxes()

        MsgBox("Done!")

    End Sub

    Private Sub InputGUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sim = New Simulator()
        numMoves.Text = "10000"
        UpdateTextBoxes()
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
            myControlToFind.Text = i.ToString() + Environment.NewLine + (sim.board.ElementAt(i).NumHits * 100 / numSteps).ToString("0.00") + "%"
            If sim.board.ElementAt(i).NumHits < hitCounts(10) Then
                myControlToFind.BackColor = Color.Red
            ElseIf sim.board.ElementAt(i).NumHits < hitCounts(30) Then
                myControlToFind.BackColor = Color.Yellow
            Else
                myControlToFind.BackColor = Color.Chartreuse
            End If
        Next

    End Sub

End Class
