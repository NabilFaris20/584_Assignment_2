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
            Console.WriteLine("3. Notakto");


            int gameChoice;
            while (!int.TryParse(Console.ReadLine(), out gameChoice) || gameChoice < 1 || gameChoice > 3)
            {
                Console.Write("Invalid input. Enter 1, 2, or 3: ");
            }
            
            Console.WriteLine("Do you want to load a saved game? (y/n):");
            string? loadChoice = Console.ReadLine()?.Trim().ToLower();

            IGame game;

              if (loadChoice == "y")
            {
                if (gameChoice == 1)
                {
                    var (board, player1, player2, currentPlayer) = SaveManager.LoadNumericalGame();
                    game = new NumericalTicTacToeGame(board, player1, player2, currentPlayer);
                }
                else if (gameChoice == 2)
                {
                    var (board, player1, player2, currentPlayer) = SaveManager.LoadGomokuGame();
                    game = new GomokuGame(board, player1, player2, currentPlayer);
                }
                else
                {
                    var (boards, player1, player2, currentPlayer) = SaveManager.LoadNotaktoGame();
                    game = new NotaktoGame(boards, player1, player2, currentPlayer);
                }

            }
            else
            {
                if (gameChoice == 1)
                    game = new NumericalTicTacToeGame();
                else if (gameChoice == 2)
                    game = new GomokuGame();
                else
                    game = new NotaktoGame();

            }

            if (gameChoice == 1)
            {
                NumericalGameManager numericalManager = new NumericalGameManager(game);
                numericalManager.Start();
            }
            else if (gameChoice == 2)
            {
                GomokuGameManager gomokuManager = new GomokuGameManager(game);
                gomokuManager.Start();
            }
            else
            {
                NotaktoGameManager notaktoManager = new NotaktoGameManager(game);
                notaktoManager.Start();
            }
        }
    }
}