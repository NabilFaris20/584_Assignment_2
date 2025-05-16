namespace BoardGameFramework{
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
}