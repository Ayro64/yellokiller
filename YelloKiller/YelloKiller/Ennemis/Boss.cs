using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        int vie;

        public Boss(Vector2 position)
            : base(position)
        {
            Position = position;
            SourceRectangle = new Rectangle(26, 64, 18, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 18, 26);
            MaxIndex = 0;
            positionDesiree = position;
            vie = 5;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, "Boss");
            MaxIndex = maxIndex;
        }

        public int Vie
        {
            get { return vie; }
            set { vie = value; }
        }

        public void Update(GameTime gameTime, List<Shuriken> shuriken, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24));
            /*Théo, modifier tu dois la ligne du dessus, ca ne peut pas etre les meme parametres que les gardes, aux differentes positions des sprites dans la feuille de sprite ils correspondent 
             * Cette meme ligne pour les patrouilleurs avec et sans chevaux tu regarderas, tu comprendras*/

            foreach (Shuriken _shuriken in shuriken)
            {
                //Console.WriteLine("_shuriken = " + (int)(_shuriken.position.Y / 28) + " , boss = " + (int)(this.position.Y / 28));

                if (Monter && Descendre && Droite && Gauche)
                {
                    if (_shuriken.Direction == Vector2.UnitX && (int)(position.X / 28) - (int)(_shuriken.position.X) / 28 < 7
                        && (int)(position.Y) / 28 == (int)(_shuriken.position.Y) / 28)
                    {
                        if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                            (int)carte.Cases[(int)(position.Y) / 28 + 1, (int)(position.X) / 28].Type > 0)
                        {
                            Console.WriteLine("Bas !");
                            positionDesiree.Y += 28;
                            Descendre = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        else if (position.Y > 0 && (int)carte.Cases[(int)(position.Y) / 28 - 1, (int)(position.X) / 28].Type > 0)
                        {
                            Console.WriteLine("Haut !");
                            positionDesiree.Y -= 28;
                            Monter = false;
                            break;
                        }
                    }

                    if (_shuriken.Direction == Vector2.UnitY && (int)(position.Y / 28) - (int)(_shuriken.position.Y) / 28 < 7
                        && (int)(position.X) / 28 == (int)(_shuriken.position.X) / 28)
                    {
                        if (position.X < 28 * Taille_Map.LARGEUR_MAP - 1 && (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X) / 28 + 1].Type > 0)
                        {
                            Console.WriteLine("Droite !");
                            positionDesiree.X += 28;
                            Droite = false;
                            break;
                        }
                        else if (position.X > 0 && (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X) / 28 - 1].Type > 0)
                        {
                            Console.WriteLine("Gauche !");
                            positionDesiree.X -= 28;
                            Gauche = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}