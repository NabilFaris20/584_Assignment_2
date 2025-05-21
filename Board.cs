namespace BoardGameFramework
{
    public abstract class Board
    {
        public int Size { get; protected set; }

        public Board(int size)
        {
            Size = size;
        }

        public static int BoardSize()
        {
            int size;
            string? input;

            Console.WriteLine("Enter board size: ");
            input = Console.ReadLine();

            while (!int.TryParse(input, out size) || size <= 0)
            {
                Console.Write("Invalid input. Please enter a valid number: ");
                input = Console.ReadLine();
            }

            return size;
        }

        public abstract void Display();
    }

    public class NumericalBoard : Board
    {
        public int?[,] Grid { get; private set; }

        public NumericalBoard(int size) : base(size)
        {
            Grid = new int?[size, size];
        }

        public override void Display()
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Console.Write(Grid[i, j]?.ToString() ?? "_");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }

    public class GomokuBoard : Board
    {
        public char?[,] Grid { get; private set; }

        public GomokuBoard(int size) : base(3)
        {
            Grid = new char?[size, size];
        }

        public override void Display()
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Console.Write(Grid[i, j]?.ToString() ?? "_");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }

    public class NotaktoBoard : Board
    {
        public bool[,] Grid { get; private set; }

        public NotaktoBoard() : base(3)
        {
            Grid = new bool[3, 3];
        }

        public override void Display()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(Grid[row, col] ? "X" : "_");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
