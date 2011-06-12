using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Interrupteur : Sprite
    {
        public Vector2 PortePosition { get; set; }
        Texture2D texturePorteFermee, texturePorteOuverte, textureInterrupteurTouche;
        public bool PorteOuverte { get; private set; }

        public Interrupteur(Vector2 position)
            : base(position)
        {
            PorteOuverte = false;
            PortePosition = new Vector2(X, Y) + Vector2.One;
        }

        public void LoadContent(ContentManager content)
        {
            texturePorteFermee = content.Load<Texture2D>(@"Menu Editeur de Maps\barriereFermee");
            texturePorteOuverte = content.Load<Texture2D>(@"Menu Editeur de Maps\barriereOuverte");
            textureInterrupteurTouche = content.Load<Texture2D>(@"Menu Editeur de Maps\interrupteurTouche");
            base.LoadContent(content, @"Menu Editeur de Maps\interrupteurPasTouche");
        }

        public void DrawInGame(SpriteBatch spriteBatch, Rectangle camera)
        {
            if (!PorteOuverte)
            {
                base.Draw(spriteBatch, camera);
                spriteBatch.Draw(texturePorteFermee, 28 * new Vector2(PortePosition.X, PortePosition.Y) - new Vector2(camera.X, camera.Y), Color.White);
            }
            else
            {
                spriteBatch.Draw(textureInterrupteurTouche, new Vector2(position.X - camera.X, position.Y - camera.Y), Color.White);
                spriteBatch.Draw(texturePorteOuverte, 28 * new Vector2(PortePosition.X, PortePosition.Y) - new Vector2(camera.X, camera.Y), Color.White);
            }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(Texture, 28 * new Vector2(X - camera.X + 2, Y - camera.Y), Color.White);
            spriteBatch.Draw(texturePorteFermee, 28 * new Vector2(PortePosition.X - camera.X + 2, PortePosition.Y - camera.Y), Color.White);
        }

        public void OuvrirPorte()
        {
            if (!PorteOuverte)
                PorteOuverte = true;
        }
    }
}
