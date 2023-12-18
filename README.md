# Circular Maze Generator

Projet Github réalisé dans le cadre du cours de Mondes virtuels - Génération procédurale de Gilles Gesquiere du Master Gamagora.


<p align="center">
  <img src="./generated_maze.png" />
</p>

<p align="center">
  <img src="./circular-maze-animation.gif" />
</p>

## Comment compiler et tester le code

- Cloner le dépot git en local
- Ouvrir le dossier sur Unity
- Ouvrir la scène appelée "DemoScene"
- (optionnel) Jouer avec les paramètres de génération en cliquant sur le "GameManager"
- Lancer la scène

## Documentation

### L'algorithme utilisé

L'algorithme DFS (Depth-First Search, ou Parcours en Profondeur) est utilisé dans le générateur de labyrinthe circulaire pour créer le labyrinthe lui-même.

Dans le contexte de la génération de labyrinthes, le DFS commence à une cellule aléatoire et explore aussi loin que possible le long de chaque branche avant de revenir en arrière. Ce processus est répété jusqu'à ce que l'ensemble du labyrinthe ait été exploré. Le résultat est un labyrinthe parfait, c'est-à-dire qu'il existe un seul chemin entre deux cellules et qu'il n'y a pas de boucles.

L'algorithme DFS est un choix populaire pour la génération de labyrinthes en raison de sa simplicité et du fait qu'il génère des labyrinthes avec un facteur "rivière" élevé, ce qui signifie qu'ils ont de longs couloirs sinueux.

### Documentation du Projet Unity

Projet visant à créer un labyrinthe circulaire en utilisant Unity pour permettre de "gamifier" cette génération procédurale. Le labyrinthe est construit à partir de segments circulaires, formant des anneaux concentriques avec des chemins entre eux.

#### **Composants Principaux**

1. **GameManager**
    - **Responsabilité** : Contrôle la génération du labyrinthe et gère les différents composants.
    - **Fonctionnalités** :
        - Crée des anneaux concentriques.
        - Initialise et coordonne la génération des cellules du labyrinthe.
        - Crée des murs courbés et plats pour représenter les chemins du labyrinthe.
        - Gère le démarrage de la génération du labyrinthe.

2. **Maze**
    - **Responsabilité** : Implémente l'algorithme de backtracking pour créer les chemins du labyrinthe.
    - **Fonctionnalités** :
        - Génère les chemins à travers les cellules du labyrinthe.
        - Utilise un algorithme de backtracking pour connecter les cellules et former le labyrinthe.
        - Identifie aléatoirement une sortie dans l'anneau extérieur pour démarrer la génération.

3. **MazeCell**
    - **Responsabilité** : Représente une cellule individuelle du labyrinthe.
    - **Fonctionnalités** :
        - Gère les murs avant et gauche de chaque cellule.
        - Permet d'initialiser les cellules avec des paramètres de position.
        - Active ou désactive les murs avant et gauche de la cellule.
        - Chaque cellule contient un mur à gauche et un mur courbé.

4. **CurvedWall**
    - **Responsabilité** : Génère les murs courbés pour les segments du labyrinthe.
    - **Fonctionnalités** :
        - Crée des murs courbés en fonction des paramètres tels que le rayon, l'angle et la hauteur spécifiés.
        - Génère un maillage représentant les murs courbés.
        - Configure un MeshCollider pour permettre la détection de collision.

#### **Fonctionnement Général**

1. Le GameManager est chargé de créer les anneaux concentriques du labyrinthe.
2. Chaque anneau est composé de cellules représentées par des MazeCells.
3. Le Maze utilise un algorithme de backtracking pour créer des chemins à travers les cellules.
4. Les murs sont représentés par des CurvedWalls pour former la géométrie du labyrinthe.
5. Le projet utilise des maillages pour rendre les murs visibles et un MeshCollider pour détecter les collisions.
6. Chaque cellule contient un mur à gauche et un mur courbé.
7. Le nombre de cellules par anneau dépend du nombre de segments de l'anneau, chaque anneau étant segmenté en plusieurs parties (paramètres utilisateur).



## **Conclusion**

Ce projet de génération de labyrinthe circulaire utilise des composants pour créer un labyrinthe complexe à partir de segments circulaires. L'utilisation de l'algorithme de backtracking et de la génération de maillage permet de créer un labyrinthe unique et jouable dans l'environnement Unity, notamment pour notre jeu s'inspirant du film "Le Labyrinthe" de Wes Ball.

<p align="center">
  <img src="./labyrinthe.gif" />
</p>
