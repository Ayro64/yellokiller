using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace YelloKiller
{
    class Moteur_Physique
    {
        static public void updateObjet(List<Sprite> sprites)
        {
            int i, x1, x2, y1, y2;
            float oldx, oldy;
            bool res;

            foreach(Sprite sprite1 in sprites)
            {
                oldx = sprite1.Position.X;
                oldy = sprite1.Position.Y;
                sprite1.Update();

                foreach (Sprite sprite2 in sprites)
                {
                    res = collisionObjets(sprite1, sprite2);
                    if (res)
                        sprite1.Position = new Vector2(oldx, oldy);
                }
            }
            

        }

        static private bool collisionObjets(Sprite objet1, Sprite objet2)
        {
            if (objet1.Position.X + objet1.Texture.Width < objet2.Position.X)
                return false;
            if (objet2.Position.X + objet2.Texture.Width < objet1.Position.X)
                return false;
            if (objet1.Position.Y + objet1.Texture.Height < objet2.Position.Y)
                return false;
            if (objet2.Position.Y + objet2.Texture.Height < objet1.Position.Y)
                return false;

            return true;
        }
    }
}
