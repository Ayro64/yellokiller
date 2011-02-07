using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Yellokiller.Yello_Killer
{
    class Hero2 : Case
    {
        Vector2 position;
        float vitesse_animation = 0.008f;
        int vitesse_sprite = 1;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Rectangle rectangle;
        Texture2D texture;
        int countshuriken = 100;
        public bool ishero2 = false;

        public bool monter = true, descendre = true, droite = true, gauche = true;

        public Hero2(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public Texture2D Texture
        {
            get { return texture; }
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

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Ninja2Trans");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero1 hero1, GameplayScreenCoop yk, List<Shuriken> _shuriken)
        {
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.RightControl) && countshuriken > 0)
            {
                countshuriken--;
                Console.WriteLine("il reste : " + countshuriken + " shurikens pour hero2.");
                ishero2 = true;
                _shuriken.Add(new Shuriken(yk, new Vector2(position.X, position.Y), this.texture.Width, hero1, this));
            }
            else
                ishero2 = false;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);
            Moteur_physique.Collision(this.rectangle, hero1.Rectangle, ref droite, ref gauche, ref monter, ref descendre);

            if (!ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up))                        // arreter le sprite
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

            if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.RightShift))
            {
                vitesse_sprite = 3;
                vitesse_animation = 0.016f;
            }
            else
            {
                vitesse_sprite = 1;
                vitesse_animation = 0.008f;
            }

            if (position.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up) && monter &&
                (int)carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type > 0 &&
                (int)carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 133, 16, 28);
                    position.Y -= vitesse_sprite;
                }
                else
                    index = 0f;
            }

            else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Down) && descendre &&
                     (int)carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type > 0 &&
                     (int)carte.Cases[(int)(position.Y / 28) + 1, (int)position.X / 28].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    position.Y += vitesse_sprite;
                }
                else
                    index = 0f;
            }

            if (position.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Left) && gauche &&
                (int)carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type > 0 &&
                (int)carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    position.X -= vitesse_sprite;
                }
                else
                    index = 0f;
            }

            else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 18 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Right) && droite &&
                     (int)carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type > 0 &&
                     (int)carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    position.X += vitesse_sprite;
                }
                else
                    index = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera, Carte carte)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
            spriteBatch.DrawString(ScreenManager.font, "Le joueur 2 a encore " + countshuriken.ToString() + " shurikens.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.BurlyWood);
        }
    }
}