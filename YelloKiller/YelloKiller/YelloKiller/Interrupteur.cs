using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Interrupteur : Sprite
    {
        Vector2 portePosition;
        Texture2D texturePorteFermee, texturePorteOuverte, textureInterrupteurTouche;
        public bool PorteOuverte { get; set; }
        public byte rotation { get; set; }

        public Interrupteur(Vector2 position, Carte carte)
            : base(position)
        {
            PorteOuverte = false;
            PortePosition = new Vector2(position.X, position.Y) + Vector2.One;
            rotation = 0;
            carte.Cases[(int)portePosition.Y, (int)portePosition.X].EstFranchissable = false;
            carte.Cases[(int)portePosition.Y, (int)portePosition.X + 1].EstFranchissable = false;
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
            spriteBatch.Draw(Texture, 28 * new Vector2(position.X - camera.X + 1, position.Y - camera.Y), Color.White);
            spriteBatch.Draw(texturePorteFermee, 28 * new Vector2(PortePosition.X - camera.X + 1, PortePosition.Y - camera.Y), null, Color.White, rotation * (float)Math.PI / 2, Origin, 1, SpriteEffects.None, 1);
        }

        public void OuvrirPorte(SoundBank soundBank, Carte carte)
        {
            if (!PorteOuverte)
            {
                soundBank.PlayCue("porte");
                PorteOuverte = true;

                switch (rotation)
                {
                    case 0:
                        carte.Cases[(int)portePosition.Y, (int)portePosition.X].EstFranchissable = true;
                        carte.Cases[(int)portePosition.Y, (int)portePosition.X + 1].EstFranchissable = true;
                        break;
                    case 1:
                        carte.Cases[(int)portePosition.Y, (int)portePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)portePosition.Y + 1, (int)portePosition.X - 1].EstFranchissable = true;
                        break;
                    case 2:
                        carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X - 2].EstFranchissable = true;
                        break;
                    case 3:
                        carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X].EstFranchissable = true;
                        carte.Cases[(int)portePosition.Y - 2, (int)portePosition.X].EstFranchissable = true;
                        break;
                }
            }
        }

        public void Rotationner()
        {
            rotation++;
            rotation %= 4;
        }

        public Vector2 PortePosition
        {
            get { return portePosition; }
            set { portePosition = value; }
        }

        public void SauvegarderCheckPoint(ref StreamWriter file)
        {
            file.WriteLine(position.X + "," + position.Y + "," + portePosition.X + "," + portePosition.Y + "," + rotation + "," + PorteOuverte);
        }

        public void MettreLaRotation(Carte carte, byte rotation)
        {
            this.rotation = rotation;
     
            switch(rotation)
            {
                case 0:
                    carte.Cases[(int)portePosition.Y, (int)portePosition.X].EstFranchissable = false;
                    carte.Cases[(int)portePosition.Y, (int)portePosition.X + 1].EstFranchissable = false;
                    break;
                case 1:
                    carte.Cases[(int)portePosition.Y, (int)portePosition.X - 1].EstFranchissable = false;
                    carte.Cases[(int)portePosition.Y + 1, (int)portePosition.X - 1].EstFranchissable = false;
                    break;
                case 2:
                    carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X - 1].EstFranchissable = false;
                    carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X - 2].EstFranchissable = false;
                    break;
                case 3:
                    carte.Cases[(int)portePosition.Y - 1, (int)portePosition.X].EstFranchissable = false;
                    carte.Cases[(int)portePosition.Y - 2, (int)portePosition.X].EstFranchissable = false;
                    break;
            }
        }
    }
}