namespace BoardGameFramework{
    public class SaveManager{
    public static void SaveGame(
        int?[,] board,
        List<int> player1Numbers,
        List<int> player2Numbers,
        string currentTurn,
        bool isComputer,
        string path = "AssignmentGameSave.txt")
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            int size = board.GetLength(0);
            writer.WriteLine(size);
            writer.WriteLine(string.Join(",", player1Numbers));
            writer.WriteLine(string.Join(",", player2Numbers));
            writer.WriteLine(currentTurn); 
            writer.WriteLine(isComputer.ToString().ToLower());

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    writer.Write(board[i, j]?.ToString() ?? "_");
                    if (j < size - 1) writer.Write(",");
                }
                writer.WriteLine();
            }
        }

        Console.WriteLine("Game saved, returning to game");
    }

    public static (Board board, Player player1, Player player2, Player currentPlayer) LoadGame(string path = "AssignmentGameSave.txt")
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Save file not found");
            Environment.Exit(0);
        }

        string[] lines = File.ReadAllLines(path);

        int size = int.Parse(lines[0]);
        var player1Numbers = lines[1].Split(',').Select(int.Parse).ToList();
        var player2Numbers = lines[2].Split(',').Select(int.Parse).ToList();
        string turn = lines[3].Trim().ToLower();
        bool isComputer = bool.Parse(lines[4].Trim());

        Board board = new Board(size);
        for (int i = 0; i < size; i++)
        {
            string[] row = lines[5 + i].Split(',');
            for (int j = 0; j < size; j++)
            {
                board.boardGrid[i, j] = row[j] == "_" ? null : int.Parse(row[j]);
            }
        }

        Player player1 = new Player("Player 1", player1Numbers);
        Player player2;

        if (isComputer)
        {
            player2 = new ComputerPlayer("Computer", player2Numbers);
        }
        else
        {
            player2 = new Player("Player 2", player2Numbers);
        }

        Player currentPlayer = (turn == "player1") ? player1 : player2;

        Console.WriteLine("Game loaded successfully");
        return (board, player1, player2, currentPlayer);
    }
}
}