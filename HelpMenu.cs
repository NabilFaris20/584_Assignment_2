namespace BoardGameFramework{
    public static class HelpMenu
{
    public static void Show()
    {
        Console.WriteLine("----- HELP MENU -----");
        Console.WriteLine("Type a number from your available list to play.");
        Console.WriteLine("Type 'save' to save the game.");
        Console.WriteLine("Type 'undo' to undo the last move.");
        Console.WriteLine("Type 'redo' to redo the previous move.");
        Console.WriteLine("Type 'help' to view this help menu again.");
        Console.WriteLine("Win by forming a line that adds up to the magic sum.");
        Console.WriteLine("---------------------");
    }
}
}