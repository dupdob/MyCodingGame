using System;
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
    static void MainCastle(string[] args)
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
        //
        int turn=1;
        bool partyended = false;
        while (enemies.Count > 0)
        {
            // identify targets
            var arrows = new List<int>();
            foreach (var tower in towers)
            {
                var target = FindNearest(tower, enemies);
                if (target >= 0)
                {
                    arrows.Add(target);
                }
            }
            // fire arrows
            foreach (var arrow in arrows)
            {
                Console.Error.WriteLine("Shoot on {0}:{1}", enemies[arrow].X, enemies[arrow].Y);
                enemies[arrow].HP--;
            }
            // identify deads
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].HP <= 0)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            if (enemies.Count == 0)
            {
                // winning
                break;
            }
            // enmies move
            for (var index = 0; index < enemies.Count; index++)
            {
                var enemy = enemies[index];
                enemy.Y--;
                if (enemy.Y < 0)
                {
                    partyended = true;
                    break;
                }
                foreach (var tower in towers)
                {
                    if (tower.X == enemy.X && tower.Y == enemy.Y)
                    {
                        // enemy distroyed
                        enemies.RemoveAt(index--);
                    }
                }
            }
            if (partyended)
            {
                break;
            }
            turn++;
        }
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("{0} {1}", enemies.Count==0 ? "WIN" : "LOSE", turn);
    }

    static int FindNearest(Coord tower, IList<Enemy> enemies)
    {
        var ids = new List<int>();
        int northernLine = int.MaxValue;
        // find norther enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (Math.Abs(tower.X - enemy.X) > 2 || Math.Abs(tower.Y - enemy.Y) > 2 || enemy.Y > northernLine)
            {
                continue;
            }
            else
            {
                if (enemy.Y < northernLine)
                {
                    // this enemye is further north than others
                    northernLine = enemy.Y;
                    ids.Clear();
                }
                ids.Add(i);
            }
        }
        if (ids.Count > 1)
        {
            var list = new List<int>(ids);
            int mindist = int.MaxValue;
            ids.Clear();
            // we need to look for closest enemy
            foreach (var id in list)
            {
                var enemy = enemies[id];
                var dist = (tower.X - enemy.X) * (tower.X - enemy.X) + (tower.Y - enemy.Y) * (tower.Y - enemy.Y);
                if (dist > mindist)
                {
                    continue;
                }
                else
                {
                    if (dist < mindist)
                    {
                        ids.Clear();
                        mindist = dist;
                    }
                    ids.Add(id);
                }
            }
        }
        if (ids.Count > 1)
        {
            var list = new List<int>(ids);
            int maxEast = int.MinValue;
            ids.Clear();
            // we need to look for enemy further east
            foreach (var id in list)
            {
                var enemy = enemies[id];
                if (enemy.X < maxEast)
                {
                    continue;
                }
                else
                {
                    if (enemy.X > maxEast)
                    {
                        ids.Clear();
                        maxEast = enemy.X;
                    }
                    ids.Add(id);
                }
            }
            // there should be only one id here
        }
        return ids.Count == 0 ? -1 : ids[0];
    }
}