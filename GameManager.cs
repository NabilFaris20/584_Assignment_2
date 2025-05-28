using System;
using System.Collections.Generic;

namespace BoardGameFramework
{
    public abstract class BaseGameManager
    {
        protected readonly IGame game;
        protected readonly Stack<Move> undoStack = new();
        protected readonly Stack<Move> redoStack = new();

        protected BaseGameManager(IGame game)
        {
            this.game = game;
        }

        public abstract void Start();

        protected virtual void HandleUndo()
        {
            if (undoStack.Count > 0)
            {
                // Handle double-undo for computer player
                if (game.Player2 is ComputerPlayer && game.CurrentPlayer == game.Player1 && undoStack.Count >= 2)
                {
                    // Undo computer move
                    Move compMove = undoStack.Pop();
                    if (game.GameBoard is NumericalBoard nb1)
                        nb1.Grid[compMove.Row, compMove.Col] = null;
                    else if (game.GameBoard is GomokuBoard gb1)
                        gb1.Grid[compMove.Row, compMove.Col] = null;
                    else if (game is NotaktoGame ng1)
                        ng1.Boards[compMove.BoardIndex].Grid[compMove.Row, compMove.Col] = false;

                    redoStack.Push(compMove);

                    // Undo human move
                    Move playerMove = undoStack.Pop();
                    if (game.GameBoard is NumericalBoard nb2)
                        nb2.Grid[playerMove.Row, playerMove.Col] = null;
                    else if (game.GameBoard is GomokuBoard gb2)
                        gb2.Grid[playerMove.Row, playerMove.Col] = null;
                    else if (game is NotaktoGame ng2)
                        ng2.Boards[playerMove.BoardIndex].Grid[playerMove.Row, playerMove.Col] = false;

                    if (!(playerMove.Player is SymbolPlayer))
                        playerMove.Player.AvailableNumbers.Add(playerMove.Value);

                    game.CurrentPlayer = playerMove.Player;
                    redoStack.Push(playerMove);

                    Console.WriteLine("Your last move and the computer's move have been undone.");
                }
                else
                {
                    Move lastMove = undoStack.Pop();

                    if (game.GameBoard is NumericalBoard nb)
                        nb.Grid[lastMove.Row, lastMove.Col] = null;
                    else if (game.GameBoard is GomokuBoard gb)
                        gb.Grid[lastMove.Row, lastMove.Col] = null;
                    else if (game is NotaktoGame ng)
                        ng.Boards[lastMove.BoardIndex].Grid[lastMove.Row, lastMove.Col] = false;

                    if (!(lastMove.Player is SymbolPlayer))
                        lastMove.Player.AvailableNumbers.Add(lastMove.Value);

                    game.CurrentPlayer = lastMove.Player;
                    redoStack.Push(lastMove);

                    Console.WriteLine("Last move undone.");
                }

                game.GameBoard.Display();
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }


        protected virtual void HandleRedo()
        {
            if (redoStack.Count > 0)
            {
                Move move = redoStack.Pop();

                if (game.GameBoard is NumericalBoard nb)
                    nb.Grid[move.Row, move.Col] = move.Value;
                else if (game.GameBoard is GomokuBoard gb)
                    gb.Grid[move.Row, move.Col] = (char?)move.Value;
                else if (game is NotaktoGame ng)
                    ng.Boards[move.BoardIndex].Grid[move.Row, move.Col] = true;

                if (!(game.CurrentPlayer is SymbolPlayer))
                    move.Player.RemoveNumber(move.Value);

                undoStack.Push(move);
                game.CurrentPlayer = move.Player == game.Player1 ? game.Player2 : game.Player1;

                Console.WriteLine("Move redone.");
                game.GameBoard.Display();
            }
            else
            {
                Console.WriteLine("Nothing to redo.");
            }
        }

    }

    public class NumericalGameManager : BaseGameManager
    {
        public NumericalGameManager(IGame game) : base(game) { }

