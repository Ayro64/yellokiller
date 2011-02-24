using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Si tu vois un bateau qui flotte sur l'eau c'est que ta mère n'est pas à bord !!!!!

    Ta mère est tellement vieille que dans sa première photo de classe on voit Jésus !

    Ta mère est tellement grosse que quand elle passe devant le soleil tout le monde crie : "une éclipse "!
s
    Ta mère est tellement radine que quand elle va aux toilettes elle utilise les deux côtés du papier toilette.
 
*/

namespace YelloKiller
{
    class Hero2 : Sprite
    {
        Vector2 position, positionDesiree;
        public Rectangle? sourceRectangle;
        Rectangle rectangle;
        Texture2D texture;

        float vitesse_animation, index;
        int vitesse_sprite, maxIndex, countshuriken;
        public bool ishero2;
        bool bougerHaut, bougerBas, bougerDroite, bougerGauche;

        public Hero2(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            vitesse_animation = 0.008f;
            vitesse_sprite = 1;
            index = 0;
            maxIndex = 0;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);            
            countshuriken = 0;
            ishero2 = false;
            positionDesiree = position;
            bougerBas = bougerDroite = bougerGauche = bougerHaut = true;
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
            texture = content.Load<Texture2D>("Hero2");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero1 hero1, GameplayScreenCoop yk, List<Shuriken> _shuriken, MoteurAudio moteurAudio)
        {
            Position = position;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.RightControl) && countshuriken > 0)
            {
                countshuriken--;
                Console.WriteLine("il reste : " + countshuriken + " shurikens pour hero2.");
                ishero2 = true;
                _shuriken.Add(new Shuriken(yk, new Vector2(position.X, position.Y), this.texture.Width, hero1, this));
                moteurAudio.SoundBank.PlayCue("shuriken");
            }
            else
                ishero2 = false;
            
            if (!ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Up))                        // arreter le sprite
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
                if (position != positionDesiree && index < maxIndex)
                {
                    position.Y += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
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
                if (position != positionDesiree && index < maxIndex)
                {
                    position.X -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
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
                if (position != positionDesiree && index < maxIndex)
                {
                    position.X += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
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
                if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.RightShift))
                {
                    vitesse_sprite = 4;
                    vitesse_animation = 0.016f;
                }
                else
                {
                    vitesse_sprite = 2;
                    vitesse_animation = 0.008f;
                }

                if (position.Y > 5 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Up) &&
                    (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0 &&
                    (position.X != hero1.PositionDesiree.X || position.Y - 28 != hero1.PositionDesiree.Y))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X;
                    positionDesiree.Y = position.Y - 28;
                    bougerHaut = false;
                }

                else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Down) &&
                         (int)carte.Cases[(int)((position.Y + 28) / 28), (int)(position.X) / 28].Type > 0 &&
                         (position.X != hero1.PositionDesiree.X || position.Y + 28 != hero1.PositionDesiree.Y))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X;
                    positionDesiree.Y = position.Y + 28;
                    bougerBas = false;
                }

                else if (position.X > 10 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Left) &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0 &&
                         (position.Y != hero1.PositionDesiree.Y || position.X - 28 != hero1.PositionDesiree.X))
                {
                    moteurAudio.SoundBank.PlayCue("pasBois");
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    bougerGauche = false;
                }

                else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Right) &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0 &&
                         (position.Y != hero1.PositionDesiree.Y || position.X + 28 != hero1.PositionDesiree.X))
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
            spriteBatch.DrawString(ScreenManager.font, "Le joueur 2 a encore " + countshuriken.ToString() + " shurikens.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.BurlyWood);
        }
    }
}