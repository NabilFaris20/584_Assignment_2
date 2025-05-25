namespace BoardGameFramework{

    public class NotaktoGame : IGame
    {
        public NotaktoBoard[] Boards { get; private set; } = null!;
        public Player Player1 { get; private set; } = null!;
        public Player Player2 { get; private set; } = null!;
        public Player CurrentPlayer { get; set; } = null!;
        public List<(int board, int row, int col)> MoveHistory { get; private set; } = new();
        public int HistoryIndex { get; private set; } = -1;

        // --- IGame interface compatibility ---
        public int BoardSize => 3;
        public Board GameBoard => new NotaktoMultiBoard(Boards); // ✅ Use an inline subclass
        public void DisplayMagicSum() { }

        public NotaktoGame()
        {
            Console.WriteLine("Welcome to Notakto!");
            Console.WriteLine("Take turns placing an X on any of the 3 boards. The game ends when all 3 board have three-in-a-row, \nat which point the player to make the last move loses");
            InitializeGame();
        }

        public NotaktoGame(NotaktoBoard[] boards, Player player1, Player player2, Player currentPlayer)
        {
            Boards = boards;
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = currentPlayer;
        }

        private void InitializeGame()
        {
            Boards = new NotaktoBoard[3];
            for (int i = 0; i < 3; i++)
                Boards[i] = new NotaktoBoard();

            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1 = Human vs Human");
            Console.WriteLine("2 = Human vs Computer");

            int mode;
            while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 1 && mode != 2))
                Console.Write("Invalid input. Enter 1 or 2: ");

            Player1 = new HumanPlayer("Player 1", new List<int>());
            Player2 = (mode == 2)
                ? new NotaktoComputerPlayer("Computer", new List<int>())
                : new Player("Player 2", new List<int>());
            CurrentPlayer = Player1;
        }

        public bool IsWin()
        {
            return Boards.All(b => GameUtils.HasThreeInARow(b.Grid));
        }

        public bool IsTie() => false;

        public void TrackMove(int board, int row, int col)
        {
            if (HistoryIndex < MoveHistory.Count - 1)
                MoveHistory.RemoveRange(HistoryIndex + 1, MoveHistory.Count - HistoryIndex - 1);

            MoveHistory.Add((board, row, col));
            HistoryIndex = MoveHistory.Count - 1;
        }

       
    }

    public class NotaktoMultiBoard : Board
    {
        private readonly NotaktoBoard[] boards;

        public NotaktoMultiBoard(NotaktoBoard[] boards) : base(3)
        {
            this.boards = boards;
        }

        public override void Display()
        {
            Console.WriteLine("\n=== NOTAKTO BOARDS ===");

            for (int i = 0; i < boards.Length; i++)
            {
                Console.Write($"Board {i}".PadRight(10)); // Ensures even board titles
            }
            Console.WriteLine();

            for (int row = 0; row < 3; row++)
            {
                for (int b = 0; b < boards.Length; b++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        Console.Write((boards[b].Grid[row, col] ? "X" : "_").PadRight(3));
                    }
                    Console.Write("   "); // Space between boards
                }
                Console.WriteLine();
            }

            Console.WriteLine("=====================\n");
        }

    }


}