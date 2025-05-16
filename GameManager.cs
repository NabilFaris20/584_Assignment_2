using System;
using System.Collections.Generic;

namespace BoardGameFramework
{
    public class GameManager
    {
        private readonly IGame game;
        private readonly Stack<Move> undoStack = new();
        private readonly Stack<Move> redoStack = new();

        public GameManager(IGame game)
        {
            this.game = game;
        }

        public void Start()
        {
            while (true)
            {
                game.GameBoard.Display();

                int chosen;
                int row, col;

                if (game.CurrentPlayer is ComputerPlayer computer)
                {
                    Console.WriteLine($"{computer.Name}'s turn");
                    (chosen, row, col) = computer.MakeMove(game.GameBoard.boardGrid, game.BoardSize);
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
                            SaveManager.SaveGame(
                                game.GameBoard.boardGrid,
                                game.Player1.AvailableNumbers,
                                game.Player2.AvailableNumbers,
                                game.CurrentPlayer == game.Player1 ? "player1" : "player2",
                                game.Player2 is ComputerPlayer
                            );
                            game.GameBoard.Display();
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

                    (row, col) = InputHandler.GetPositionFromPlayer(game.BoardSize);
                }

                bool placed = GameUtils.PlaceNumber(game.GameBoard.boardGrid, row, col, chosen);
                if (placed)
                {
                    undoStack.Push(new Move(row, col, chosen, game.CurrentPlayer));
                    redoStack.Clear();

                    game.CurrentPlayer.RemoveNumber(chosen);
                    Console.WriteLine("Move successful");

                    if (game.IsWin())
                    {
                        game.GameBoard.Display();
                        Console.WriteLine($"{game.CurrentPlayer.Name} wins!");
                        break;
                    }

                    if (GameUtils.isBoardFull(game.GameBoard.boardGrid))
                    {
                        game.GameBoard.Display();
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

        private void HandleUndo()
        {
            if (undoStack.Count > 0)
            {
                Move lastMove = undoStack.Pop();
                game.GameBoard.boardGrid[lastMove.Row, lastMove.Col] = null;
                lastMove.Player.AvailableNumbers.Add(lastMove.Value);
                game.CurrentPlayer = lastMove.Player;
                redoStack.Push(lastMove);

                Console.WriteLine("Last move undone.");
                game.GameBoard.Display();
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }

        private void HandleRedo()
        {
            if (redoStack.Count > 0)
            {
                Move move = redoStack.Pop();
                game.GameBoard.boardGrid[move.Row, move.Col] = move.Value;
                move.Player.RemoveNumber(move.Value);
                undoStack.Push(move);
                game.CurrentPlayer = (move.Player == game.Player1) ? game.Player2 : game.Player1;

                Console.WriteLine("Move redone.");
                game.GameBoard.Display();
            }
            else
            {
                Console.WriteLine("Nothing to redo.");
            }
        }
    }
}
