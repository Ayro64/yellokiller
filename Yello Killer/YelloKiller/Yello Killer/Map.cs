﻿using System;
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
        public int[,] map = new int[Taille_Map.HAUTEURMAP + 1, Taille_Map.LARGEURMAP + 1];
        public int largeurMap = Taille_Map.LARGEURMAP, hauteurMap = Taille_Map.HAUTEURMAP;
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

            largeurMap = ligne.Length;

            ligne = file.ReadLine();

            while (ligne != null)
            {
                hauteurMap++;
                ligne = file.ReadLine();
            }
            file.Close();

            hauteurMap++;
        }*/

        private int charToInt(char c)
        {
            return c - '0';
        }

        private void mapGen()
        {
            StreamReader file = new StreamReader(nomFichier);
            string line;
            int c = 0;

            for (int i = 0; i < hauteurMap; i++)
            {
                line = file.ReadLine();
                if (line == "")
                    line = file.ReadLine();
                else if (line == null)
                    break;
                for (int j = 0; j < largeurMap; j++)
                {
                    map[i, j] = charToInt(line[j]);
                }
            }

            line = file.ReadLine();

            while (line[c] != ' ')
            {
                origine1.X = 10 * origine1.X + charToInt(line[c]);
                c++;
            }
            c++;
            while (c < line.Length)
            {
                origine1.Y = 10 * origine1.Y + charToInt(line[c]);
                c++;
            }

            c = 0;
            line = file.ReadLine();

            while (line[c] != ' ')
            {
                origine2.X = 10 * origine2.X + charToInt(line[c]);
                c++;
            }
            c++;
            while (c < line.Length)
            {
                origine2.Y = 10 * origine2.Y + charToInt(line[c]);
                c++;
            }
        }

        private Texture2D LoadContent(ContentManager content, string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            for (int y = 0; y < hauteurMap; y++)
            {
                for (int x = 0; x < largeurMap; x++)
                {
                    switch (map[y, x])
                    {
                        case 0:
                            spriteBatch.Draw(LoadContent(content, "herbe"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(LoadContent(content, "arbre"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(LoadContent(content, "mur"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(LoadContent(content, "maison"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 4:
                            spriteBatch.Draw(LoadContent(content, "arbre2"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                    }
                }
            }
        }
    }
}
