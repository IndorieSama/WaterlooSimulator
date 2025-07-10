namespace Battleship;

using System.Collections.Generic;

public class Grid
{
    public int Width { get; }
    public int Height { get; }

    public List<Ship> Ships { get; } = new();
    public int RemainingShipsCount => Ships.FindAll(s => !s.IsSunk).Count;

    public bool WasLastShipSunk { get; private set; }

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool IsOverlap(Coordinate start, Coordinate end)
    {
        var newPositions = GetPositionsBetween(start, end);

        foreach (var ship in Ships)
        {
            foreach (var pos in ship.Positions)
            {
                if (newPositions.Exists(p => p.X == pos.X && p.Y == pos.Y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void PlaceShip(string shipType, int length, Coordinate start, Coordinate end)
    {
        var positions = GetPositionsBetween(start, end);
        Ships.Add(new Ship(shipType, length, positions));
    }

    public string FireAt(Coordinate coord)
    {
        foreach (var ship in Ships)
        {
            if (ship.Contains(coord))
            {
                ship.RegisterHit(coord);
                if (ship.IsSunk)
                {
                    WasLastShipSunk = true;
                    return "Coulé";
                }
                else
                {
                    WasLastShipSunk = false;
                    return "Touché";
                }
            }
        }

        WasLastShipSunk = false;
        return "À l'eau";
    }

    private List<Coordinate> GetPositionsBetween(Coordinate start, Coordinate end)
    {
        var positions = new List<Coordinate>();

        if (start.X == end.X) // Vertical
        {
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);
            for (int y = minY; y <= maxY; y++)
            {
                positions.Add(new Coordinate(start.X, y));
            }
        }
        else if (start.Y == end.Y) // Horizontal
        {
            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            for (int x = minX; x <= maxX; x++)
            {
                positions.Add(new Coordinate(x, start.Y));
            }
        }
        else
        {
            throw new InvalidOperationException("Placement diagonal interdit");
        }

        return positions;
    }
    
    public void SetRemainingShips(int count)
    {
        // Ex. si tu veux simuler "plus qu'un seul bateau vivant"
        int sunkCount = Ships.Count - count;

        int counter = 0;
        foreach (var ship in Ships)
        {
            if (counter < sunkCount)
            {
                // Marque ce bateau comme coulé
                ship.ForceSink();
                counter++;
            }
            else
            {
                // Les autres restent à flot
            }
        }
    }
}

