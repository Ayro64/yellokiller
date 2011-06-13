using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Interrupteur : Sprite
    {
        Vector2 portePosition;
        Texture2D texturePorteFermee, texturePorteOuverte, textureInterrupteurTouche;
        public bool PorteOuverte { get; private set; }
        public byte rotation;

        public Interrupteur(Vector2 position)
            : base(position)
        {
            PorteOuverte = false;
            PortePosition = new Vector2(position.X, position.Y) + Vector2.One;
            rotation = 0;
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
                spriteBatch.Draw(Texture, 28 * new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);
                spriteBatch.Draw(texturePorteFermee, 28 * new Vector2(PortePosition.X, PortePosition.Y) - new Vector2(camera.X, camera.Y), null, Color.White, rotation * ((float)Math.PI / 2), Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(textureInterrupteurTouche, 28 * new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);
                spriteBatch.Draw(texturePorteOuverte, 28 * new Vector2(PortePosition.X, PortePosition.Y) - new Vector2(camera.X, camera.Y), null, Color.White, rotation * ((float)Math.PI / 2), Vector2.Zero, 1, SpriteEffects.None, 1);
            }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(Texture, 28 * new Vector2(position.X - camera.X + 2, position.Y - camera.Y), Color.White);
            spriteBatch.Draw(texturePorteFermee, 28 * new Vector2(PortePosition.X - camera.X + 2, PortePosition.Y - camera.Y), null, Color.White, rotation * (float)Math.PI / 2, Origin, 1, SpriteEffects.None, 1);
        }

        public void OuvrirPorte()
        {
            if (!PorteOuverte)
                PorteOuverte = true;
        }

        public void Rotationner()
        {
            rotation++;
            rotation %= 4;
            /*if (rotation == 1)
            {
                portePosition.X++;
            }
            else if (rotation == 2)
            {
                portePosition.Y++;
                portePosition.X++;
            }
            else if (rotation == 3)
            {
                portePosition.X -= 2;
                portePosition.Y++;
            }
            else
                portePosition.Y -= 2;*/
        }

        public Vector2 PortePosition
        {
            get { return portePosition; }
            set { portePosition = value; }
        }

        public void ChangerPosition(Vector2 nouvellePosition)
        {
            portePosition = new Vector2(nouvellePosition.X, nouvellePosition.Y);
            /*if (rotation == 0)
                portePosition = new Vector2(nouvellePosition.X, nouvellePosition.Y);
            else if (rotation == 1)
                portePosition = new Vector2(nouvellePosition.X + 1, nouvellePosition.Y);
            else if (rotation == 2)
                portePosition = new Vector2(nouvellePosition.X + 2, nouvellePosition.Y + 1);
            else if (rotation == 3)
                portePosition = new Vector2(nouvellePosition.X, nouvellePosition.Y + 2);*/
        }
    }
}