namespace Battleship;

using System.Collections.Generic;

public class Ship
{
    public string Type { get; }
    public int Length { get; }
    public List<Coordinate> Positions { get; }
    public HashSet<Coordinate> Hits { get; }

    public Ship(string type, int length, List<Coordinate> positions)
    {
        Type = type;
        Length = length;
        Positions = positions;
        Hits = new HashSet<Coordinate>();
    }

    public bool IsSunk => Hits.Count >= Length;

    public bool Contains(Coordinate coord)
    {
        return Positions.Exists(p => p.X == coord.X && p.Y == coord.Y);
    }

    public void RegisterHit(Coordinate coord)
    {
        if (Contains(coord))
        {
            Hits.Add(coord);
        }
    }

    public void ForceSink()
    {
        Hits.Clear();
        foreach (var pos in Positions)
        {
            Hits.Add(pos);
        }
    }
}

