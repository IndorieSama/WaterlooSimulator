using System;
using System.Collections.Generic;
using Battleship;

public class BattleshipGame
{
    public int Width { get; }
    public int Height { get; }

    public List<Player> Players { get; } = new();
    public string CurrentPlayer { get; private set; }

    public bool LastPlacementSuccess { get; private set; }
    public string LastErrorMessage { get; private set; }
    public string Winner { get; private set; }
    public bool LastShipSunk { get; private set; }

    public int TotalShips { get; private set; } = 5; // Exemple par défaut

    public BattleshipGame(int width, int height)
    {
        Width = width;  
        Height = height;
    }

    public void AddPlayer(string name)
    {
        Players.Add(new Player(name));
        if (Players.Count == 1)
        {
            CurrentPlayer = name; // Premier joueur commence
        }
    }

    public void PlaceShip(string playerName, string shipType, int length, string start, string end)
    {
        var player = GetPlayer(playerName);
        var startCoord = Coordinate.Parse(start);
        var endCoord = Coordinate.Parse(end);
        
        if (!IsWithinBounds(endCoord))
        {
            LastPlacementSuccess = false;
            LastErrorMessage = "Erreur: Hors grille";
            return;
        }

        if (player.Grid.IsOverlap(startCoord, endCoord))
        {
            LastPlacementSuccess = false;
            LastErrorMessage = "Erreur: Chevauchement";
            return;
        }

        player.Grid.PlaceShip(shipType, length, startCoord, endCoord);
        LastPlacementSuccess = true;
        LastErrorMessage = null;
    }

    public string Fire(string playerName, string coordinate)
    {
        var opponent = GetOpponent(playerName);
        var coord = Coordinate.Parse(coordinate);

        var result = opponent.Grid.FireAt(coord);

        LastShipSunk = opponent.Grid.WasLastShipSunk;

        if (result == "Touché" || result == "Coulé")
        {
        }
        else
        {
            SwitchTurn();
        }

        CheckWinCondition(opponent);

        return result;
    }

    public int GetRemainingShips(string playerName)
    {
        return GetPlayer(playerName).Grid.RemainingShipsCount;
    }

    private void SwitchTurn()
    {
        if (Players.Count != 2) return;

        var other = GetOpponent(CurrentPlayer);
        CurrentPlayer = other.Name;
    }

    private void CheckWinCondition(Player opponent)
    {
        if (opponent.Grid.RemainingShipsCount <= 0)
        {
            Winner = CurrentPlayer;
        }
    }
    
    public void SetRemainingShips(string playerName, int count)
    {
        var player = GetPlayer(playerName);
        player.Grid.SetRemainingShips(count);
    }


    private Player GetPlayer(string name)
    {
        return Players.Find(p => p.Name == name);
    }

    private Player GetOpponent(string name)
    {
        return Players.Find(p => p.Name != name);
    }

    private bool IsWithinBounds(Coordinate coord)
    {
        return coord.X >= 1 && coord.X <= Width && coord.Y >= 1 && coord.Y <= Height;
    }
}
