namespace BoardGameFramework{
    public class ComputerPlayer : Player {

    public ComputerPlayer(string name, List<int> availableNumbers) : base(name, availableNumbers){
        
    }

    public (int, int, int) MakeMove(int?[,] boardGrid, int boardSize)
    {
        foreach (int number in AvailableNumbers)
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if (boardGrid[row, col] == null)
                    {
                        boardGrid[row, col] = number;
                        bool win = GameUtils.CheckWin(boardGrid, boardSize);
                        boardGrid[row, col] = null; 

                        if (win)
                        {
                            return (number, row, col);
                        }
                    }
                }
            }
        }

        Random rand = new Random();
        var emptyCells = new List<(int, int)>();

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (boardGrid[row, col] == null)
                {
                    emptyCells.Add((row, col));
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            var (row, col) = emptyCells[rand.Next(emptyCells.Count)];
            int randomNumber = AvailableNumbers[rand.Next(AvailableNumbers.Count)];
            return (randomNumber, row, col);
        }

        return (-1, -1, -1); 
    }
}

public class GomokuComputerPlayer : SymbolPlayer
{
    public GomokuComputerPlayer(char symbol) : base("Computer", symbol) { }

    public (int, int) MakeMove(char?[,] grid, int size)
    {
        // Try winning move
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (grid[row, col] == null)
                {
                    grid[row, col] = Symbol;
                    if (GameUtils.CheckFiveInARow(grid, size, Symbol))
                    {
                        grid[row, col] = null;
                        return (row, col);
                    }
                    grid[row, col] = null;
                }
            }
        }

        // Random fallback
        Random rand = new Random();
        List<(int, int)> validMoves = new();
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (grid[row, col] == null)
                    validMoves.Add((row, col));
            }
        }

        return validMoves[rand.Next(validMoves.Count)];
    }
}

public class NotaktoComputerPlayer : ComputerPlayer
    {
        public NotaktoComputerPlayer(string name, List<int> availableNumbers) : base(name, availableNumbers) { }

        public (int, int, int) MakeMove(NotaktoBoard[] boards)
        {
            Random rand = new Random();

            for (int boardIdx = 0; boardIdx < boards.Length; boardIdx++)
            {
                if (GameUtils.HasThreeInARow(boards[boardIdx].Grid))
                    continue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (!GameUtils.GetCell(boards[boardIdx].Grid, row, col))
                        {
                            GameUtils.HasThreeInARow(boards[boardIdx].Grid);
                            if (!GameUtils.HasThreeInARow(boards[boardIdx].Grid))
                            {
                                GameUtils.ClearCell(boards[boardIdx].Grid, row, col);
                                return (boardIdx, row, col);
                            }
                            GameUtils.ClearCell(boards[boardIdx].Grid, row, col);

                        }
                    }
                }
            }

            var validBoards = new List<int>();
            for (int i = 0; i < boards.Length; i++)
            {
                if (!GameUtils.HasThreeInARow(boards[i].Grid))
                    validBoards.Add(i);
            }

            if (validBoards.Count == 0)
                return (-1, -1, -1);

            int randomBoardIdx = validBoards[rand.Next(validBoards.Count)];

            var emptyCells = new List<(int, int)>();
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (!GameUtils.GetCell(boards[randomBoardIdx].Grid, row, col))
                        emptyCells.Add((row, col));
                }
            }

            if (emptyCells.Count == 0)
                return (-1, -1, -1);

            var (randomRow, randomCol) = emptyCells[rand.Next(emptyCells.Count)];
            return (randomBoardIdx, randomRow, randomCol);
        }
    }

}