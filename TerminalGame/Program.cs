namespace TerminalGame;

using System;
using Battleship;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Bataille Navale ===");

        var game = new BattleshipGame(10, 10);

        // Création des joueurs
        Console.Write("Nom du joueur 1 : ");
        string player1 = Console.ReadLine();
        game.AddPlayer(player1);

        Console.Write("Nom du joueur 2 : ");
        string player2 = Console.ReadLine();
        game.AddPlayer(player2);

        // Placement des bateaux pour chaque joueur
        foreach (var player in game.Players)
        {
            Console.WriteLine($"\nPlacement des bateaux pour {player.Name}");

            for (int i = 0; i < game.TotalShips; i++)
            {
                Console.WriteLine($"\nBateau #{i + 1} (ex: Destroyer)");

                Console.Write("Type de bateau : ");
                string type = Console.ReadLine();

                Console.Write("Longueur du bateau : ");
                int length = int.Parse(Console.ReadLine());

                Console.Write("Coordonnée de début (ex: A1) : ");
                string start = Console.ReadLine();

                Console.Write("Coordonnée de fin (ex: A5) : ");
                string end = Console.ReadLine();

                game.PlaceShip(player.Name, type, length, start, end);

                if (!game.LastPlacementSuccess)
                {
                    Console.WriteLine($"Erreur : {game.LastErrorMessage}");
                    i--; // Refaire le placement de ce bateau
                }
                else
                {
                    Console.WriteLine("Bateau placé !");
                }
            }
        }

        // Boucle de jeu principale
        while (string.IsNullOrEmpty(game.Winner))
        {
            Console.WriteLine($"\nC'est au tour de {game.CurrentPlayer}");
            Console.Write("Coordonnée à tirer (ex: B2) : ");
            string coord = Console.ReadLine();

            string result = game.Fire(game.CurrentPlayer, coord);

            if (game.LastErrorMessage != null)
            {
                Console.WriteLine($"Erreur : {game.LastErrorMessage}");
            }
            else
            {
                Console.WriteLine($"Résultat : {result}");
                Console.WriteLine($"Bateaux restants de l'adversaire : {game.GetRemainingShips(game.GetOpponent(game.CurrentPlayer).Name)}");
                var opponent = game.GetOpponent(game.CurrentPlayer);
                Console.WriteLine($"\nGrille de {opponent.Name} :");
                opponent.Grid.Display();
            }
        }

        Console.WriteLine($"\n🏆 Le gagnant est : {game.Winner} !");
        Console.WriteLine("Fin de la partie. Merci d'avoir joué !");
    }
}
