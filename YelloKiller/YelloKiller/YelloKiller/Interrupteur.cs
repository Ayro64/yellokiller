using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Interrupteur : Sprite
    {
        public Vector2 PortePosition { get; set; }
        Texture2D texturePorte;
        public bool PorteOuverte { get; private set; }

        public Interrupteur(Vector2 position)
            : base(position)
        {
            PorteOuverte = false;
            PortePosition = new Vector2(X, Y) + Vector2.One;
        }

        public void LoadContent(ContentManager content)
        {
            texturePorte = content.Load<Texture2D>(@"Menu Editeur de Maps\porte");
            base.LoadContent(content, @"Menu Editeur de Maps\interrupteur");
        }

        public void DrawInGame(SpriteBatch spriteBatch, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
            if(!PorteOuverte)
                spriteBatch.Draw(texturePorte, 28 * new Vector2(PortePosition.X, PortePosition.Y) - new Vector2(camera.X, camera.Y), Color.White);
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(Texture, 28 * new Vector2(X - camera.X + 2, Y - camera.Y), Color.White);
            spriteBatch.Draw(texturePorte, 28 * new Vector2(PortePosition.X - camera.X + 2, PortePosition.Y - camera.Y), Color.White);
        }

        public void OuvrirPorte()
        {
            if (!PorteOuverte)
            {
                PorteOuverte = true;
                Rotation = (float)Math.PI;
                position += 28 * Vector2.One;
            }
        }
    }
}
