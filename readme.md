# 🚢 Projet Tests SpecFlow – Bataille Navale

## 📌 Objectifs

Ce projet SpecFlow illustre la gestion de tests BDD pour une partie de bataille navale.

## 🗂️ Ce qui est inclus

- **Feature** : gestion complète de la partie (placement des bateaux, tirs, règles de victoire, erreurs).
- **Step Definitions (`BattleshipSteps.cs`)** : implémentation C# pour lier les steps Gherkin au moteur de jeu `BattleshipGame`.

## 🎯 Bonnes pratiques mises en œuvre

- **Identification des cas de test** : couverture des actions principales et des cas limites.
- **Priorisation des scénarios** : du plus simple (placement) au plus critique (gagner la partie).
- **Lisibilité** : langage clair, scénarios auto-documentés.
- **Données de test explicites** : coordonnées claires, bateaux nommés.
- **Extensibilité** : facile d’ajouter de nouveaux scénarios ou bateaux.
- **Langage ubiquitaire** : vocabulaire partagé joueur/métier/développeur.
- **Réutilisabilité** : steps génériques, calcul automatique des longueurs, pas de duplication.

## ⚙️ Structure recommandée

/BattleshipProject/
├─ Features/
│ └─ BatailleNavale.feature
├─ Steps/
│ └─ BattleshipSteps.cs
├─ BattleshipGame.cs (moteur de jeu)
└─ README.md


## 📝 Remarques MSBuild

Pour éviter l'erreur `[MSB4067] L'élément "#text" ... n'est pas reconnu` :  
- Vérifie bien que ton `Template.csproj` ne contient pas de texte résiduel en dehors des balises `<PropertyGroup>` ou `<ItemGroup>`.
- Les variables `$target$`, `$langVersion$`, etc. doivent être correctement interprétées par ton moteur de template.

## 🚀 Lancer les tests

1. Cloner le projet
2. Restaurer les packages NuGet
3. Exécuter les tests via `dotnet test` ou ton runner SpecFlow/IDE préféré.

---

**Auteur** : *Ton Nom ou ton équipe*