﻿using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Carte
    {
        Case[,] _case;
        public Vector2 origineJoueur1, origineJoueur2;
        public List<Vector2> _originesGarde;
        public List<Vector2> _originesPatrouilleur;
        public List<Vector2> _originesPatrouilleur_a_cheval;

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
            origineJoueur1 = new Vector2(0, 0);
            origineJoueur2 = new Vector2(0, 0);
            _originesGarde = new List<Vector2>();
            _originesPatrouilleur = new List<Vector2>();
            _originesPatrouilleur_a_cheval = new List<Vector2>();
        }

        public Case[,] Cases
        {
            get { return _case; }
            set { _case = value; }
        }

        public bool ValidCoordinates(Case _case)
        {
            if (_case.Position.X < 0)
                return false;
            if (_case.Position.Y < 0)
                return false;
            if ((int)_case.Position.X / 28 >= Taille_Map.LARGEUR_MAP)
                return false;
            if ((int)_case.Position.Y / 28 >= Taille_Map.HAUTEUR_MAP)
                return false;

            return true;
        }

        public void DrawInGame(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28 + camera.Height; y >= 0; y--)
            {
                for (int x = camera.X / 28 + camera.Width; x >= 0; x--)
                {
                    _case[y, x].Position = 28 * new Vector2(x, y) - new Vector2(camera.X, camera.Y);

                    _case[y, x].DrawInGame(spriteBatch, content);
                }
            }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y + camera.Height - 1; y >= 0; y--)
            {
                for (int x = camera.X + camera.Width - 1; x >= 0; x--)
                {
                    _case[y, x].Position = new Vector2(x - camera.X, y - camera.Y);

                    _case[y, x].DrawInMapEditor(spriteBatch, content);
                }
            }
        }

        /*private int stringToInt(string s)
        {
            int ret = 0, dec = 1;

            for (int i = s.Length - 1; i >= 0; i--)
            {
                ret += dec * (int)(s[i] - 48);
                dec *= 10;
            }

            return ret;
        }*/

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _case[y, x] = new Case(new Vector2(x, y), new Rectangle(), TypeCase.herbe);
        }

        public void OuvrirCarteCoop(string nomDeFichier)
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
                        case ('a'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.arbre);
                            break;
                        case ('A'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.arbre2);
                            break;
                        case ('s'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.buissonSurHerbe);
                            break;
                        case ('m'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.murBlanc);
                            break;
                        case ('t'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.tableauMurBlanc);
                            break;
                        case ('b'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.bois);
                            break;
                        case ('B'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.boisCarre);
                            break;
                        case ('T'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.tapisRougeBC);
                            break;
                        case ('h'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            break;
                        case ('H'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbeFoncee);
                            break;
                        case ('p'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.piedDeMurBois);
                            break;
                        case ('r'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.terre);
                            break;
                        case ('f'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.fondNoir);
                            break;
                        case ('F'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurFN);
                            break;
                        case ('d'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurDroite);
                            break;
                        case ('g'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurGauche);
                            break;
                        case ('c'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.carlageNoir);
                            break;

                        case ('G'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesGarde.Add(new Vector2(x, y));
                            break;
                        case ('P'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesPatrouilleur.Add(new Vector2(x, y));
                            break;
                        case ('C'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesPatrouilleur_a_cheval.Add(new Vector2(x, y));
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

        public void OuvrirCarteSolo(string nomDeFichier)
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
                        case ('a'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.arbre);
                            break;
                        case ('A'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.arbre2);
                            break;
                        case ('s'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.buissonSurHerbe);
                            break;
                        case ('m'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.murBlanc);
                            break;
                        case ('t'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.tableauMurBlanc);
                            break;
                        case ('b'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.bois);
                            break;
                        case ('B'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.boisCarre);
                            break;
                        case ('T'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.tapisRougeBC);
                            break;
                        case ('h'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            break;
                        case ('H'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbeFoncee);
                            break;
                        case ('p'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.piedDeMurBois);
                            break;
                        case ('r'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.terre);
                            break;
                        case ('f'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.fondNoir);
                            break;
                        case ('F'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurFN);
                            break;
                        case ('d'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurDroite);
                            break;
                        case ('g'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.finMurGauche);
                            break;
                        case ('c'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.carlageNoir);
                            break;

                        case ('G'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesGarde.Add(new Vector2(x, y));
                            break;
                        case ('P'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesPatrouilleur.Add(new Vector2(x, y));
                            break;
                        case ('C'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            _originesPatrouilleur_a_cheval.Add(new Vector2(x, y));
                            break;
                        case ('o'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            origineJoueur1 = new Vector2(x, y);
                            break;
                        case ('O'):
                            _case[y, x] = new Case(28 * new Vector2(x, y), new Rectangle(), TypeCase.herbe);
                            origineJoueur1 = new Vector2(x, y);
                            break;
                    }
                }
            }

            /*line = file.ReadLine();
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