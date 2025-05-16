namespace BoardGameFramework{
    public static class InputHandler{
        public static int GetPlayerNumber(Player player)
    {
        int number;

        Console.WriteLine("Choose a number from your list: ");
        Console.WriteLine(string.Join(", ", player.AvailableNumbers));
        string? input = Console.ReadLine();

        while (!int.TryParse(input, out number) || !player.AvailableNumbers.Contains(number))
        {
            Console.Write("Invalid number. Choose again: ");
            input = Console.ReadLine();
        }

        return number;
    }

    public static (int, int) GetPositionFromPlayer(int boardsize)
    {
        int row, column;

        Console.WriteLine("Which row would you like to place this number?");
        Console.WriteLine("Enter row 0 to " + (boardsize - 1));
        while(!int.TryParse(Console.ReadLine(), out row) || row < 0 || row >= boardsize)
        {
            Console.Write("Invalid row. Enter again: ");    
        }

        Console.WriteLine("Which column would you like to place this number?");
        Console.WriteLine("Enter column 0 to " + (boardsize - 1));
        while(!int.TryParse(Console.ReadLine(), out column) || column < 0 || column >= boardsize)
        {
            Console.Write("Invalid column. Enter again: ");    
        }

        return (row, column);
    }
    }
}