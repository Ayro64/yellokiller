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

        public void Update(GameTime gameTime, Carte carte, Heros heross1, Heros heross2, Rectangle camera, List<EnnemiMort> ennemisMorts, Rectangle fumeeHeros1, Rectangle fumeeHeros2)
        {
            if (Collision(heross1.Rectangle, fumeeHeros1, fumeeHeros2))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heross1.Y, heross1.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }
            else if (heross2 != null && (Collision(heross2.Rectangle, fumeeHeros1, fumeeHeros2)))
            {
                Depart = carte.Cases[Y, X];
                Arrivee = carte.Cases[heross2.Y, heross2.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
            }

            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24), heross1, heross2, ennemisMorts, fumeeHeros1, fumeeHeros2);            
        }
    }
}