using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoardGameFramework;

namespace BoardGameFramework
{
    public static class SaveManager
    {
        // --------------------- NUMERICAL GAME ---------------------
        public static void SaveNumericalGame(int?[,] board, List<int> p1Nums, List<int> p2Nums, string currentTurn, bool isComputer, string path = "NumericalSave.txt")
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                int size = board.GetLength(0);
                writer.WriteLine(size);
                writer.WriteLine(string.Join(",", p1Nums));
                writer.WriteLine(string.Join(",", p2Nums));
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

            Console.WriteLine("Numerical Tic Tac Toe game saved.");
        }

        public static (Board board, Player player1, Player player2, Player currentPlayer) LoadNumericalGame(string path = "NumericalSave.txt")
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Numerical save file not found.");
                Environment.Exit(0);
            }

            string[] lines = File.ReadAllLines(path);

            int size = int.Parse(lines[0]);
            var player1Numbers = lines[1].Split(',').Select(int.Parse).ToList();
            var player2Numbers = lines[2].Split(',').Select(int.Parse).ToList();
            string turn = lines[3].Trim().ToLower();
            bool isComputer = bool.Parse(lines[4].Trim());

            NumericalBoard board = new NumericalBoard(size);

            for (int i = 0; i < size; i++)
            {
                string[] row = lines[5 + i].Split(',');
                for (int j = 0; j < size; j++)
                {
                    board.Grid[i, j] = row[j] == "_" ? null : int.Parse(row[j]);
                }
            }

            Player player1 = new HumanPlayer("Player 1", player1Numbers);
            Player player2 = isComputer
                ? new ComputerPlayer("Computer", player2Numbers)
                : new HumanPlayer("Player 2", player2Numbers);

            Player currentPlayer = (turn == "player1") ? player1 : player2;

            Console.WriteLine("Numerical game loaded successfully.");
            return (board, player1, player2, currentPlayer);
        }

        // --------------------- GOMOKU GAME ---------------------
        public static void SaveGomokuGame(char?[,] board, string currentTurn, string path = "GomokuSave.txt")
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                int size = board.GetLength(0);
                writer.WriteLine(size);
                writer.WriteLine(currentTurn);

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

            Console.WriteLine("Gomoku game saved.");
        }

        public static (Board board, Player player1, Player player2, Player currentPlayer) LoadGomokuGame(string path = "GomokuSave.txt")
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Gomoku save file not found.");
                Environment.Exit(0);
            }

            string[] lines = File.ReadAllLines(path);

            int size = int.Parse(lines[0]);
            string turn = lines[1].Trim().ToLower();

            GomokuBoard board = new GomokuBoard(size);
            for (int i = 0; i < size; i++)
            {
                string[] row = lines[2 + i].Split(',');
                for (int j = 0; j < size; j++)
                {
                    board.Grid[i, j] = row[j] == "_" ? null : row[j][0];
                }
            }

            Player player1 = new SymbolPlayer("Player 1", 'X');
            Player player2 = new SymbolPlayer("Player 2", 'O');
            Player currentPlayer = (turn == "player1") ? player1 : player2;

            Console.WriteLine("Gomoku game loaded successfully.");
            return (board, player1, player2, currentPlayer);
        }

                // --------------------- Notaktu GAME ---------------------


        public static void SaveNotaktoGame(bool[][,] boardStates, string currentTurn, bool isComputer, string path = "NotaktoSave.txt")
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(currentTurn);               // player1 or player2
                writer.WriteLine(isComputer.ToString());     // true or false

                for (int b = 0; b < boardStates.Length; b++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            writer.Write(boardStates[b][i, j] ? "1" : "0");
                            if (j < 2) writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }
            }

            Console.WriteLine("Notakto game saved.");
        }

        public static (NotaktoBoard[] boards, Player player1, Player player2, Player currentPlayer) LoadNotaktoGame(string path = "NotaktoSave.txt")
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Notakto save file not found.");
                Environment.Exit(0);
            }

            string[] lines = File.ReadAllLines(path);

            string turn = lines[0].Trim().ToLower();
            bool isComputer = bool.Parse(lines[1].Trim());

            NotaktoBoard[] boards = new NotaktoBoard[3];
            for (int b = 0; b < 3; b++)
            {
                boards[b] = new NotaktoBoard();
                for (int i = 0; i < 3; i++)
                {
                    string[] row = lines[2 + b * 3 + i].Split(',');
                    for (int j = 0; j < 3; j++)
                    {
                        boards[b].Grid[i, j] = row[j] == "1";
                    }
                }
            }

            Player player1 = new Player("Player 1", new List<int>());
            Player player2 = isComputer
                ? new NotaktoComputerPlayer("Computer", new List<int>())
                : new Player("Player 2", new List<int>());

            Player currentPlayer = (turn == "player1") ? player1 : player2;

            Console.WriteLine("Notakto game loaded successfully.");
            return (boards, player1, player2, currentPlayer);
        }


    }


    
}
