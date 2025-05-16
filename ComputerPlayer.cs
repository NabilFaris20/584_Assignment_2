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
}