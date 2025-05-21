namespace BoardGameFramework{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, List<int> availableNumbers)
            : base(name, availableNumbers) { }
    }

    public class SymbolPlayer : Player
        {
            public char Symbol { get; }

            public SymbolPlayer(string name, char symbol)
                : base(name, new List<int>()) // No numbers needed
            {
                Symbol = symbol;
            }
        }
}