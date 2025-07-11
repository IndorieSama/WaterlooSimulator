using System;
using Battleship;
using TechTalk.SpecFlow;
using Xunit;

[Binding]
public class BattleshipSteps
{
    private BattleshipGame _game;
    private string _lastShotResult;

    [Given(@"une grille de (.*) par (.*)")]
    public void GivenUneGrilleDePar(int width, int height)
    {
        _game = new BattleshipGame(width, height);
    }

    [Given(@"un joueur A et un joueur B")]
    public void GivenUnJoueurAEtUnJoueurB()
    {
        _game.AddPlayer("A");
        _game.AddPlayer("B");
    }

    [Given(@"une partie en cours avec le joueur A et le joueur B")]
    public void GivenUnePartieEnCoursAvecDeuxJoueurs()
    {
        _game = new BattleshipGame(10, 10);
        _game.AddPlayer("A");
        _game.AddPlayer("B");
    }

    [When(@"le joueur (.*) place un (.*) de longueur (.*) de (.*) à (.*)")]
    public void WhenLeJoueurPlaceUnBateau(string player, string shipType, int length, string start, string end)
    {
        _game.PlaceShip(player, shipType, length, start, end);
    }

    [Given(@"le joueur (.*) a placé un (.*) de (.*) à (.*)")]
    public void GivenLeJoueurAPlaceUnBateau(string player, string shipType, string start, string end)
    {
        int length = CalculateShipLength(start, end);
        _game.PlaceShip(player, shipType, length, start, end);
    }

    [When(@"le joueur (.*) tente de placer un (.*) de longueur (.*) de (.*) à (.*)")]
    public void WhenLeJoueurTenteDePlacerUnBateau(string player, string shipType, int length, string start, string end)
    {
        _game.PlaceShip(player, shipType, length, start, end);
    }

    [Then(@"le bateau est placé correctement sur la grille")]
    public void ThenLeBateauEstPlaceCorrectement()
    {
        Assert.True(_game.LastPlacementSuccess);
    }

    [Then(@"un message d'erreur indique que les bateaux se chevauchent")]
    public void ThenUnMessageErreurChevauchement()
    {
        Assert.Equal("Erreur: Chevauchement", _game.LastErrorMessage);
    }

    [Then(@"un message d'erreur indique que le bateau dépasse la grille")]
    public void ThenUnMessageErreurDepasseGrille()
    {
        Assert.Equal("Erreur: Hors grille", _game.LastErrorMessage);
    }

    [Given(@"le joueur (.*) a un (.*) sur (.*) à (.*)")]
    public void GivenLeJoueurAAUnBateau(string player, string shipType, string start, string end)
    {
        int length = CalculateShipLength(start, end);
        _game.PlaceShip(player, shipType, length, start, end);
    }

    [When(@"le joueur (.*) tire en (.*)")]
    public void WhenLeJoueurTireEn(string player, string coordinate)
    {
        _lastShotResult = _game.Fire(player, coordinate);
    }

    [Then(@"le tir est un ""(.*)""")]
    public void ThenLeTirEstUn(string expectedResult)
    {
        Assert.Equal(expectedResult, _lastShotResult);
    }

    [Given(@"aucune case n'est occupée en (.*)")]
    public void GivenAucuneCaseOccupeeEn(string coordinate)
    {
        // Rien à faire : aucune case = pas de bateau à cet endroit.
    }

    [Then(@"le sous-marin est coulé")]
    public void ThenLeSousMarinEstCoule()
    {
        Assert.True(_game.LastShipSunk);
    }

    [Then(@"le joueur B a perdu un bateau")]
    public void ThenLeJoueurBAPerduUnBateau()
    {
        Assert.Equal(_game.TotalShips - 1, _game.GetRemainingShips("B"));
    }

    [Then(@"le joueur (.*) garde la main et peut tirer à nouveau")]
    public void ThenLeJoueurGardeLaMain(string player)
    {
        Assert.Equal(player, _game.CurrentPlayer);
    }

    [Then(@"le tour passe au joueur (.*)")]
    public void ThenLeTourPasseAuJoueur(string player)
    {
        Assert.Equal(player, _game.CurrentPlayer);
    }

    [Given(@"le joueur B n’a plus de bateaux après le dernier tir")]
    public void GivenLeJoueurBNAPasDeBateaux()
    {
        _game.SetRemainingShips("B", 1);
    }

    [Then(@"le joueur A gagne la partie")]
    public void ThenLeJoueurAGagneLaPartie()
    {
        Assert.Equal("A", _game.Winner);
    }

    private int CalculateShipLength(string start, string end)
    {
        var startCoord = Coordinate.Parse(start);
        var endCoord = Coordinate.Parse(end);

        if (startCoord.X == endCoord.X)
            return Math.Abs(endCoord.Y - startCoord.Y) + 1;
        else if (startCoord.Y == endCoord.Y)
            return Math.Abs(endCoord.X - startCoord.X) + 1;
        else
            throw new InvalidOperationException("Placement diagonal interdit");
    }

    [Given(@"un joueur A")]
    public void GivenUnJoueurA()
    {
        if (_game == null)
        {
            _game = new BattleshipGame(10, 10);
        }
        _game.AddPlayer("A");
    }

    [When(@"le joueur A place un destroyer de B(.*) à B(.*)")]
    public void WhenLeJoueurAPlaceUnDestroyerDeB1AB5(int startY, int endY)
    {
        string start = $"B{startY}";
        string end = $"B{endY}";
        int length = Math.Abs(endY - startY) + 1;
        _game.PlaceShip("A", "destroyer", length, start, end);
    }

    [When(@"le joueur A tire et coule le dernier bateau")]
    public void WhenLeJoueurATireEtCouleLeDernierBateau()
    {
        // Tir sur une position qui coule le dernier bateau
        _lastShotResult = _game.Fire("A", "E2");
    }

    [Then(@"un message d'erreur indique ""(.*)""")]
    public void ThenUnMessageDerreurIndique(string expected)
    {
        Assert.Equal(expected, _game.LastErrorMessage);
    }

    [Then(@"c'est toujours au joueur A de jouer")]
    public void ThenCestToujoursAuJoueurADeJouer()
    {
        Assert.Equal("A", _game.CurrentPlayer);
    }

    [When(@"le joueur A tire de nouveau en D(.*)")]
    public void WhenLeJoueurATireDeNouveauEnD(int y)
    {
        _lastShotResult = _game.Fire("A", $"D{y}");
    }

    [Then(@"tous les bateaux sont placés correctement sur la grille")]
    public void ThenTousLesBateauxSontPlacesCorrectementSurLaGrille()
    {
        var player = _game.Players.First(p => p.Name == "A");
        Assert.Equal(_game.TotalShips, player.Grid.Ships.Count);
    }
}
