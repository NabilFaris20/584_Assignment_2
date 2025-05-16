namespace BoardGameFramework
{
    public class GomokuGame : IGame
{
    public int BoardSize { get; private set; }
    public Board GameBoard { get; private set; } = null!;
    public Player Player1 { get; private set; } = null!;
    public Player Player2 { get; private set; } = null!;
    public Player CurrentPlayer { get; set; } = null!;

    public GomokuGame()
    {
        InitializeGame();
    }

    public GomokuGame(Board board, Player player1, Player player2, Player currentPlayer)
        {
            BoardSize = board.boardGrid.GetLength(0);
            GameBoard = board;
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = currentPlayer;
        }

    private void InitializeGame()
    {
        BoardSize = Board.BoardSize();
        GameBoard = new Board(BoardSize);

        Player1 = new SymbolPlayer("Player 1", 'X');
        Player2 = new SymbolPlayer("Player 2", 'O');

        CurrentPlayer = Player1;
    }

    public bool IsWin()
    {
        return GameUtils.CheckFiveInARow(GameBoard.boardGrid, BoardSize, ((SymbolPlayer)CurrentPlayer).Symbol);
    }

    public bool IsTie()
    {
        return GameUtils.isBoardFull(GameBoard.boardGrid);
    }

    public void DisplayMagicSum() { /* Not applicable, leave empty */ }
}

}
