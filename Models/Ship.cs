namespace BattleshipBackend.Models;

public class Ship
{
    public Position Start { get; set; }
}

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
}

public enum Rotation
{
    Horizontal = 1,     
    Vertical
}