using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;




namespace Yellokiller
{
    class Map
    {
        string nomFichier;
        public char[,] map = new char[Taille_Map.HAUTEUR_MAP + 1, Taille_Map.LARGEUR_MAP + 1];
        public int LARGEUR_MAP = Taille_Map.LARGEUR_MAP, HAUTEUR_MAP = Taille_Map.HAUTEUR_MAP;
        public Vector2 origine1 = new Vector2(0, 0), origine2 = new Vector2(0, 0);        

        public Map(string nomFichier)
        {
            this.nomFichier = nomFichier;
            //calculTailleMap();
            mapGen();
        }

        /*private void calculTailleMap()
        {
            StreamReader file = new StreamReader(nomFichier);
            string ligne = file.ReadLine();

            LARGEUR_MAP = ligne.Length;

            ligne = file.ReadLine();

            while (ligne != null)
            {
                HAUTEUR_MAP++;
                ligne = file.ReadLine();
            }
            file.Close();

            HAUTEUR_MAP++;
        }*/

        private int stringToInt(string s)
        {
            int ret = 0, dec = 1;

            for (int i = s.Length - 1; i >= 0; i--)
            {
                ret += dec * (int)(s[i] - 48);
                dec *= 10;
            }

            return ret;
        }

        private void mapGen()
        {
            StreamReader file = new StreamReader(nomFichier);
            string line;

            for (int i = 0; i < HAUTEUR_MAP; i++)
            {
                line = file.ReadLine();
                if (line == "")
                    line = file.ReadLine();
                else if (line == null)
                    break;
                for (int j = 0; j < LARGEUR_MAP; j++)
                {
                    map[i, j] = line[j];
                }
            }

            line = file.ReadLine();
            origine1.X = stringToInt(line);

            line = file.ReadLine();
            origine1.Y = stringToInt(line);

            line = file.ReadLine();
            origine2.X = stringToInt(line);

            line = file.ReadLine();
            origine2.Y = stringToInt(line);

            file.Close();
        }

        private Texture2D LoadContent(ContentManager content, string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            for (int y = 0; y < HAUTEUR_MAP; y++)
            {
                for (int x = 0; x < LARGEUR_MAP; x++)
                {
                    switch (map[y, x])
                    {
                        case 'h':
                            spriteBatch.Draw(LoadContent(content, "herbe"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'a':
                            spriteBatch.Draw(LoadContent(content, "arbre"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'm':
                            spriteBatch.Draw(LoadContent(content, "mur"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'M':
                            spriteBatch.Draw(LoadContent(content, "maison"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'A':
                            spriteBatch.Draw(LoadContent(content, "arbre2"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                    }
                }
            }
        }
    }
}
