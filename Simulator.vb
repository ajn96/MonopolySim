Public Class Simulator

    'track number of iterations
    Private m_stepCount As Integer
    'track current board position
    Private m_currentTile As Integer
    'data structure for the board
    Public board As List(Of GameTile)
    'Random generator
    Private Rnd As Random

    'constructor
    Sub New()
        'seed random
        Rnd = New Random(System.DateTime.Now.Millisecond)
        m_stepCount = 0
        m_currentTile = 0
        board = New List(Of GameTile)
        For i As Integer = 0 To 39
            board.Add(New GameTile(i))
        Next
    End Sub

    Sub StepSim()
        m_currentTile = m_currentTile + GetRoll()
        m_currentTile = m_currentTile Mod 40
        MoveToTile(m_currentTile)
    End Sub

    Public ReadOnly Property StepCount
        Get
            Return m_stepCount
        End Get
    End Property

    Private Sub MoveToTile(ByVal nextTile)
        m_stepCount = m_stepCount + 1
        board.ElementAt(nextTile).UseTile()
        m_currentTile = nextTile
        EvalSpecialTile()
    End Sub

    Private Function GetRoll() As Integer
        Dim Die1, Die2 As Integer
        Die1 = Rnd.Next(1, 6)
        Die2 = Rnd.Next(1, 6)
        Return Die1 + Die2
    End Function

    Private Sub EvalSpecialTile()
        Select Case m_currentTile
            Case 2, 17, 33
                EvalCommunityChest()
            Case 7, 22, 36
                EvalChance()
            Case 30
                EvalJail()
            Case Else
                'Not special
        End Select
    End Sub

    Private Sub EvalCommunityChest()
        Dim card = Rnd.Next(0, 16)
        Select Case card
            Case 0
                'Go
                MoveToTile(0)
            Case 5
                'Jail
                EvalJail()
            Case Else
                'Dont move
        End Select
    End Sub

    Private Sub EvalChance()
        Dim card = Rnd.Next(0, 15)
        Select Case card
            Case 0
                'Go
                MoveToTile(0)
            Case 1
                'Illinois
                MoveToTile(24)
            Case 2
                'St charles
                MoveToTile(11)
            Case 3
                'Utility
                If m_currentTile > 12 Then
                    MoveToTile(28)
                Else
                    MoveToTile(12)
                End If
            Case 4
                'railroad
                If m_currentTile < 5 Then
                    MoveToTile(5)
                ElseIf m_currentTile < 15 Then
                    MoveToTile(15)
                ElseIf m_currentTile < 25 Then
                    MoveToTile(25)
                ElseIf m_currentTile < 35 Then
                    MoveToTile(35)
                Else
                    MoveToTile(5)
                End If
            Case 7
                'Back 3
                MoveToTile(m_currentTile - 3)
            Case 8
                'Jail
                EvalJail()
            Case 11
                'reading
                MoveToTile(5)
            Case 12
                'boardwalk
                MoveToTile(39)
            Case Else
                'Do nothing
        End Select
    End Sub

    Private Sub EvalJail()
        MoveToTile(10)
        'Calculate the next move while in jail
        Dim buyout As Integer = Rnd.Next(0, 1)
        Dim nextMove As Integer
        Dim currentRoll As Integer = 0
        Dim roll1, roll2 As Integer

        'calculate non - buyout roll
        If buyout = 0 Then
            currentRoll = 0
            While currentRoll < 3
                roll1 = Rnd.Next(1, 6)
                roll2 = Rnd.Next(1, 6)
                If roll1 = roll2 Then
                    nextMove = roll1 + roll2
                    currentRoll = 3
                End If
            End While
        End If
        If buyout = 1 Or nextMove = 0 Then
            nextMove = GetRoll()
        End If
        MoveToTile(nextMove)
    End Sub

End Class
