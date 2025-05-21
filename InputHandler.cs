namespace BoardGameFramework
{
    public static class InputHandler
    {
        // Only used in Numerical Tic Tac Toe
        public static int GetPlayerNumber(Player player)
        {
            int number;

            Console.WriteLine("Choose a number from your list:");
            Console.WriteLine(string.Join(", ", player.AvailableNumbers));
            string? input = Console.ReadLine();

            while (!int.TryParse(input, out number) || !player.AvailableNumbers.Contains(number))
            {
                Console.Write("Invalid number. Choose again: ");
                input = Console.ReadLine();
            }

            return number;
        }

        // Used in both games, but you pass game type for proper prompt
        public static (int, int) GetPositionFromPlayer(int boardSize, string gameType = "numerical")
        {
            if (gameType.ToLower() == "gomoku")
            {
                Console.WriteLine("Enter row and column to place your symbol (e.g., X or O).");
            }
            else
            {
                Console.WriteLine("Enter row and column to place your number.");
            }

            int row = GetCoordinate("row", boardSize);
            int col = GetCoordinate("column", boardSize);
            return (row, col);
        }

        // Optional: Reusable command handler
        public static string GetActionChoice()
        {
            Console.WriteLine("Type 'save', 'undo', 'redo', 'help', or press Enter to proceed:");
            return Console.ReadLine()?.Trim().ToLower() ?? "";
        }

        // Internal coordinate input logic
        private static int GetCoordinate(string label, int boardSize)
        {
            int value;
            Console.WriteLine($"Enter {label} (0 to {boardSize - 1}):");

            while (!int.TryParse(Console.ReadLine(), out value) || value < 0 || value >= boardSize)
            {
                Console.Write($"Invalid {label}. Enter again (0 to {boardSize - 1}): ");
            }

            return value;
        }

        public static bool TryParseMove(string input, out int board, out int row, out int col)
        {
            board = row = col = -1;

            string[] parts = input.Split(',');
            if (parts.Length != 3)
                return false;

            return int.TryParse(parts[0], out board) &&
                   int.TryParse(parts[1], out row) &&
                   int.TryParse(parts[2], out col);
        }
    }
}
