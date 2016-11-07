using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
Ceci est la simulation du siège d'un château sur une carte rectangulaire. Les ennemis cherchent à atteindre le château situé au dessus de la carte pendant que les tours cherchent à les arrêter.

Chaque tour de jeu se décompose en trois étapes :

1. Chaque tour vise un unique ennemi dans une zone carré de 5x5 centrée sur la tour.

Les tours visent en priorité les ennemis selon les critères suivants :
1. L'ennemi le plus avancé (le plus au nord)
2. L'ennemi le plus proche de la tour
3. L'ennemi le plus à l'est

2. Les points de vie (HP) de chaque ennemi sont réduits du nombre de tours le visant. Ensuite, chaque ennemi qui a 0 point de vie ou moins est DETRUIT. Enfin, si aucun ennemi ne reste en jeu, alors le joueur GAGNE.

3. Chaque ennemi se déplace vers le NORD d'une case. Si un ennemi se déplace sur une tour, l'ennemi est DETRUIT. Si un ennemi se déplace en dehors de la carte, le joueur PERD.

Vous devez afficher WIN/LOSE suivi par le numéro du tour où le jeu se termine.
**/

class CastleSiege
{
    class Coord
    {
        public int X;
        public int Y;
    }

    class Enemy : Coord
    {
        public int HP;
    }
    static void Main(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        var W = int.Parse(inputs[0]);
        var H = int.Parse(inputs[1]);
        var towers = new List<Coord>();
        var enemies = new List<Enemy>();
        for (var i = 0; i < H; i++)
        {
            var line = Console.ReadLine();
            for (var j = 0; j < W; j++)
            {
                if (line[j] == '.')
                {
                    continue;
                }
                if (line[j] == 'T')
                {
                    towers.Add(new Coord {X=j, Y=i});
                }
                else
                {
                    enemies.Add(new Enemy {HP=line[j]-'0', X=j, Y=i});
                }
            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("answer");
    }
}