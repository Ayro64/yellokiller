using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        Vector2 Origine;

        public Boss(Vector2 position, Carte carte)
            : base(position, carte)
        {
            Position = position;
            SourceRectangle = new Rectangle(26, 64, 18, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 18, 26);
            MaxIndex = 0;
            positionDesiree = position;
            Vie = 5;
            Origine = new Vector2(X, Y);
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, 0, @"Feuilles de sprites\Boss");
            MaxIndex = maxIndex;
        }

        public int Vie { get; set; }

        public void Update(GameTime gameTime, List<Shuriken> shuriken, Carte carte, Heros heross1, Heros heross2, Rectangle camera, List<EnnemiMort> morts, Rectangle fumeeHeros1, Rectangle fumeeHeros2)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24), heross1, heross2, morts, fumeeHeros1, fumeeHeros2);

            if (!RetourneCheminNormal && Collision(heross1.Rectangle, fumeeHeros1, fumeeHeros2))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heross1.Y, heross1.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }
            else if (!RetourneCheminNormal && heross2 != null && (Collision(heross2.Rectangle, fumeeHeros1, fumeeHeros2)))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heross2.Y, heross2.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }

            if (Math.Abs(Origine.X - X) > 4 || Math.Abs(Origine.Y - Y) > 4)
            {
                RetourneCheminNormal = true;
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[(int)Origine.Y, (int)Origine.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }
            else
                RetourneCheminNormal = false;


            IA.Esquive_Shuriken.Boss_Esquive_Shuriken(heross1, this, shuriken, carte, camera);

            if (Vie < 5 && Chemin.Count == 0)
            {
                if (heross1.Regarde_Bas && position.Y > heross1.position.Y)
                    SourceRectangle = new Rectangle(26, 0, 16, 24);
                else if (heross1.Regarde_Gauche && position.X < heross1.position.X)
                    SourceRectangle = new Rectangle(26, 33, 16, 24);
               else if (heross1.Regarde_Haut && position.Y < heross1.position.Y)
                    SourceRectangle = new Rectangle(26, 64, 16, 24);
                else if (heross1.Regarde_Droite && position.X > heross1.position.X)
                    SourceRectangle = new Rectangle(26, 97, 16, 24);
            }
        }
    }
}