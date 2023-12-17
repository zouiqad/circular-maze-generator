# Circular Maze Generator

Projet Github réalisé dans le cadre du cours de Mondes virtuels - Génération procédurale de Gilles Gesquiere du Master Gamagora.


## Comment compiler et tester le code

- Cloner le dépot git en local
- Ouvrir le dossier sur Unity
- Ouvrir la scène appelée "DemoScene"
- (optionnel) Jouer avec les paramètres de génération en cliquant sur le "GameManager"
- Lancer la scène

## Documentation

### L'algorithme 

L'algorithme DFS (Depth-First Search, ou Parcours en Profondeur) est utilisé dans le générateur de labyrinthe circulaire pour créer le labyrinthe lui-même.

Dans le contexte de la génération de labyrinthes, le DFS commence à une cellule aléatoire et explore aussi loin que possible le long de chaque branche avant de revenir en arrière. Ce processus est répété jusqu'à ce que l'ensemble du labyrinthe ait été exploré. Le résultat est un labyrinthe parfait, c'est-à-dire qu'il existe un seul chemin entre deux cellules et qu'il n'y a pas de boucles.

L'algorithme DFS est un choix populaire pour la génération de labyrinthes en raison de sa simplicité et du fait qu'il génère des labyrinthes avec un facteur "rivière" élevé, ce qui signifie qu'ils ont de longs couloirs sinueux.


### Fonctionnement dans Unity

1. La génération du labyrinthe commence avec la classe `GameManager` qui est attachée à un objet dans la scène Unity. Cette classe est le contrôleur principal pour la génération du labyrinthe.

2. Dans `GameManager` qui représente le labyrinthe. Chaque liste interne de `MazeCell` représente un "anneau" du labyrinthe, et chaque `MazeCell` représente une cellule dans cet anneau.

3. Les attributs `curvedWallHeight` contrôlent divers aspects du labyrinthe, comme la hauteur des murs courbés, l'épaisseur des murs, le rayon du premier anneau, le nombre de segments dans un anneau, le nombre d'anneaux, le modèle de mur gauche, le matériau à utiliser pour les murs, et la distance seuil entre les segments.

4. La méthode `CreateCurvedWall` est utilisée pour créer un mur courbé dans le labyrinthe. Elle utilise les attributs mentionnés ci-dessus pour déterminer les caractéristiques du mur.

5. La classe `CurvedWall` représente un mur courbé dans le labyrinthe. Elle a des méthodes et des attributs qui lui permettent de se comporter comme un mur dans le labyrinthe.

6. La génération du labyrinthe commence dans la méthode `Start`. Cette méthode est appelée par Unity avant la première mise à jour du frame.

7. Une fois que toutes les cellules ont été créées, le labyrinthe est  finalisé en supprimant certains murs pour créer un chemin à travers le labyrinthe.

8. Enfin, le labyrinthe est stocké dans le `GameObject` `MazeGO` pour être utilisé plus tard dans le jeu.

LABYRINTHE EST COMPOSEE D'ANNEAU CHAQUE ANNEAU EST COMPOSEE DE CELULES , LES CELULES ADJASTANCES SONT VOISINES ENTRE ELLES, ET CHAQUE CELULE CONTIENT UN MUR A GAUCHE ET UN MUR COURBEE, LE NOMBRE DE CELULLE PAR ANNEAU DEPENDS DU NOMBRE DE SEGMENTS DE LANNEAU (CHAQUE ANNEAU EST SEGMENTE EN PLUSIEURS PARTIES (PARAMETRES UTILISATEUR)) 

