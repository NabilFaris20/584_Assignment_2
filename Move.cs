namespace BoardGameFramework{
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

}