        public override void Start()
        {
            NumericalBoard board = (NumericalBoard)game.GameBoard;

            while (true)
            {
                board.Display();

                int chosen;
                int row, col;

                if (game.CurrentPlayer is ComputerPlayer computer)
                {
                    Console.WriteLine($"{computer.Name}'s turn");
                    (chosen, row, col) = computer.MakeMove(board.Grid, game.BoardSize);
                    Console.WriteLine($"Computer chose number {chosen} at row {row}, column {col}");
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine($"{game.CurrentPlayer.Name}, it's your turn.");
                        Console.WriteLine("Available numbers: " + string.Join(", ", game.CurrentPlayer.AvailableNumbers));
                        Console.WriteLine("Enter a number, or type 'save', 'undo', 'redo', or 'help':");

                        string? input = Console.ReadLine()?.Trim().ToLower();

                        if (input == "save")
                        {
                            SaveManager.SaveNumericalGame(
                                board.Grid,
                                game.Player1.AvailableNumbers,
                                game.Player2.AvailableNumbers,
                                game.CurrentPlayer == game.Player1 ? "player1" : "player2",
                                game.Player2 is ComputerPlayer
                            );
                            board.Display();
                            continue;
                        }

                        if (input == "help")
                        {
                            HelpMenu.Show();
                            continue;
                        }

                        if (input == "undo")
                        {
                            HandleUndo();
                            continue;
                        }

                        if (input == "redo")
                        {
                            HandleRedo();
                            continue;
                        }

                        if (int.TryParse(input, out chosen) && game.CurrentPlayer.AvailableNumbers.Contains(chosen))
                        {
                            break;
                        }

                        Console.WriteLine("Invalid input. Try again.");
                    }

                    (row, col) = InputHandler.GetPositionFromPlayer(game.BoardSize, "numerical");
                }

                bool placed = GameUtils.PlaceNumber(board.Grid, row, col, chosen);
                if (placed)
                {
                    undoStack.Push(new Move(row, col, chosen, game.CurrentPlayer));
                    redoStack.Clear();

                    game.CurrentPlayer.RemoveNumber(chosen);
                    Console.WriteLine("Move successful");

                    if (game.IsWin())
                    {
                        board.Display();
                        Console.WriteLine($"{game.CurrentPlayer.Name} wins!");
                        break;
                    }

                    if (GameUtils.isBoardFull(board.Grid))
                    {
                        board.Display();
                        Console.WriteLine("The game is a tie!");
                        break;
                    }

                    game.CurrentPlayer = (game.CurrentPlayer == game.Player1) ? game.Player2 : game.Player1;
                    Console.WriteLine("Changed player");
                }
                else
                {
                    Console.WriteLine("That spot is already taken");
                }
            }
        }
    }

    public class GomokuGameManager : BaseGameManager
    {
        public GomokuGameManager(IGame game) : base(game) { }

