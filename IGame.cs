namespace BoardGameFramework
{
    public interface IGame
    {
        int BoardSize { get; }
        Board GameBoard { get; }
        Player Player1 { get; }
        Player Player2 { get; }
        Player CurrentPlayer { get; set; }

        bool IsWin();
        bool IsTie();
        void DisplayMagicSum();
    }
}
