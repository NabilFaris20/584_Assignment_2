using System.Globalization;
public class Program{

    static void Main()
        {
            int size;
            Stack<Move> undoStack = new();
            Stack<Move> redoStack = new();

            Console.WriteLine("Do you want to load a saved game? (y/n):");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            Board board;
            Player player1;
            Player player2;
            Player currentPlayer;

            if (choice == "y")
            {
                (board, player1, player2, currentPlayer) = SaveManager.LoadGame();
                size = board.boardGrid.GetLength(0);
            }
            else
            {

                size = Board.BoardSize();
                board = new Board(size);
                Console.WriteLine("Choose game mode:");
                Console.WriteLine("1 = Human vs Human");
                Console.WriteLine("2 = Human vs Computer"); 

                int mode = 1;

                while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 1 && mode != 2))
                {
                    Console.Write("Invalid input. Enter 1 or 2: ");
                }
                
                
                

                List<int> allNumbers = new List<int>();
                for(int i = 1; i <= size * size; i++){
                    allNumbers.Add(i);
                }



                player1 = new Player("Player 1", allNumbers.Where(n => n % 2 == 1).ToList());
                

                if (mode == 2)
                {
                    player2 = new ComputerPlayer("Computer", allNumbers.Where(n => n % 2 == 0).ToList());
                }
                else
                {
                    player2 = new Player("Player 2", allNumbers.Where(n => n % 2 == 0).ToList());
                }
                
                Console.WriteLine("Your magic sum is " + (size * (size * size + 1) / 2));

                currentPlayer = player1;

                
            }

            while (true)
                {
                    board.Display();

                    int chosen;
                    int row;
                    int col;

                    if (currentPlayer is ComputerPlayer computer)
                    {
                        Console.WriteLine(computer.Name + "'s turn");
                        (chosen, row, col) = computer.MakeMove(board.boardGrid, size);
                        Console.WriteLine("Computer chose number " + chosen + " at row " + row + " and column " + col);
                    }
                    else
                    {
                        while (true)
                        {
                            Console.WriteLine( currentPlayer.Name + " it's your turn.");
                            Console.WriteLine("Available numbers: " + string.Join(", ", currentPlayer.AvailableNumbers));
                            Console.WriteLine("Enter a number from your list, or type 'save' or 'help' or 'undo' or 'redo':");
                            string? input = Console.ReadLine();

                            if (input?.Trim().ToLower() == "save")
                            {
                                SaveManager.SaveGame(
                                    board.boardGrid,
                                    player1.AvailableNumbers,
                                    player2.AvailableNumbers,
                                    currentPlayer == player1 ? "player1" : "player2",
                                    player2 is ComputerPlayer
                                );
                                
                                board.Display(); 
                                continue;
                            }

                            if (input == "help")
                            {
                                Game.ShowHelp();
                                continue;
                            }

                            if (int.TryParse(input, out chosen) && currentPlayer.AvailableNumbers.Contains(chosen))
                            {
                                break; 
                            }

                            if (input?.Trim().ToLower() == "undo")
                            {
                                if (undoStack.Count > 0)
                                {
                                    Move lastMove = undoStack.Pop();
                                    board.boardGrid[lastMove.Row, lastMove.Col] = null;

                                    lastMove.Player.AvailableNumbers.Add(lastMove.Value);  // ⬅️ fix

                                    redoStack.Push(lastMove);
                                    currentPlayer = lastMove.Player;
                                    Console.WriteLine("Last move undone.");
                                    board.Display();

                                }
                                else
                                {
                                    Console.WriteLine("Nothing to undo.");
                                }
                                continue;
                            }

                            if (input == "redo")
                            {
                                if (redoStack.Count > 0)
                                {
                                    Move redoMove = redoStack.Pop();
                                    board.boardGrid[redoMove.Row, redoMove.Col] = redoMove.Value;

                                    redoMove.Player.RemoveNumber(redoMove.Value);  // ⬅️ fix

                                    undoStack.Push(redoMove);
                                    currentPlayer = (redoMove.Player == player1) ? player2 : player1;
                                    Console.WriteLine("Move redone.");
                                    board.Display();

                                }
                                else
                                {
                                    Console.WriteLine("Nothing to redo.");
                                }
                                continue;
                            }

                            

                            Console.WriteLine("Invalid input. Try again.");
                        }

                        (row, col) = Game.GetPositionFromPlayer(size);
                    }

                    bool placed = Game.PlaceNumber(board.boardGrid, row, col, chosen);
                    undoStack.Push(new Move(row, col, chosen, currentPlayer));
                    redoStack.Clear(); // Redo is reset after a new move


                    if (placed)
                    {
                        currentPlayer.RemoveNumber(chosen);
                        Console.WriteLine("Move successful");

                        if (Game.CheckWin(board.boardGrid, size))
                        {
                            board.Display();
                            Console.WriteLine( currentPlayer.Name + " wins!");
                            break;
                        }

                        if (Game.isBoardFull(board.boardGrid))
                        {
                            board.Display();
                            Console.WriteLine("The game is a tie!");
                            break;
                        }

                        currentPlayer = (currentPlayer == player1) ? player2 : player1;
                        Console.WriteLine("Changed player");
                    }
                    else
                    {
                        Console.WriteLine("That spot is already taken");
                    }
                }

            
            
            
        }

}

