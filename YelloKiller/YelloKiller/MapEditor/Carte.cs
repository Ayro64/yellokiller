using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Carte
    {
        Case[,] _case;
        Vector2 origineJoueur1, origineJoueur2, positionTemporaire;
        List<byte> rotationsDesStatues;
        List<Vector2> _originesGarde, _originesBoss, _originesStatues, bonusShurikens;
        List<List<Vector2>> _originesPatrouilleur, _originesPatrouilleur_a_cheval;
        int salaire;

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
            origineJoueur1 = -Vector2.One;
            origineJoueur2 = -Vector2.One;
            positionTemporaire = Vector2.Zero;
            _originesGarde = new List<Vector2>();
            _originesPatrouilleur = new List<List<Vector2>>();
            _originesPatrouilleur_a_cheval = new List<List<Vector2>>();
            _originesBoss = new List<Vector2>();
            _originesStatues = new List<Vector2>();
            rotationsDesStatues = new List<byte>();
            bonusShurikens = new List<Vector2>();
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
                    _case[y, x].Position = new Vector2(x - camera.X + 2, y - camera.Y);

                    _case[y, x].DrawInMapEditor(spriteBatch, content);
                }
            }
        }

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            for (int y = Taille_Map.HAUTEUR_MAP - 1; y >= 0; y -= 2)
            {
                for (int x = Taille_Map.LARGEUR_MAP - 1; x >= 0; x -= 2)
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

        public void OuvrirCartePourMenu(string nomDeFichier)
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

            file.Close();
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

            while (line != "Patrouilleurs" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesGarde.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Patrouilleurs A Cheval" && line != null)
            {
                line = file.ReadLine();
                _originesPatrouilleur.Add(new List<Vector2>());

                while (line != "New" && line != "Patrouilleurs A Cheval")
                {
                    positionTemporaire.X = Convert.ToInt32(line);
                    line = file.ReadLine();
                    positionTemporaire.Y = Convert.ToInt32(line);
                    _originesPatrouilleur[_originesPatrouilleur.Count - 1].Add(positionTemporaire);
                    line = file.ReadLine();
                }
            }

            line = file.ReadLine();

            while (line != "Boss" && line != null)
            {
                line = file.ReadLine();
                _originesPatrouilleur_a_cheval.Add(new List<Vector2>());

                while (line != "New" && line != "Boss")
                {
                    positionTemporaire.X = Convert.ToInt32(line);
                    line = file.ReadLine();
                    positionTemporaire.Y = Convert.ToInt32(line);
                    _originesPatrouilleur_a_cheval[_originesPatrouilleur_a_cheval.Count - 1].Add(positionTemporaire);
                    line = file.ReadLine();
                }
            }

            line = file.ReadLine();

            while (line != "Statue dragon" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesBoss.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Bonus Shurikens" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesStatues.Add(positionTemporaire);
                line = file.ReadLine();
                rotationsDesStatues.Add(Convert.ToByte(line));
                line = file.ReadLine();
            }
            
            line = file.ReadLine();

            while (line != "Salaire")
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                bonusShurikens.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            salaire = Convert.ToInt32(line);
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

        public int Salaire
        {
            get { return salaire; }
        }

        public List<Vector2> OriginesGardes
        {
            get { return _originesGarde; }
        }

        public List<List<Vector2>> OriginesPatrouilleurs
        {
            get { return _originesPatrouilleur; }
        }

        public List<List<Vector2>> OriginesPatrouilleursAChevaux
        {
            get { return _originesPatrouilleur_a_cheval; }
        }

        public List<Vector2> OriginesBoss
        {
            get { return _originesBoss; }
        }

        public List<Vector2> OriginesStatues
        {
            get { return _originesStatues; }
        }

        public Vector2 OrigineJoueur1
        {
            get { return origineJoueur1; }
        }

        public Vector2 OrigineJoueur2
        {
            get { return origineJoueur2; }
        }

        public List<Vector2> BonusShuriken
        {
            get { return bonusShurikens; }
        }

        public Case[,] Cases
        {
            get { return _case; }
            set { _case = value; }
        }

        public List<byte> RotationsDesStatues
        {
            get { return rotationsDesStatues; }
        }
    }
}