﻿using System;
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

namespace YelloKiller
{
    class Hero : Sprite
    {
        Vector2 positionDesiree;
        Rectangle rectangle;
        float vitesseAnimation, index;
        int maxIndex, nombreShuriken, vitesseSprite, numeroHero;
        public bool ishero;
        bool monter, descendre, droite, gauche;
        Keys up, down, right, left, shuriken, courir;

        public Hero(Vector2 position, Keys up, Keys down, Keys right, Keys left, Keys shuriken, Keys courir, int numeroHero, int nombreShuriken)
            : base(position)
        {
            this.position = position;
            positionDesiree = position;
            this.numeroHero = numeroHero;
            SourceRectangle = new Rectangle(25, 133, 16, 25);
            monter = true;
            descendre = true;
            droite = true;
            gauche = true;
            vitesseSprite = 1;
            vitesseAnimation = 0.008f;
            index = 0;
            maxIndex = 0;
            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 26);
            this.nombreShuriken = nombreShuriken;
            ishero = false;
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
            this.shuriken = shuriken;
            this.courir = courir;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            if (numeroHero == 1)
                base.LoadContent(content, "Hero1");
            else
                base.LoadContent(content, "Hero2");

            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, ref Rectangle camera, List<Shuriken> _shuriken, MoteurAudio moteurAudio, ContentManager content, Hero hero2)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(shuriken) && nombreShuriken > 0)
            {
                nombreShuriken--;
                ishero = true;
                _shuriken.Add(new Shuriken(position, this, content));
                moteurAudio.SoundBank.PlayCue("shuriken");
            }
            else
                ishero = false;

            if (!ServiceHelper.Get<IKeyboardService>().TouchePressee(up))    // arreter le sprite
            {
                if (SourceRectangle.Value.Y == 133)
                    SourceRectangle = new Rectangle(24, 133, 16, 28);
                if (SourceRectangle.Value.Y == 198)
                    SourceRectangle = new Rectangle(24, 198, 16, 28);
                if (SourceRectangle.Value.Y == 230)
                    SourceRectangle = new Rectangle(24, 230, 16, 28);
                if (SourceRectangle.Value.Y == 166)
                    SourceRectangle = new Rectangle(24, 166, 16, 28);
            }

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesseSprite;
                    SourceRectangle = new Rectangle((int)index * 48, 133, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (numeroHero == 1)
                    {
                        if (camera.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 200)
                            camera.Y -= vitesseSprite;
                    }
                }

                else
                {
                    monter = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!descendre)
            {
                if (position != positionDesiree)
                {
                    position.Y += vitesseSprite;
                    SourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (numeroHero == 1)
                    {
                        if (camera.Y + vitesseSprite < 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 200)
                            camera.Y += vitesseSprite;
                    }
                }
                else
                {
                    descendre = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!gauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= vitesseSprite;
                    SourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (numeroHero == 1)
                    {
                        if (camera.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 200)
                            camera.X -= vitesseSprite;
                    }
                }

                else
                {
                    gauche = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!droite)
            {
                if (position != positionDesiree)
                {
                    position.X += vitesseSprite;
                    SourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (numeroHero == 1)
                    {
                        if (camera.X + vitesseSprite < 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 200)
                            camera.X += vitesseSprite;
                    }
                }

                else
                {
                    droite = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (monter && descendre && droite && gauche)
            {
                if (ServiceHelper.Get<IKeyboardService>().TouchePressee(courir))
                {
                    vitesseSprite = 4;
                    vitesseAnimation = 0.016f;
                }
                else
                {
                    vitesseSprite = 2;
                    vitesseAnimation = 0.008f;
                }

                if (hero2 == null)
                {

                    if (position.Y > 5 && ServiceHelper.Get<IKeyboardService>().TouchePressee(up) &&
                        (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }


                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(down) &&
                             (int)carte.Cases[(int)((position.Y + 28) / 28), (int)(position.X) / 28].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }


                    else if (position.X > 10 && ServiceHelper.Get<IKeyboardService>().TouchePressee(left) &&
                             (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }


                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(right) &&
                             (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X + 28;
                        positionDesiree.Y = position.Y;
                        droite = false;
                        /*etienne++;

                        if (etienne > 4)
                        {
                            positionDesiree.X = position.X + 28;
                            positionDesiree.Y = position.Y;
                            bougerDroite = false;
                        }
                        else
                            sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);*/
                    }
                }

                else
                {
                    if (position.Y > 5 && ServiceHelper.Get<IKeyboardService>().TouchePressee(up) &&
                        (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0 &&
                        (position.X != hero2.PositionDesiree.X || position.Y - 28 != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }

                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(down) &&
                              (int)carte.Cases[(int)((position.Y + 28) / 28), (int)(position.X) / 28].Type > 0 &&
                              (position.X != hero2.PositionDesiree.X || position.Y + 28 != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }

                    else if (position.X > 10 && ServiceHelper.Get<IKeyboardService>().TouchePressee(left) &&
                             (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0 &&
                             (position.X - 28 != hero2.PositionDesiree.X || position.Y != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }

                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(right) &&
                             (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0 &&
                             (position.X + 28 != hero2.PositionDesiree.X || position.Y != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X + 28;
                        positionDesiree.Y = position.Y;
                        droite = false;
                    }
                }
            }
        }

        public Vector2 PositionDesiree
        {
            get { return positionDesiree; }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
            if (numeroHero == 1)
                spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreShuriken.ToString() + " shurikens au joueur 1.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.BurlyWood);
            else
                spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreShuriken.ToString() + " shurikens au joueur 2.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.BurlyWood);
        }
    }
}