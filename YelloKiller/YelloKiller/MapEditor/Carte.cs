using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Carte
    {
        Case[,] _case;
        Vector2 origineJoueur1, origineJoueur2, positionTemporaire;
        List<Vector2> _originesGarde, _originesPatrouilleur, _originesPatrouilleur_a_cheval, _originesBoss;

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
            origineJoueur1 = -Vector2.One;
            origineJoueur2 = -Vector2.One; 
            positionTemporaire = Vector2.Zero;
            _originesGarde = new List<Vector2>();
            _originesPatrouilleur = new List<Vector2>();
            _originesPatrouilleur_a_cheval = new List<Vector2>();
            _originesBoss = new List<Vector2>();
        }

        public void DrawInGame(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28 + camera.Height; y >= camera.Y / 28 - 2; y--)
            {
                if (y < 60 && y >= 0)
                {
                    for (int x = camera.X / 28 + camera.Width; x >= camera.X / 28 - 2; x--)
                    {
                        if (x < 80 && x >= 0)
                        {
                            _case[y, x].Position = 28 * new Vector2(x, y) - new Vector2(camera.X, camera.Y);

                            _case[y, x].DrawInGame(spriteBatch, content);
                        }
                    }
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

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            for (int y = Taille_Map.HAUTEUR_MAP - 1; y >= 0; y--)
            {
                for (int x = Taille_Map.LARGEUR_MAP - 1; x >= 0; x--)
                {
                    _case[y, x].Position = new Vector2(x, y);

                    _case[y, x].DrawInMenu(spriteBatch, content, origine);
                }
            }
        }

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _case[y, x] = new Case(new Vector2(x, y), TypeCase.herbe);
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
                    Switch(line[x], x, y);
            }

            line = file.ReadLine();
            line = file.ReadLine();
            origineJoueur1.X = Convert.ToInt32(line);

            line = file.ReadLine();
            origineJoueur1.Y = Convert.ToInt32(line);

            if (nomDeFichier[0] == 'C') // Si la carte est en cooperation.
            {
                line = file.ReadLine();
                origineJoueur2.X = Convert.ToInt32(line);

                line = file.ReadLine();
                origineJoueur2.Y = Convert.ToInt32(line);
            }

            line = file.ReadLine();
            line = file.ReadLine();

            while (line != "Patrouilleurs")
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesGarde.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Patrouilleurs A Cheval")
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesPatrouilleur.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Boss")
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesPatrouilleur_a_cheval.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesBoss.Add(positionTemporaire);
                line = file.ReadLine();
            }

            file.Close();
        }

        public void Switch(char c, int x, int y)
        {
            switch (c)
            {
                // a, A ,s, m, t, b, B, T, h, H, p, r, f, F, d, g, c, G, P, C, o, O
                case ('a'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre);
                    break;
                case ('A'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre2);
                    break;
                case ('s'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.buissonSurHerbe);
                    break;
                case ('m'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc);
                    break;
                case ('t'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc);
                    break;
                case ('b'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bois);
                    break;
                case ('B'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisCarre);
                    break;
                case ('T'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tapisRougeBC);
                    break;
                case ('h'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbe);
                    break;
                case ('H'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeFoncee);
                    break;
                case ('p'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.piedDeMurBois);
                    break;
                case ('r'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.terre);
                    break;
                case ('f'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fondNoir);
                    break;
                case ('F'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurFN);
                    break;
                case ('d'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurDroite);
                    break;
                case ('g'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurGauche);
                    break;
                case ('c'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoir);
                    break;
                case ('l'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.Lit);
                    break;
                case ('L'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode);
                    break;
                case ('y'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.TableMoyenne);
                    break;
                case ('Y'):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.GrandeTable);
                    break;
            }
        }

        public List<Vector2> OriginesGardes
        {
            get { return _originesGarde; }
        }

        public List<Vector2> OriginesPatrouilleurs
        {
            get { return _originesPatrouilleur; }
        }

        public List<Vector2> OriginesPatrouilleursAChevaux
        {
            get { return _originesPatrouilleur_a_cheval; }
        }

        public List<Vector2> OriginesBoss
        {
            get { return _originesBoss; }
        }

        public Vector2 OrigineJoueur1
        {
            get { return origineJoueur1; }
        }

        public Vector2 OrigineJoueur2
        {
            get { return origineJoueur2; }
        }

        public Case[,] Cases
        {
            get { return _case; }
            set { _case = value; }
        }
    }
}