public class Move
{
    public int Row { get; }
    public int Col { get; }
    public int Value { get; }
    public Player Player { get; }

    public Move(int row, int col, int value, Player player)
    {
        Row = row;
        Col = col;
        Value = value;
        Player = player;
    }
}
public class Game{

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

    public static bool PlaceNumber(int?[,] boardgrid, int row, int col, int number)
    {
        if (row < 0 || row >= boardgrid.GetLength(0) || col < 0 || col >= boardgrid.GetLength(1)){
            return false;
        }
            
        if (boardgrid[row, col] != null){
            return false;
        }  

        boardgrid[row, col] = number;
        return true;
    }

    // public bool CheckWin()
    // {

    // }

    public static bool CheckWin(int? [,] boardGrid, int boardsize)
    {
        int magicSum = boardsize * (boardsize * boardsize + 1) / 2;

        for(int i = 0; i < boardsize; i++){

            int sum = 0;
            bool complete = true;

            for(int j = 0; j < boardsize; j++){
                if(boardGrid[i,j] == null){
                    complete = false;
                    break;
                }
                int cellValue = boardGrid[i, j]!.Value;
                sum += cellValue;
            }

            if(complete && sum == magicSum){
                return true;
            }
        }

        for(int j = 0; j < boardsize; j++){

            int sum = 0;
            bool complete = true;

            for(int i = 0; i < boardsize; i++){
                if(boardGrid[i,j] == null){
                    complete = false;
                    break;
                }
                int cellValue = boardGrid[i, j]!.Value;
                sum += cellValue;
            }

            if(complete && sum == magicSum){
                return true;
            }
        }

        //check fro diagonal
        int diagSum1 = 0;
        bool diagComplete = true;

        for(int i = 0; i < boardsize; i++){
            if(boardGrid[i, i] == null){
                diagComplete = false;
                break;
            }
            int cellValue = boardGrid[i, i]!.Value;
            diagSum1 += cellValue;
        }
        if(diagComplete && diagSum1 == magicSum){
            return true;
        }

        //check for other diagonal
        int diagSum2 = 0;
        bool diagComplete2 = true;

        for(int i = 0; i < boardsize; i++){
            if(boardGrid[i, i] == null){
                diagComplete2 = false;
                break;
            }
            int cellValue = boardGrid[i, i]!.Value;
            diagSum2 += cellValue;
        }
        if(diagComplete2 && diagSum2 == magicSum){
            return true;
        }

        
        

        return false;
    }

    public static bool isBoardFull(int?[,] boardGrid)
    {
        foreach (var cell in boardGrid)
        {
            if (cell == null){
                return false;
            }
        }
        return true;
    }

    public static void ShowHelp()
    {
        Console.WriteLine("----- HELP MENU -----");
        Console.WriteLine("Type a number from your available list to play.");
        Console.WriteLine("Type 'save' to save the game.");
        Console.WriteLine("Type 'help' to view this help menu again.");
        Console.WriteLine("Win by forming a line that adds up to the magic sum.");
        Console.WriteLine("---------------------");
    }


}

public class Board
{
    internal int?[,] boardGrid;



        public Board(int size)
        {
            boardGrid = new int?[size, size];
        }

        public static int BoardSize()
        {
            int size;
            string? input;

            Console.WriteLine("Enter board size: ");
            input = Console.ReadLine();
            
            while(!int.TryParse(input, out size) || size <= 0){
                Console.Write("Invalid input. Please enter a valid number: ");
                input = Console.ReadLine();
            }

            return size;

        }

        public void Display()
        {   
            for (int i = 0; i < boardGrid.GetLength(0); i++)
            {
                for (int j = 0; j < boardGrid.GetLength(1); j++)
                {
                    Console.Write(boardGrid[i, j]?.ToString() ?? "_");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

}

public class Player{

    public string Name {get;}
    public List<int> AvailableNumbers {get; private set;}

    public Player(string name, List<int> availableNumbers){

        Name = name;
        AvailableNumbers = availableNumbers;

    }

    public void RemoveNumber(int number){
        AvailableNumbers.Remove(number);
    }

    // public void ShowNumbers()
    // {
    //    Console.WriteLine($"{Name}'s available numbers: {string.Join(", ", AvailableNumbers)}");
    // }
    

}

public class HumanPlayer{

}

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
                        bool win = Game.CheckWin(boardGrid, boardSize);
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



