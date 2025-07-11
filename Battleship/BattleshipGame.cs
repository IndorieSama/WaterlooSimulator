﻿using System;
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

    private readonly Dictionary<string, int> _sunkCount = new();
    private readonly Dictionary<string, HashSet<Coordinate>> _shotsFired = new();

    public int TotalShips { get; private set; } = 1;

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
            CurrentPlayer = name;
        }
        _sunkCount[name]     = 0;
        _shotsFired[name]    = new HashSet<Coordinate>();
    }

    public void PlaceShip(string playerName, string shipType, int length, string start, string end)
    {
        var player     = GetPlayer(playerName);
        var startCoord = Coordinate.Parse(start);
        var endCoord   = Coordinate.Parse(end);

        if (!IsWithinBounds(startCoord) || !IsWithinBounds(endCoord))
        {
            LastPlacementSuccess = false;
            LastErrorMessage     = "Erreur: Hors grille";
            return;
        }

        if (player.Grid.IsOverlap(startCoord, endCoord))
        {
            LastPlacementSuccess = false;
            LastErrorMessage     = "Erreur: Chevauchement";
            return;
        }

        player.Grid.PlaceShip(shipType, length, startCoord, endCoord);
        LastPlacementSuccess = true;
        LastErrorMessage     = null;
    }

    public string Fire(string playerName, string coordinate)
    {
        LastErrorMessage = null;

        var coord = Coordinate.Parse(coordinate);
        if (!IsWithinBounds(coord))
        {
            LastErrorMessage = "Erreur: Hors grille";
            return null;
        }

        if (_shotsFired[playerName].Contains(coord))
        {
            LastErrorMessage = "Case déjà ciblée";
            return null;
        }
        _shotsFired[playerName].Add(coord);

        var opponent = GetOpponent(playerName);
        var result = opponent.Grid.FireAt(coord);
        var realSink = opponent.Grid.WasLastShipSunk;
        LastShipSunk = realSink;

        if (realSink)
        {
            _sunkCount[opponent.Name]++;
            SwitchTurn();
        }
        else if (result == "Touché")
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
        return TotalShips - _sunkCount[playerName];
    }

    public void SetRemainingShips(string playerName, int count)
    {
        
        GetPlayer(playerName).Grid.SetRemainingShips(count);
        
        _sunkCount[playerName] = TotalShips - count;
    }

    private void SwitchTurn()
    {
        if (Players.Count != 2) return;
        CurrentPlayer = GetOpponent(CurrentPlayer).Name;
    }

    private void CheckWinCondition(Player opponent)
    {
        if (GetRemainingShips(opponent.Name) <= 0)
        {
            Winner = CurrentPlayer;
        }
    }

    private Player GetPlayer(string name)
    {
        return Players.Find(p => p.Name == name)
               ?? throw new InvalidOperationException($"Joueur '{name}' introuvable");
    }

    public Player GetOpponent(string name)
    {
        return Players.Find(p => p.Name != name)
               ?? throw new InvalidOperationException("Impossible de trouver l'adversaire");
    }

    private bool IsWithinBounds(Coordinate coord)
    {
        return coord.X >= 1 && coord.X <= Width
            && coord.Y >= 1 && coord.Y <= Height;
    }
}