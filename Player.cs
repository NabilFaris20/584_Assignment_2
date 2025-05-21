namespace BoardGameFramework{
    public class Player{

    public string Name {get;}
    public List<int> AvailableNumbers {get; private set;}

    public Player(string name, List<int> availableNumbers){

        Name = name;
        AvailableNumbers = availableNumbers;

    }

    public void RemoveNumber(int number){
        AvailableNumbers.Remove(number);
    }

    // public void ShowNumbers()
    // {
    //    Console.WriteLine($"{Name}'s available numbers: {string.Join(", ", AvailableNumbers)}");
    // }
    

}
}