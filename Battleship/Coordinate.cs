namespace Battleship;

public class Coordinate
{
    public int X { get; }
    public int Y { get; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Coordinate Parse(string coord)
    {
        char letter = coord[0];
        int x = letter - 'A' + 1;
        int y = int.Parse(coord.Substring(1));
        return new Coordinate(x, y);
    }

    public override bool Equals(object obj)
    {
        if (obj is Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}

