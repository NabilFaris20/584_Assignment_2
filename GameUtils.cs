namespace BoardGameFramework
{
    public static class GameUtils
    {
        // ---------- NUMERICAL GAME UTILS ----------

        public static bool PlaceNumber(int?[,] grid, int row, int col, int value)
        {
            if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1))
                return false;

            if (grid[row, col] != null)
                return false;

            grid[row, col] = value;
            return true;
        }

        public static bool CheckWin(int?[,] grid, int boardSize)
        {
            int magicSum = boardSize * (boardSize * boardSize + 1) / 2;

            for (int i = 0; i < boardSize; i++)
            {
                int rowSum = 0;
                int colSum = 0;
                bool rowComplete = true;
                bool colComplete = true;

                for (int j = 0; j < boardSize; j++)
                {
                    if (grid[i, j] == null)
                        rowComplete = false;
                    else
                        rowSum += grid[i, j].Value;

                    if (grid[j, i] == null)
                        colComplete = false;
                    else
                        colSum += grid[j, i].Value;
                }

                if ((rowComplete && rowSum == magicSum) || (colComplete && colSum == magicSum))
                    return true;
            }

            int diagSum1 = 0;
            int diagSum2 = 0;
            bool diag1Complete = true;
            bool diag2Complete = true;

            for (int i = 0; i < boardSize; i++)
            {
                if (grid[i, i] == null)
                    diag1Complete = false;
                else
                    diagSum1 += grid[i, i].Value;

                if (grid[i, boardSize - i - 1] == null)
                    diag2Complete = false;
                else
                    diagSum2 += grid[i, boardSize - i - 1].Value;
            }

            return (diag1Complete && diagSum1 == magicSum) || (diag2Complete && diagSum2 == magicSum);
        }

        public static bool isBoardFull(int?[,] grid)
        {
            foreach (var cell in grid)
            {
                if (cell == null)
                    return false;
            }
            return true;
        }

        // ---------- GOMOKU GAME UTILS ----------

        public static bool PlaceSymbol(char?[,] grid, int row, int col, char symbol)
        {
            if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1))
                return false;

            if (grid[row, col] != null)
                return false;

            grid[row, col] = symbol;
            return true;
        }

        public static bool CheckFiveInARow(char?[,] grid, int size, char symbol)
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (grid[row, col] == symbol)
                    {
                        if (CheckDirection(grid, row, col, symbol, 1, 0) ||
                            CheckDirection(grid, row, col, symbol, 0, 1) ||
                            CheckDirection(grid, row, col, symbol, 1, 1) ||
                            CheckDirection(grid, row, col, symbol, 1, -1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool isBoardFullGomoku(char?[,] grid)
        {
            foreach (var cell in grid)
            {
                if (cell == null)
                    return false;
            }
            return true;
        }

        private static bool CheckDirection(char?[,] grid, int row, int col, char symbol, int dRow, int dCol)
        {
            int size = grid.GetLength(0);
            for (int i = 0; i < 5; i++)
            {
                int r = row + i * dRow;
                int c = col + i * dCol;

                if (r < 0 || c < 0 || r >= size || c >= size || grid[r, c] != symbol)
                    return false;
            }
            return true;
        }

        // ---------- NOTAKTO GAME UTILS ----------

        public static bool PlaceX(bool[,] grid, int row, int col)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
                return false;

            if (grid[row, col])
                return false;

            grid[row, col] = true;
            return true;
        }

        public static void ClearCell(bool[,] grid, int row, int col)
        {
            if (row >= 0 && row < 3 && col >= 0 && col < 3)
                grid[row, col] = false;
        }

        public static bool GetCell(bool[,] grid, int row, int col)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
                return false;

            return grid[row, col];
        }

        public static bool HasThreeInARow(bool[,] grid)
        {
            for (int i = 0; i < 3; i++)
                if (grid[i, 0] && grid[i, 1] && grid[i, 2]) return true;

            for (int i = 0; i < 3; i++)
                if (grid[0, i] && grid[1, i] && grid[2, i]) return true;

            if (grid[0, 0] && grid[1, 1] && grid[2, 2]) return true;
            if (grid[0, 2] && grid[1, 1] && grid[2, 0]) return true;

            return false;
        }

        public static bool IsBoardFullNotakto(bool[,] grid)
        {
            foreach (var cell in grid)
                if (!cell) return false;

            return true;
        }

    }

    
}
