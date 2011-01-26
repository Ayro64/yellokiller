using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Collections.Generic;

namespace Yellokiller
{
    class Carte
    {
        Case[,] _case;
        public Vector2 origineJoueur1 = new Vector2(0, 0), origineJoueur2 = new Vector2(0, 0);
        public List<Vector2> _originesEnnemis = new List<Vector2>();

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
        }

        public Case[,] Cases
        {
            get { return _case; }
            set { _case = value; }
        }

        public bool ValidCoordinates(float x,float y)
        {
            if (x < 0)
                return false;
            if (y < 0)
                return false;
            if (x >= Cases.GetLength(1))
                return false;
            if (y >= Cases.GetLength(0))
                return false;
            return true;
        }

        public void DrawInGame(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28; y < camera.Y / 28 + camera.Height + 1; y++)
            {
                for (int x = camera.X / 28; x < camera.X / 28 + camera.Width + 1; x++)
                {
                    _case[y, x].Position = 28 * new Vector2(x,y) - new Vector2(camera.X, camera.Y);
                    _case[y, x].DrawInGame(spriteBatch, content);
                }
            }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y; y < camera.Y + camera.Height; y++)
            {
                for (int x = camera.X; x < camera.X + camera.Width; x++)
                {
                    _case[y, x].Position = new Vector2(x - camera.X, y - camera.Y);
                    _case[y, x].DrawInMapEditor(spriteBatch, content);
                }
            }
        }

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

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    _case[y, x] = new Case(new Vector2(x, y ),new Rectangle(), TypeCase.herbe);
                }
            }
        }

        public void OuvrirCarte(string nomDeFichier)
        {
            StreamReader file = new StreamReader(nomDeFichier);
            string line;

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
            {
                line = file.ReadLine();
                if (line == "")
                    line = file.ReadLine();
                else if (line == null)
                    break;
                for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                {
                    switch (line[x])
                    {
                        case ('h'):
                            _case[y,x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            break;
                        case ('H'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbeFoncee);
                            break;
                        case ('a'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.arbre);
                            break;
                        case ('m'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.mur);
                            break;            
                        case ('M'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.maison);
                            break;
                        case ('E'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesEnnemis.Add(new Vector2(x, y));
                            break;
                        case ('o'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            origineJoueur1 = new Vector2(x, y);
                            break;
                        case ('O'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            origineJoueur2 = new Vector2(x, y);
                            break;                        
                    }
                }
            }
            /*
            line = file.ReadLine();
            origineJoueur1.X = stringToInt(line);

            line = file.ReadLine();
            origineJoueur1.Y = stringToInt(line);

            line = file.ReadLine();
            origineJoueur2.X = stringToInt(line);

            line = file.ReadLine();
            origineJoueur2.Y = stringToInt(line);*/

            file.Close();
        }
    }
}