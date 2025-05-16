using System.Globalization;
using BoardGameFramework;

namespace BoardGameFramework
{
    public class Program
    {
        public static void Main()
        {

            Console.WriteLine("Choose a game:");
            Console.WriteLine("1. Numerical Tic Tac Toe");
            Console.WriteLine("2. Gomoku");

            int gameChoice = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Do you want to load a saved game? (y/n):");
            string? loadChoice = Console.ReadLine()?.Trim().ToLower();

            IGame game;

              if (loadChoice == "y")
            {
                (Board board, Player player1, Player player2, Player currentPlayer) = SaveManager.LoadGame();

                game = gameChoice == 2
                    ? new GomokuGame(board, player1, player2, currentPlayer)
                    : new NumericalTicTacToeGame(board, player1, player2, currentPlayer);
            }
            else
            {
                game = gameChoice == 2
                    ? new GomokuGame()
                    : new NumericalTicTacToeGame();
            }

            GameManager manager = new GameManager(game);
            manager.Start();
        }
    }
}