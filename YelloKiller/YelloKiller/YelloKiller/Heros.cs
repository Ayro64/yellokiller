﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public enum State
    {
        state_hadoken,
        state_fume,
        state_shuriken,
        state_ball,
        state_sabre,
    };

    class Heros : Sprite
    {
        Vector2 positionDesiree, caseDeDepart;
        Texture2D flamme, textureShuriken, textureHadoken, textureBouleDeFeu, textureFumigene, textureSabre;
        Rectangle rectangle;
        float vitesseAnimation, index, tempsCourir;
        public int NombreShuriken { get; set; }
        public int NombreHadoken { get; set; }
        public int NombreFumigene { get; set; }
        public int NombreBall { get; set; }
        int maxIndex, vitesseSprite;
        public byte NumeroHero { get; private set; }
        public bool isheros { get; private set; }
        public bool attaque { get; private set; }
        bool animation_sabre;
        bool monter, descendre, droite, gauche;
        Keys up, down, right, left, changer_arme, courir, tirer;
        const int NumStates = 5;
        State currentState = State.state_shuriken;
        int state_sabre = 0;

        public Heros(Vector2 position, Keys up, Keys down, Keys right, Keys left, Keys changer_arme, Keys tirer, Keys courir, byte numeroHero)
            : base(position)
        {
            this.position = position;
            positionDesiree = position;
            this.NumeroHero = numeroHero;
            SourceRectangle = new Rectangle(25, 133 - state_sabre, 16, 26);
            monter = true;
            descendre = true;
            droite = true;
            gauche = true;
            animation_sabre = false;
            Regarde_Droite = false;
            Regarde_Gauche = false;
            Regarde_Haut = true;
            Regarde_Bas = false;
            vitesseSprite = 4;
            vitesseAnimation = 0.008f;
            index = 0;
            maxIndex = 0;
            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 26);
            isheros = false;
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
            this.changer_arme = changer_arme;
            this.tirer = tirer;
            this.courir = courir;
            caseDeDepart = new Vector2(X, Y);
            attaque = false;
        }

        public int Distance_Hero_Mur(Carte carte)
        {
            int distance = 0;

            if (Regarde_Haut)
                for (int i = 0; i < 6 && this.Y - i > 0 && carte.Cases[this.Y - i, this.X].EstFranchissable; i++)
                    distance++;
            else if (Regarde_Droite)
                for (int i = 0; i < 6 && this.X + i < Taille_Map.LARGEUR_MAP && carte.Cases[this.Y, this.X + i].EstFranchissable; i++)
                    distance++;
            else if (Regarde_Gauche)
                for (int i = 0; i < 6 && this.X - i > 0 && carte.Cases[this.Y, this.X - i].EstFranchissable; i++)
                    distance++;
            else if (Regarde_Bas)
                for (int i = 0; i < 6 && this.Y + i < Taille_Map.HAUTEUR_MAP && carte.Cases[this.Y + i, this.X].EstFranchissable; i++)
                    distance++;

            return (distance < 6 ? distance : 6);
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            if (NumeroHero == 1)
                base.LoadContent(content, @"Feuilles de sprites\Heros1");
            else
                base.LoadContent(content, @"Feuilles de sprites\Heros2");

            flamme = content.Load<Texture2D>("barre de vitesse");
            textureShuriken = content.Load<Texture2D>(@"Barre infos\shuriken barre");
            textureHadoken = content.Load<Texture2D>(@"Barre infos\hadoken barre");
            textureBouleDeFeu = content.Load<Texture2D>(@"Barre infos\boule de feu barre");
            textureFumigene = content.Load<Texture2D>(@"Barre infos\fumigene barre");
            textureSabre = content.Load<Texture2D>(@"Barre infos\sabre");

            tempsCourir = flamme.Width;

            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, ref Rectangle camera, MoteurParticule particule, List<Shuriken> _shuriken, ContentManager content, Heros heros2, List<Interrupteur> interrupteurs)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            MoteurParticule.Camera = new Vector2(camera.X, camera.Y);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer())
                GameplayScreen.Timer_Update_Collision += gameTime.ElapsedGameTime.TotalSeconds;

            //armes
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(changer_arme) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().ChangerArme() : false))
                currentState = (State)((int)(currentState + 1) % NumStates);

            // animation attaque au sabre
            state_sabre = (currentState == State.state_sabre ? 133 : 0);

            animation_sabre = (ServiceHelper.Get<IKeyboardService>().TouchePressee(tirer) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Tirer() : false)) && (currentState == State.state_sabre);

            if (Regarde_Haut && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 0, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (Regarde_Bas && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 65, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (Regarde_Gauche && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 97, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (Regarde_Droite && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 33, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }


            // animation moteur particule

            switch (currentState)
            {
                case State.state_hadoken:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && NombreHadoken > 0 && NumeroHero == 1)
                    {
                        GameplayScreen.Enable_Timer_Hero1 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero1 == 0)
                        {
                            NombreHadoken--;
                            particule.UpdateExplosions_heros(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && NombreHadoken > 0 && NumeroHero == 2)
                    {
                        GameplayScreen.Enable_Timer_Hero2 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero2 == 0)
                        {
                            NombreHadoken--;
                            particule.UpdateExplosions_heros(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_ball:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && NombreBall > 0 && NumeroHero == 1)
                    {
                        GameplayScreen.Enable_Timer_Hero1 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero1 == 0)
                        {
                            NombreBall--;
                            particule.UpdateBall(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && NombreBall > 0 && NumeroHero == 2)
                    {
                        GameplayScreen.Enable_Timer_Hero2 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero2 == 0)
                        {
                            NombreBall--;
                            particule.UpdateBall(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_fume:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Tirer() : false)) && NombreFumigene > 0)
                    {
                        NombreFumigene--;
                        particule.UpdateFumigene(this);
                    }
                    break;

                case State.state_shuriken:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Tirer() : false)) && NombreShuriken > 0)
                    {
                        NombreShuriken--;
                        isheros = true;
                        _shuriken.Add(new Shuriken(position, this, content));
                        AudioEngine.SoundBank.PlayCue("shuriken");
                    }
                    else
                        isheros = false;
                    break;
                case State.state_sabre:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Tirer() : false))
                        && monter && descendre && droite && gauche)
                    {
                        if (Regarde_Bas)
                        {
                            AllerEnBas(carte, heros2, interrupteurs);
                            AudioEngine.SoundBank.PlayCue("sabre");
                        }
                        else if (Regarde_Haut)
                        {
                            AllerEnHaut(carte, heros2, interrupteurs);
                            AudioEngine.SoundBank.PlayCue("sabre");
                        }
                        else if (Regarde_Gauche)
                        {
                            AllerAGauche(carte, heros2, interrupteurs);
                            AudioEngine.SoundBank.PlayCue("sabre");
                        }
                        else if (Regarde_Droite)
                        {
                            AllerADroite(carte, heros2, interrupteurs);
                            AudioEngine.SoundBank.PlayCue("sabre");
                        }

                        animation_sabre = true;
                        isheros = true;
                        attaque = true;
                    }
                    else
                    {
                        isheros = false;
                        attaque = false;
                    }
                    break;
            }

            if (tempsCourir > 0 && (ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Courir() : false)) && !(monter && descendre && droite && gauche))
                tempsCourir -= 0.1f * gameTime.ElapsedGameTime.Milliseconds;
            else if (tempsCourir < flamme.Width && ServiceHelper.Get<IKeyboardService>().ToucheRelevee(courir) && !(NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Courir() : false))
                tempsCourir += 0.1f * gameTime.ElapsedGameTime.Milliseconds;

            if (!ServiceHelper.Get<IKeyboardService>().TouchePressee(up))    // arreter le sprite
            {
                if (SourceRectangle.Value.Y == 133 - state_sabre)
                    SourceRectangle = new Rectangle(24, 133 - state_sabre, 16, 26);

                if (SourceRectangle.Value.Y == 198 - state_sabre)
                    SourceRectangle = new Rectangle(24, 198 - state_sabre, 16, 26);

                if (SourceRectangle.Value.Y == 230 - state_sabre)
                    SourceRectangle = new Rectangle(24, 230 - state_sabre, 16, 26);

                if (SourceRectangle.Value.Y == 166 - state_sabre)
                    SourceRectangle = new Rectangle(24, 166 - state_sabre, 16, 26);
            }

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesseSprite;
                    SourceRectangle = new Rectangle((int)index * 48, 133 - state_sabre, 16, 26);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (NumeroHero == 1 && camera.Y - vitesseSprite >= 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 308)
                        camera.Y -= vitesseSprite;
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
                    SourceRectangle = new Rectangle((int)index * 48, 198 - state_sabre, 16, 26);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (NumeroHero == 1 && camera.Y + vitesseSprite <= 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 310)
                        camera.Y += vitesseSprite;
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
                    SourceRectangle = new Rectangle((int)index * 48, 230 - state_sabre, 16, 26);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (NumeroHero == 1 && camera.X - vitesseSprite >= 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 304)
                        camera.X -= vitesseSprite;
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
                    SourceRectangle = new Rectangle((int)index * 48, 166 - state_sabre, 16, 26);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;

                    if (NumeroHero == 1 && camera.X + vitesseSprite <= 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 314)
                        camera.X += vitesseSprite;
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
                if ((ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().Courir() : false)) && (int)tempsCourir != 0)
                {
                    vitesseSprite = 4;
                    vitesseAnimation = 0.016f;
                }
                else
                {
                    vitesseSprite = 2;
                    vitesseAnimation = 0.008f;
                }

                if (ServiceHelper.Get<IKeyboardService>().TouchePressee(up) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().AllerEnHaut() : false))
                    AllerEnHaut(carte, heros2, interrupteurs);

                else if (ServiceHelper.Get<IKeyboardService>().TouchePressee(down) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().AllerEnBas() : false))                
                    AllerEnBas(carte, heros2, interrupteurs);

                else if (ServiceHelper.Get<IKeyboardService>().TouchePressee(left) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().AllerAGauche() : false))                
                    AllerAGauche(carte, heros2, interrupteurs);

                else if (ServiceHelper.Get<IKeyboardService>().TouchePressee(right) || (NumeroHero == 1 ? ServiceHelper.Get<IGamePadService>().AllerADroite() : false))                
                    AllerADroite(carte, heros2, interrupteurs);
            }
        }

        public void Draw_Hero(SpriteBatch spriteBatch, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
        }

        public void Draw_Infos(SpriteBatch spriteBatch)
        {
            if (NumeroHero == 1)
            {
                switch (currentState)
                {
                    case State.state_hadoken:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 1 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        spriteBatch.Draw(textureHadoken, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreHadoken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        break;
                    case State.state_ball:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 1 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        spriteBatch.Draw(textureBouleDeFeu, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 35), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreBall.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 1 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        spriteBatch.Draw(textureFumigene, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreFumigene.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 1 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        spriteBatch.Draw(textureShuriken, new Vector2(120, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreShuriken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        break;
                    case State.state_sabre:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 1 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.DarkBlue);
                        spriteBatch.Draw(textureSabre, new Vector2(120, Taille_Ecran.HAUTEUR_ECRAN - 30), Color.White);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(350 - (int)tempsCourir, Taille_Ecran.HAUTEUR_ECRAN - 25), new Rectangle(flamme.Width - (int)tempsCourir, 0, (int)tempsCourir, flamme.Height), Color.White);
            }
            else
            {
                switch (currentState)
                {
                    case State.state_hadoken:

                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 2 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        spriteBatch.Draw(textureHadoken, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreHadoken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        break;
                    case State.state_ball:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 2 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        spriteBatch.Draw(textureBouleDeFeu, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 60), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreBall.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 2 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        spriteBatch.Draw(textureFumigene, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreFumigene.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 2 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        spriteBatch.Draw(textureShuriken, new Vector2(120, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "x" + NombreShuriken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        break;
                    case State.state_sabre:
                        spriteBatch.DrawString(ScreenManager.font, Langue.tr("Joueur") + " 2 ", new Vector2(25, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.DarkBlue);
                        spriteBatch.Draw(textureSabre, new Vector2(120, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(350 - (int)tempsCourir, Taille_Ecran.HAUTEUR_ECRAN - 55), new Rectangle(flamme.Width - (int)tempsCourir, 0, (int)tempsCourir, flamme.Height), Color.White);
            }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 PositionDesiree
        {
            get { return positionDesiree; }
        }

        public bool Regarde_Haut { get; set; }

        public bool Regarde_Bas { get; set; }

        public bool Regarde_Droite { get; set; }

        public bool Regarde_Gauche { get; set; }

        public void SauvegarderCheckPoint(ref StreamWriter file)
        {
            file.WriteLine(X.ToString() + "," + Y.ToString() + "," + NombreShuriken.ToString() + "," + NombreHadoken.ToString() + "," + NombreBall.ToString() + "," + NombreFumigene.ToString());
        }

        public void ChargerCheckPoint(ref StreamReader file)
        {
            string banana = "";
            string[] dessert = null;

            banana = file.ReadLine();
            dessert = banana.Split(',');

            X = Convert.ToInt32(dessert[0]);
            Y = Convert.ToInt32(dessert[1]);

            position = new Vector2(28 * X + 5, 28 * Y + 1);
            NombreShuriken = Convert.ToInt32(dessert[2]);
            NombreHadoken = Convert.ToInt32(dessert[3]);
            NombreBall = Convert.ToInt32(dessert[4]);
            NombreFumigene = Convert.ToInt32(dessert[5]);

            positionDesiree = new Vector2(28 * X + 5, 28 * Y + 1);

            Regarde_Bas = false;
            Regarde_Droite = false;
            Regarde_Haut = true;
            Regarde_Gauche = false;
        }

        public bool EstAuPointDeDepart
        {
            get
            {
                return X == caseDeDepart.X && Y == caseDeDepart.Y;
            }
        }

        private void AllerEnBas(Carte carte, Heros heros2, List<Interrupteur> interrupteurs)
        {
            if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && carte.Cases[Y + 1, X].EstFranchissable &&
                (heros2 == null ? true : (position.X != heros2.PositionDesiree.X || position.Y + 28 != heros2.PositionDesiree.Y)))
            {
                AudioEngine.SoundBank.PlayCue("pasBois");
                positionDesiree.X = position.X;
                positionDesiree.Y = position.Y + 28;
                descendre = false;
            }
            Regarde_Bas = true;
            Regarde_Droite = false;
            Regarde_Haut = false;
            Regarde_Gauche = false;
        }

        private void AllerEnHaut(Carte carte, Heros heros2, List<Interrupteur> interrupteurs)
        {
            if (position.Y > 5 && carte.Cases[Y - 1, X].EstFranchissable &&
                (heros2 == null ? true : (position.X != heros2.PositionDesiree.X || position.Y - 28 != heros2.PositionDesiree.Y)))
            {
                AudioEngine.SoundBank.PlayCue("pasBois");
                positionDesiree.X = position.X;
                positionDesiree.Y = position.Y - 28;
                monter = false;
            }
            Regarde_Bas = false;
            Regarde_Droite = false;
            Regarde_Haut = true;
            Regarde_Gauche = false;
        }

        private void AllerAGauche(Carte carte, Heros heros2, List<Interrupteur> interrupteurs)
        {
            if (position.X > 8 && carte.Cases[Y, X - 1].EstFranchissable &&
                (heros2 == null ? true : (position.X - 28 != heros2.PositionDesiree.X || position.Y != heros2.PositionDesiree.Y)))
            {
                AudioEngine.SoundBank.PlayCue("pasBois");
                positionDesiree.X = position.X - 28;
                positionDesiree.Y = position.Y;
                gauche = false;
            }
            Regarde_Bas = false;
            Regarde_Droite = false;
            Regarde_Haut = false;
            Regarde_Gauche = true;
        }

        private void AllerADroite(Carte carte, Heros heros2, List<Interrupteur> interrupteurs)
        {
            if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && carte.Cases[Y, X + 1].EstFranchissable &&
                (heros2 == null ? true : (position.X + 28 != heros2.PositionDesiree.X || position.Y != heros2.PositionDesiree.Y)))
            {
                AudioEngine.SoundBank.PlayCue("pasBois");
                positionDesiree.X = position.X + 28;
                positionDesiree.Y = position.Y;
                droite = false;
            }
            Regarde_Bas = false;
            Regarde_Droite = true;
            Regarde_Haut = false;
            Regarde_Gauche = false;
        }

        public bool AttaqueAuSabre(int x, int y)
        {
            return attaque && (int)positionDesiree.X / 28 == x && (int)positionDesiree.Y / 28 == y;
        }
    }
}