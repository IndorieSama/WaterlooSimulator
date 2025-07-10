Feature: Gestion d'une partie de Bataille Navale
En tant que joueur,
Je veux pouvoir placer mes bateaux sur une grille
Et attaquer la grille adverse
Afin de couler tous les bateaux ennemis et gagner la partie.

    Background:
        Given une grille de 10 par 10
        And un joueur A et un joueur B

    Scenario: Placer un bateau horizontalement
        When le joueur A place un destroyer de longueur 3 de A1 à A3
        Then le bateau est placé correctement sur la grille

    Scenario: Tenter de placer un bateau qui chevauche un autre
        Given le joueur A a placé un croiseur de B1 à B4
        When le joueur A place un destroyer de B3 à B5
        Then un message d'erreur indique que les bateaux se chevauchent
        
    Scenario: Tenter de placer un bateau hors de la grille
        Given une grille de 10 par 10
        And un joueur A
        When le joueur A tente de placer un porte-avions de longueur 5 de J8 à J12
        Then un message d'erreur indique que le bateau dépasse la grille

    Scenario: Tirer et toucher un bateau
        Given le joueur B a un destroyer sur C5 à C7
        When le joueur A tire en C5
        Then le tir est un "Touché"

    Scenario: Tirer dans l'eau
        Given aucune case n'est occupée en D4
        When le joueur A tire en D4
        Then le tir est un "À l'eau"

    Scenario: Couler un bateau
        Given le joueur B a un sous-marin sur E2 à E2
        When le joueur A tire en E2
        Then le sous-marin est coulé
        And le joueur B a perdu un bateau
        
    Scenario: Le joueur rejoue après un tir réussi
        Given une partie en cours avec le joueur A et le joueur B
        And le joueur B a un destroyer sur F5 à F7
        When le joueur A tire en F5
        Then le tir est un "Touché"
        And le joueur A garde la main et peut tirer à nouveau

    Scenario: Le joueur passe son tour après un tir raté
        Given une partie en cours avec le joueur A et le joueur B
        And aucune case n'est occupée en G8
        When le joueur A tire en G8
        Then le tir est un "À l'eau"
        And le tour passe au joueur B


    Scenario: Gagner la partie
        Given le joueur B n’a plus de bateaux après le dernier tir
        When le joueur A tire et coule le dernier bateau
        Then le joueur A gagne la partie