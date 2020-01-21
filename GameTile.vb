Public Class GameTile

    Private m_NumHits As Integer
    Private m_tileNum As Integer

    Sub New()
        Me.New(0)
    End Sub

    Sub New(ByVal tileNum As Integer)
        m_tileNum = tileNum
        m_NumHits = 0
        'Init GO at 1 hit
        If m_tileNum = 0 Then
            m_NumHits = 1
        End If
    End Sub

    Public Sub UseTile()
        m_NumHits = m_NumHits + 1
    End Sub

    Public ReadOnly Property NumHits As Integer
        Get
            Return m_NumHits
        End Get
    End Property

    Public Property TileNum As Integer
        Get
            Return m_tileNum
        End Get
        Set(value As Integer)
            m_tileNum = value
        End Set
    End Property

End Class