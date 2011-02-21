using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Quelle est la différence entre une blonde et une boule de bowling ?
   On ne peut mettre que trois doigts dans la boule de bowling.
*/

namespace YelloKiller
{
    class Hero1 : Case
    {
        Vector2 position, positionDesiree;
        Rectangle? sourceRectangle;
        Rectangle rectangle;
        Texture2D texture;

        float vitesse_animation, index;
        int vitesse_sprite, maxIndex, countshuriken;
        public bool ishero1;
        bool bougerHaut, bougerBas, bougerDroite, bougerGauche;

        public Hero1(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            vitesse_animation = 0.008f;
            vitesse_sprite = 1;
            index = 0;
            maxIndex = 0;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);
            countshuriken = 20;
            ishero1 = false;
            positionDesiree = position;
            bougerBas = bougerDroite = bougerGauche = bougerHaut = true;
        }

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 PositionDesiree
        {
            get { return positionDesiree; }
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("NinjaTrans");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero2 hero2, GameplayScreenCoop yk, ref Rectangle camera, List<Shuriken> _shuriken, MoteurAudio moteurAudio)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Space) && countshuriken > 0)
            {
                countshuriken--;
                Console.WriteLine("il reste : " + countshuriken + " shurikens pour hero1.");
                ishero1 = true;
                _shuriken.Add(new Shuriken(yk, new Vector2(position.X, position.Y), this.texture.Width, this, hero2));
                moteurAudio.SoundBank.PlayCue("shuriken");
            }
            else
                ishero1 = false;
            
            if (!ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Z))                        // arreter le sprite
            {
                if (sourceRectangle.Value.Y == 133)
                    sourceRectangle = new Rectangle(24, 133, 16, 28);
                if (sourceRectangle.Value.Y == 198)
                    sourceRectangle = new Rectangle(24, 198, 16, 28);
                if (sourceRectangle.Value.Y == 230)
                    sourceRectangle = new Rectangle(24, 230, 16, 28);
                if (sourceRectangle.Value.Y == 166)
                    sourceRectangle = new Rectangle(24, 166, 16, 28);
            }

            if (!bougerHaut)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 133, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (camera.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 200)
                        camera.Y -= vitesse_sprite;
                }

                else
                {
                    bougerHaut = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!bougerBas)
            {
                if (position != positionDesiree)
                {
                    position.Y += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (camera.Y + vitesse_sprite < 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 200)
                        camera.Y += vitesse_sprite;
                }
                else
                {
                    bougerBas = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!bougerGauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (camera.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 200)
                        camera.X -= vitesse_sprite;
                }

                else
                {
                    bougerGauche = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!bougerDroite)
            {
                if (position != positionDesiree)
                {
                    position.X += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (camera.X + vitesse_sprite < 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 200)
                        camera.X += vitesse_sprite;
                }

                else
                {
                    bougerDroite = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (bougerHaut && bougerBas && bougerDroite && bougerGauche)
            {
                if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.LeftShift))
                {
                    vitesse_sprite = 2;
                    vitesse_animation = 0.016f;
                }
                else
                {
                    vitesse_sprite = 1;
                    vitesse_animation = 0.008f;
                }

                if (position.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Z) &&
                    (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0 &&
                    (position.X != hero2.PositionDesiree.X || position.Y - 28 != hero2.PositionDesiree.Y))
                {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        bougerHaut = false;
                }

                else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.S) &&
                         (int)carte.Cases[(int)((position.Y + 28) / 28), (int)(position.X) / 28].Type > 0 &&
                         (position.X != hero2.PositionDesiree.X || position.Y + 28 != hero2.PositionDesiree.Y))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X;
                    positionDesiree.Y = position.Y + 28;
                    bougerBas = false;
                }

                else if (position.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Q) &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0 &&
                         (position.Y != hero2.PositionDesiree.Y || position.X - 28 != hero2.PositionDesiree.X))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    bougerGauche = false;
                }

                else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.D) &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0 &&
                         (position.Y != hero2.PositionDesiree.Y || position.X + 28 != hero2.PositionDesiree.X))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    bougerDroite = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera, Carte carte)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
            spriteBatch.DrawString(ScreenManager.font, "Le joueur 1 a encore " + countshuriken.ToString() + " shurikens.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.BurlyWood);
        }
    }
}