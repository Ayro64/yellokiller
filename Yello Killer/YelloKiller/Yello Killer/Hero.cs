using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/*Avant que vous ne gueuliez tous en disant "Hey mais c'est koi ce putain de hero, on en a pas assez ?", et bah la reponse est non.
 Pour le mode solo, on a besoin que d'un hero (hehe), or, les fonctions update des hero1 et hero2 ont besoin respectivement de hero2 et hero1.
 De plus, il n'y a pas a détecter les collisions entre les deux héros puisqu'il n'y en a d'un (héhé). Il a donc été plus simple de faire une classe
 GameplayScreenSolo plus adaptée, très ressemblante à la classe GamePlayScreenCoop (que j'ai renommé au passage). De plus le shuriken a un deuxieme constructeur, 
 qui ne fait appelle qu'au hero.
 La classe Ennemi a également une deuxieme fonction Update (appelée UpdateInSolo) qui ne prend que hero en parametre, et pas hero1 et hero2.*/

namespace Yellokiller.Yello_Killer
{
    class Hero : Case
    {
        Vector2 position, positionDesiree;
        float vitesse_animation = 0.001f;
        int vitesse_sprite = 1;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Rectangle rectangle;
        Texture2D texture;
        int countshuriken = 100;
        public bool ishero = false;
        bool bougerHaut, bougerBas, bougerDroite, bougerGauche;
        
        public Hero(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            bougerBas = bougerDroite = bougerGauche = bougerHaut = true;
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

        public void Update(GameTime gameTime, Carte carte, GameplayScreenSolo yk, ref Rectangle camera, List<Shuriken> _shuriken)
        {
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.RightControl) && countshuriken > 0)
            {
                countshuriken--;
                Console.WriteLine("il reste : " + countshuriken + " shurikens pour hero2.");
                ishero = true;
                _shuriken.Add(new Shuriken(yk, new Vector2(position.X, position.Y), this.texture.Width, this));
            }
            else
                ishero = false;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);

            if (!ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up))    // arreter le sprite
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
                if (position != positionDesiree && index < maxIndex)
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
                if (position != positionDesiree && index < maxIndex)
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
                if (position != positionDesiree && index < maxIndex)
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
                if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.RightShift))
                {
                    vitesse_sprite = 2;
                    vitesse_animation = 0.016f;
                }
                else
                {
                    vitesse_sprite = 1;
                    vitesse_animation = 0.008f;
                }

                if (position.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up) &&
                    (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.X = position.X;
                    positionDesiree.Y = position.Y - 28;
                    bougerHaut = false;
                }


                else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Down) &&
                          (int)carte.Cases[(int)((position.Y + 28) / 28), (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.X = position.X;
                    positionDesiree.Y = position.Y + 28;
                    bougerBas = false;
                }


                else if (position.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Left) &&
                    (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    bougerGauche = false;
                }

                else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Right) &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    bougerDroite = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera, Carte carte, Hero hero2)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
            spriteBatch.DrawString(ScreenManager.font, "Il reste " + countshuriken.ToString() + " shurikens.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.BurlyWood);
        }
    }
}