        public override void Start()
        {
            GomokuBoard board = (GomokuBoard)game.GameBoard;

            while (true)
            {
                board.Display();

                int row, col;
                char symbol = ((SymbolPlayer)game.CurrentPlayer).Symbol;

                if (game.CurrentPlayer is GomokuComputerPlayer computer)
                {
                    Console.WriteLine($"{computer.Name} ({symbol}) is thinking...");
                    Thread.Sleep(800);  

                    (row, col) = computer.MakeMove(board.Grid, game.BoardSize);
                    Console.WriteLine($"Computer placed at ({row}, {col})");
                }
                else
                {
                    Console.WriteLine($"{game.CurrentPlayer.Name} ({symbol}), it's your turn.");
                    Console.WriteLine("Type 'save', 'undo', 'redo', 'help', or press Enter to place symbol");

                    string? input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "save")
                    {
                        SaveManager.SaveGomokuGame(
                            board.Grid,
                            game.CurrentPlayer == game.Player1 ? "player1" : "player2"
                        );
                        board.Display();
                        continue;
                    }

                    if (input == "help")
                    {
                        HelpMenu.Show();
                        continue;
                    }

                    if (input == "undo")
                    {
                        HandleUndo();
                        continue;
                    }

                    if (input == "redo")
                    {
                        HandleRedo();
                        continue;
                    }

                    (row, col) = InputHandler.GetPositionFromPlayer(game.BoardSize, "gomoku");
                }

                bool placed = GameUtils.PlaceSymbol(board.Grid, row, col, symbol);
                if (placed)
                {
                    undoStack.Push(new Move(row, col, symbol, game.CurrentPlayer));
                    redoStack.Clear();

                    Console.WriteLine("Move successful");

                    if (game.IsWin())
                    {
                        board.Display();
                        Console.WriteLine($"{game.CurrentPlayer.Name} wins!");
                        break;
                    }

                    if (GameUtils.isBoardFullGomoku(board.Grid))
                    {
                        board.Display();
                        Console.WriteLine("The game is a tie!");
                        break;
                    }

                    game.CurrentPlayer = (game.CurrentPlayer == game.Player1) ? game.Player2 : game.Player1;
                    Console.WriteLine("Changed player");
                }
                else
                {
                    Console.WriteLine("That spot is already taken");
                }
            }
        }

    }

    public class NotaktoGameManager : BaseGameManager
    {
        private NotaktoGame notaktoGame;

        public NotaktoGameManager(IGame game) : base(game)
        {
            notaktoGame = (NotaktoGame)game;
        }

        public override void Start()
        {
            while (true)
            {
                game.GameBoard.Display();

                if (game.IsWin())
                {
                    Console.WriteLine($"{game.CurrentPlayer.Name} loses the game!");
                    break;
                }

                Console.WriteLine($"{game.CurrentPlayer.Name}'s turn");

                int board, row, col;

                // Handle Computer Player
                if (game.CurrentPlayer is NotaktoComputerPlayer computer)
                {
                    (board, row, col) = computer.MakeMove(notaktoGame.Boards);
                    Console.WriteLine($"Computer chose board {board}, row {row}, col {col}");

                    GameUtils.PlaceX(notaktoGame.Boards[board].Grid, row, col);

                    undoStack.Push(new Move(row, col, 1, game.CurrentPlayer) { BoardIndex = board }); 
                    redoStack.Clear();

                    game.CurrentPlayer = game.Player1; 
                    continue;
                }
                else
                {
                    Console.WriteLine("Enter move as 'board,row,col'. Rows/Cols are 0 to 2. or type 'save', 'undo', 'redo', 'help':");
                    string? input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "save")
                    {
                        SaveManager.SaveNotaktoGame(
                        notaktoGame.Boards.Select(b => b.Grid).ToArray(),
                        game.CurrentPlayer == game.Player1 ? "player1" : "player2",
                        game.Player2 is NotaktoComputerPlayer
                    );

                        continue;
                    }

                    if (input == "help")
                    {
                        HelpMenu.Show();
                        continue;
                    }

                    if (input == "undo")
                    {
                        HandleUndo();
                        continue;
                    }

                    if (input == "redo")
                    {
                        HandleRedo();
                        continue;
                    }

                    if (!InputHandler.TryParseMove(input, out board, out row, out col))
                    {
                        Console.WriteLine("Invalid input format. Use 'board,row,col'.");
                        continue;
                    }

                    if (board < 0 || board >= notaktoGame.Boards.Length)
                    {
                        Console.WriteLine("Invalid board index.");
                        continue;
                    }

                    if (GameUtils.HasThreeInARow(notaktoGame.Boards[board].Grid))
                    {
                        Console.WriteLine("This board already has a 3-in-a-row. Choose another.");
                        continue;
                    }
                }

                bool placed = GameUtils.PlaceX(notaktoGame.Boards[board].Grid, row, col);
                if (placed)
                {
                    undoStack.Push(new Move(row, col, 1, game.CurrentPlayer) { BoardIndex = board }); 
                    redoStack.Clear();

                    game.CurrentPlayer = game.CurrentPlayer == game.Player1 ? game.Player2 : game.Player1;
                }
                else
                {
                    Console.WriteLine("That spot is already taken or invalid.");
                }
            }
        }


       
    }
}