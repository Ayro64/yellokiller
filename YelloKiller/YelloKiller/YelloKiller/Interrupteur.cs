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
        Texture2D texturePorteFermee, texturePorteOuverte, textureInterrupteurTouche;
        public Vector2 PortePosition { get; set; }
        public bool PorteOuverte { get; set; }
        public byte rotation { get; set; }

        public Interrupteur(Vector2 position, Carte carte)
            : base(position)
        {
            PorteOuverte = false;
            PortePosition = new Vector2(position.X - (position.X + 1 >= 79 ? 1 : -1), position.Y);
            rotation = 0;
            carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = false;
            carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = false;
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
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = true;
                        break;
                    case 1:
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y + 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        break;
                    case 2:
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 2].EstFranchissable = true;
                        break;
                    case 3:
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 2, (int)PortePosition.X].EstFranchissable = true;
                        break;
                }
            }
        }

        public void Rotationner(Carte carte)
        {
            switch (this.rotation)
            {
                case 0:
                    if (PortePosition.Y + 1 < Taille_Map.HAUTEUR_MAP && PortePosition.X - 1 >= 0)
                    {
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = true;
                        rotation++;
                        rotation %= 4;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X - 1].EstFranchissable = false;
                        carte.Cases[(int)PortePosition.Y + 1, (int)PortePosition.X - 1].EstFranchissable = false;
                    }
                    break;
                case 1:
                    if (PortePosition.Y - 1 >= 0 && PortePosition.X - 2 >= 0)
                    {
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y + 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        rotation++;
                        rotation %= 4;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 1].EstFranchissable = false;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 2].EstFranchissable = false;
                    }
                    break;
                case 2:
                    if (PortePosition.Y - 2 >= 0 && PortePosition.X < Taille_Map.LARGEUR_MAP)
                    {
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 2].EstFranchissable = true;
                        rotation++;
                        rotation %= 4;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X].EstFranchissable = false;
                        carte.Cases[(int)PortePosition.Y - 2, (int)PortePosition.X].EstFranchissable = false;
                    }
                    break;
                case 3:
                    if (PortePosition.Y < Taille_Map.HAUTEUR_MAP && PortePosition.X + 1 < Taille_Map.LARGEUR_MAP)
                    {
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 2, (int)PortePosition.X].EstFranchissable = true;
                        rotation++;
                        rotation %= 4;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = false;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = false;
                    }
                    break;
            }
        }

        public void SauvegarderCheckPoint(ref StreamWriter file)
        {
            file.WriteLine(position.X + "," + position.Y + "," + PortePosition.X + "," + PortePosition.Y + "," + rotation + "," + PorteOuverte);
        }

        public void MettreLaRotation(Carte carte, byte nouvelleRotation, Vector2 nouvellePosition)
        {
            carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].Type > 0;
            carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].Type > 0;
            PortePosition = nouvellePosition;
            this.rotation = nouvelleRotation;

            switch (rotation)
            {
                case 0:
                    carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = false;
                    carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = false;
                    break;
                case 1:
                    carte.Cases[(int)PortePosition.Y, (int)PortePosition.X - 1].EstFranchissable = false;
                    carte.Cases[(int)PortePosition.Y + 1, (int)PortePosition.X - 1].EstFranchissable = false;
                    break;
                case 2:
                    carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 1].EstFranchissable = false;
                    carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 2].EstFranchissable = false;
                    break;
                case 3:
                    carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X].EstFranchissable = false;
                    carte.Cases[(int)PortePosition.Y - 2, (int)PortePosition.X].EstFranchissable = false;
                    break;
            }
        }

        public void ChangerPosition(Carte carte, Vector2 nouvelleposition)
        {
            switch (rotation)
            {
                case 0:
                    if (nouvelleposition.X + 1 < Taille_Map.LARGEUR_MAP)
                    {
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X + 1].EstFranchissable = true;
                        carte.Cases[(int)nouvelleposition.Y, (int)nouvelleposition.X].EstFranchissable = false;
                        carte.Cases[(int)nouvelleposition.Y, (int)nouvelleposition.X + 1].EstFranchissable = false;
                        PortePosition = nouvelleposition;
                    }
                    break;
                case 1:
                    if (nouvelleposition.X - 1 >= 0 && nouvelleposition.Y + 1 < Taille_Map.HAUTEUR_MAP)
                    {
                        carte.Cases[(int)PortePosition.Y, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y + 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)nouvelleposition.Y, (int)nouvelleposition.X - 1].EstFranchissable = false;
                        carte.Cases[(int)nouvelleposition.Y + 1, (int)nouvelleposition.X - 1].EstFranchissable = false;
                        PortePosition = nouvelleposition;
                    }
                    break;
                case 2:
                    if (nouvelleposition.X - 2 >= 0 && nouvelleposition.Y - 1 >= 0)
                    {
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 1].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X - 2].EstFranchissable = true;
                        carte.Cases[(int)nouvelleposition.Y - 1, (int)nouvelleposition.X - 1].EstFranchissable = false;
                        carte.Cases[(int)nouvelleposition.Y - 1, (int)nouvelleposition.X - 2].EstFranchissable = false;
                        PortePosition = nouvelleposition;
                    }
                    break;
                case 3:
                    if (nouvelleposition.Y - 2 >= 0)
                    {
                        carte.Cases[(int)PortePosition.Y - 1, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)PortePosition.Y - 2, (int)PortePosition.X].EstFranchissable = true;
                        carte.Cases[(int)nouvelleposition.Y - 1, (int)nouvelleposition.X].EstFranchissable = false;
                        carte.Cases[(int)nouvelleposition.Y - 2, (int)nouvelleposition.X].EstFranchissable = false;
                        PortePosition = nouvelleposition;
                    }
                    break;
            }
        }
    }
}