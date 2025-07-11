namespace Battleship;

using System.Collections.Generic;
using System.Linq;

public class Grid
{
    public int Width { get; }
    public int Height { get; }

    public List<Ship> Ships { get; } = new();
    public int RemainingShipsCount => Ships.FindAll(s => !s.IsSunk).Count;
    public List<Coordinate> MissedShots { get; } = new();


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
    public void Display()
    {
        Console.WriteLine("\n   A B C D E F G H I J");
        for (int y = 0; y < Height; y++)
        {
            Console.Write($"{y + 1,2} ");
            for (int x = 0; x < Width; x++)
            {
                var coord = new Coordinate(x, y);
                string symbol = "."; // par défaut : vide

                bool isShip = false;
                bool isHit = false;

                foreach (var ship in Ships)
                {
                    if (ship.Positions.Any(p => p.X == x && p.Y == y))
                    {
                        isShip = true;

                        if (ship.Hits.Any(h => h.X == x && h.Y == y))
                        {
                            isHit = true;
                        }
                    }
                }

                if (isShip && isHit) symbol = "X";   // touché
                else if (MissedShots.Any(m => m.X == x && m.Y == y)) symbol = "~"; // raté

                Console.Write($"{symbol} ");
            }
            Console.WriteLine();
        }
    }


}

