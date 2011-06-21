using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Garde : Ennemi
    {
        public Garde(Vector2 position, Carte carte)
            : base(position, carte)
        {
            Position = position;
            SourceRectangle = new Rectangle(24, 64, 16, 24);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 24);
            positionDesiree = position;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, 0, @"Feuilles de sprites\Garde");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Heros heros1, Heros heros2, Rectangle camera, List<EnnemiMort> ennemisMorts, Rectangle fumeeHeros1, Rectangle fumeeHeros2)
        {
            if (Collision(heros1.Rectangle, fumeeHeros1, fumeeHeros2))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heros1.Y, heros1.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }
            else if (heros2 != null && (Collision(heros2.Rectangle, fumeeHeros1, fumeeHeros2)))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heros2.Y, heros2.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }

            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24), heros1, heros2, ennemisMorts, fumeeHeros1, fumeeHeros2);            
        }
    }
}