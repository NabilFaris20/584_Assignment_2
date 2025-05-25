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
            Console.WriteLine("Welcome to Gomoku!");
            Console.WriteLine("Take turns placing your symbol on the board — first to get five in a row wins.");
            InitializeGame();
        }

        public GomokuGame(Board board, Player player1, Player player2, Player currentPlayer)
        {
            if (board is GomokuBoard gomokuBoard)
            {
                BoardSize = gomokuBoard.Size;
                GameBoard = gomokuBoard;
            }
            else
            {
                throw new ArgumentException("Expected a GomokuBoard instance.");
            }

            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = currentPlayer;
        }

        private void InitializeGame()
        {
            BoardSize = 15;
            GameBoard = new GomokuBoard(BoardSize);

            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1 = Human vs Human");
            Console.WriteLine("2 = Human vs Computer");

            int mode;
            while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 1 && mode != 2))
                Console.Write("Invalid input. Enter 1 or 2: ");

            Player1 = new SymbolPlayer("Player 1", 'X');

            Player2 = (mode == 2)
                ? new GomokuComputerPlayer('O')
                : new SymbolPlayer("Player 2", 'O');

            CurrentPlayer = Player1;
        }

        public bool IsWin()
        {
            var gomokuBoard = (GomokuBoard)GameBoard;
            return GameUtils.CheckFiveInARow(gomokuBoard.Grid, BoardSize, ((SymbolPlayer)CurrentPlayer).Symbol);
        }

        public bool IsTie()
        {
            var gomokuBoard = (GomokuBoard)GameBoard;
            return GameUtils.isBoardFullGomoku(gomokuBoard.Grid);
        }

        public void DisplayMagicSum()
        {
            // Not applicable for Gomoku
        }
    }
}
