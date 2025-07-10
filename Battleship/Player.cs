namespace Battleship;

public class Player
{
    public string Name { get; }
    public Grid Grid { get; }

    public Player(string name)
    {
        Name = name;
        Grid = new Grid(10, 10);
    }
}
