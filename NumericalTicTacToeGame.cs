namespace BoardGameFramework
{
    public class NumericalTicTacToeGame : IGame
    {
        public int BoardSize { get; private set; }
        public Board GameBoard { get; private set; } = null!;
        public Player Player1 { get; private set; } = null!;
        public Player Player2 { get; private set; } = null!;
        public Player CurrentPlayer { get; set; } = null!;

        public NumericalTicTacToeGame()
        {
            Console.WriteLine("Welcome to Numerical Tic Tac Toe!");
            Console.WriteLine("Take turns placing odd/even numbers to form a row, column, or diagonal that adds up to the magic sum.");
            InitializeGame();
        }

        public NumericalTicTacToeGame(Board board, Player player1, Player player2, Player currentPlayer)
        {
            if (board is NumericalBoard numericalBoard)
            {
                BoardSize = numericalBoard.Size;
                GameBoard = numericalBoard;
            }
            else
            {
                throw new ArgumentException("Expected a NumericalBoard instance.");
            }

            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = currentPlayer;
        }

        private void InitializeGame()
        {
            BoardSize = Board.BoardSize();
            GameBoard = new NumericalBoard(BoardSize);

            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1 = Human vs Human");
            Console.WriteLine("2 = Human vs Computer");

            int mode;
            while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 1 && mode != 2))
                Console.Write("Invalid input. Enter 1 or 2: ");

            List<int> allNumbers = Enumerable.Range(1, BoardSize * BoardSize).ToList();

            Player1 = new HumanPlayer("Player 1", allNumbers.Where(n => n % 2 == 1).ToList());

            Player2 = (mode == 2)
                ? new ComputerPlayer("Computer", allNumbers.Where(n => n % 2 == 0).ToList())
                : new HumanPlayer("Player 2", allNumbers.Where(n => n % 2 == 0).ToList());

            CurrentPlayer = Player1;
            DisplayMagicSum();
        }

        public bool IsWin()
        {
            var numericalBoard = (NumericalBoard)GameBoard;
            return GameUtils.CheckWin(numericalBoard.Grid, BoardSize);
        }

        public bool IsTie()
        {
            var numericalBoard = (NumericalBoard)GameBoard;
            return GameUtils.isBoardFull(numericalBoard.Grid);
        }

        public void DisplayMagicSum()
        {
            int magic = BoardSize * (BoardSize * BoardSize + 1) / 2;
            Console.WriteLine("Your magic sum is " + magic);
        }
    }
}
