using System;
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

    class Hero : Sprite
    {
        Vector2 positionDesiree;
        Texture2D flamme, textureShuriken, textureHadoken, textureBouleDeFeu, textureFumigene;
        Rectangle rectangle;
        float vitesseAnimation, index, tempsCourir;
        int maxIndex, nombreShuriken = 25, nombreHadoken = 5, nombreFumigene = 10, nombre_ball = 5, vitesseSprite;
        private byte numeroHero;
        public byte NumeroHero
        { get { return numeroHero; } }
        public bool ishero;
        bool animation_sabre;
        bool monter, descendre, droite, gauche;
        bool regarde_droite, regarde_gauche, regarde_haut, regarde_bas;
        Keys up, down, right, left, changer_arme, courir, tirer;
        const int NumStates = 4;
        State currentState = State.state_shuriken;
        int state_sabre = 0;

        public Hero(Vector2 position, Keys up, Keys down, Keys right, Keys left, Keys changer_arme, Keys tirer, Keys courir, byte numeroHero)
            : base(position)
        {
            this.position = position;
            positionDesiree = position;
            this.numeroHero = numeroHero;
            SourceRectangle = new Rectangle(25, 133 - state_sabre, 16, 26);
            monter = true;
            descendre = true;
            droite = true;
            gauche = true;
            animation_sabre = false;
            regarde_droite = true;
            regarde_gauche = true;
            regarde_haut = true;
            regarde_bas = true;
            vitesseSprite = 4;
            vitesseAnimation = 0.008f;
            index = 0;
            maxIndex = 0;
            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 26);
            ishero = false;
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
            this.changer_arme = changer_arme;
            this.tirer = tirer;
            this.courir = courir;
        }

        public int Distance_Hero_Mur(Carte carte)
        {
            int distance = 0;
            if (Regarder_Haut)
                for (int i = 0; this.Y - i > 0 && carte.Cases[this.Y - i, this.X].Type > 0; i++)
                    distance++;
            else if (Regarder_Droite)
                for (int i = 0; this.X + i < Taille_Map.LARGEUR_MAP && carte.Cases[this.Y, this.X + i].Type > 0; i++)
                    distance++;
            else if (Regarder_Gauche)
                for (int i = 0; this.X - i > 0 && carte.Cases[this.Y, this.X - i].Type > 0; i++)
                    distance++;
            else if (Regarder_Bas)
                for (int i = 0; this.Y + i < Taille_Map.HAUTEUR_MAP && carte.Cases[this.Y + i, this.X].Type > 0; i++)
                    distance++;

            if (distance < 6)
                return distance;
            else
                return 6;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            if (numeroHero == 1)
                base.LoadContent(content, @"Feuilles de sprites\Hero1");
            else
                base.LoadContent(content, @"Feuilles de sprites\Hero2");

            flamme = content.Load<Texture2D>("barre de vitesse");
            textureShuriken = content.Load<Texture2D>(@"Barre infos\shuriken barre");
            textureHadoken = content.Load<Texture2D>(@"Barre infos\hadoken barre");
            textureBouleDeFeu = content.Load<Texture2D>(@"Barre infos\boule de feu barre");
            textureFumigene = content.Load<Texture2D>(@"Barre infos\fumigene barre");

            tempsCourir = flamme.Width;

            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, ref Rectangle camera, MoteurParticule particule, List<Shuriken> _shuriken, ContentManager content, Hero hero2)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            MoteurParticule.Camera = new Vector2(camera.X, camera.Y);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer))
                GameplayScreen.Timer_Update_Collision += gameTime.ElapsedGameTime.TotalSeconds;

            if (SourceRectangle.Value.Y == 133 - state_sabre)
                regarde_haut = true;
            else
                regarde_haut = false;

            if (SourceRectangle.Value.Y == 198 - state_sabre)
                regarde_bas = true;
            else
                regarde_bas = false;

            if (SourceRectangle.Value.Y == 230 - state_sabre)
                regarde_gauche = true;
            else
                regarde_gauche = false;

            if (SourceRectangle.Value.Y == 166 - state_sabre)
                regarde_droite = true;
            else
                regarde_droite = false;

            //armes
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(changer_arme) || ServiceHelper.Get<IGamePadService>().ChangerArme())
                currentState = (State)((int)(currentState + 1) % NumStates);

            // animation attaque au sabre
            if (currentState == State.state_sabre)
                state_sabre = 133;
            else
                state_sabre = 0;

            if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && (currentState == State.state_sabre))
                animation_sabre = true;
            else
                animation_sabre = false;

            if (regarde_haut && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 0, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (Regarder_Bas && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 65, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (regarde_gauche && animation_sabre)
            {
                SourceRectangle = new Rectangle((int)index * 48, 97, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (regarde_droite && animation_sabre)
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
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombreHadoken > 0 && numeroHero == 1)
                    {
                        Distance_Hero_Mur(carte);
                        GameplayScreen.Enable_Timer_Hero1 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero1 == 0)
                        {
                            nombreHadoken--;
                            particule.UpdateExplosions_hero(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombreHadoken > 0 && numeroHero == 2)
                    {
                        Distance_Hero_Mur(carte);
                        GameplayScreen.Enable_Timer_Hero2 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero2 == 0)
                        {
                            nombreHadoken--;
                            particule.UpdateExplosions_hero(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_ball:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombre_ball > 0 && numeroHero == 1)
                    {
                        Distance_Hero_Mur(carte);
                        GameplayScreen.Enable_Timer_Hero1 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero1 == 0)
                        {
                            nombre_ball--;
                            particule.UpdateBall(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombre_ball > 0 && numeroHero == 2)
                    {
                        Distance_Hero_Mur(carte);
                        GameplayScreen.Enable_Timer_Hero2 = true; // je lance le timer                       
                        if (GameplayScreen.Timer_Hero2 == 0)
                        {
                            nombre_ball--;
                            particule.UpdateBall(this);
                            AudioEngine.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_fume:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombreFumigene > 0)
                    {
                        nombreFumigene--;
                        particule.UpdateFumigene(this);
                    }
                    break;

                case State.state_shuriken:
                    if ((ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) || ServiceHelper.Get<IGamePadService>().Tirer()) && nombreShuriken > 0)
                    {
                        nombreShuriken--;
                        ishero = true;
                        _shuriken.Add(new Shuriken(position, this, content));
                        AudioEngine.SoundBank.PlayCue("shuriken");
                    }
                    else
                        ishero = false;
                    break;
            }

            if (tempsCourir > 0 && (ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) || ServiceHelper.Get<IGamePadService>().Courir()) && !(monter && descendre && droite && gauche))
                tempsCourir -= 0.1f * gameTime.ElapsedGameTime.Milliseconds;
            else if (tempsCourir < flamme.Width && ((ServiceHelper.Get<IKeyboardService>().ToucheRelevee(courir) || ServiceHelper.Get<IGamePadService>().Courir()) || (monter && descendre && droite && gauche)))
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

                    if (numeroHero == 1 && camera.Y - vitesseSprite >= 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP - 308)
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

                    if (numeroHero == 1 && camera.Y + vitesseSprite <= 28 * (Taille_Map.HAUTEUR_MAP - camera.Height) && position.Y > 310)
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

                    if (numeroHero == 1 && camera.X - vitesseSprite >= 0 && position.X < 28 * Taille_Map.LARGEUR_MAP - 304)
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

                    if (numeroHero == 1 && camera.X + vitesseSprite <= 28 * (Taille_Map.LARGEUR_MAP - camera.Width) && position.X > 314)
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
                if ((ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) || ServiceHelper.Get<IGamePadService>().Courir()) && (int)tempsCourir != 0)
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
                    if ((position.Y > 5 && ServiceHelper.Get<IKeyboardService>().TouchePressee(up) || ServiceHelper.Get<IGamePadService>().AllerEnHaut()) &&
                        (int)carte.Cases[Y - 1, X].Type > 0)
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }

                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && (ServiceHelper.Get<IKeyboardService>().TouchePressee(down) || ServiceHelper.Get<IGamePadService>().AllerEnBas()) &&
                             (int)carte.Cases[Y + 1, X].Type > 0)
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }

                    else if (position.X > 8 && (ServiceHelper.Get<IKeyboardService>().TouchePressee(left) || ServiceHelper.Get<IGamePadService>().AllerAGauche()) &&
                             (int)carte.Cases[Y, X - 1].Type > 0)
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }

                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && (ServiceHelper.Get<IKeyboardService>().TouchePressee(right) || ServiceHelper.Get<IGamePadService>().AllerADroite()) &&
                             (int)carte.Cases[Y, X + 1].Type > 0)
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X + 28;
                        positionDesiree.Y = position.Y;
                        droite = false;
                    }
                }
                else
                {
                    if (position.Y > 5 && ServiceHelper.Get<IKeyboardService>().TouchePressee(up) &&
                        (int)carte.Cases[Y - 1, X].Type > 0 &&
                        (position.X != hero2.PositionDesiree.X || position.Y - 28 != hero2.PositionDesiree.Y))
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }

                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(down) &&
                              (int)carte.Cases[Y + 1, X].Type > 0 &&
                              (position.X != hero2.PositionDesiree.X || position.Y + 28 != hero2.PositionDesiree.Y))
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }

                    else if (position.X > 8 && ServiceHelper.Get<IKeyboardService>().TouchePressee(left) &&
                             (int)carte.Cases[Y, X - 1].Type > 0 &&
                             (position.X - 28 != hero2.PositionDesiree.X || position.Y != hero2.PositionDesiree.Y))
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }

                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(right) &&
                             (int)carte.Cases[Y, X + 1].Type > 0 &&
                             (position.X + 28 != hero2.PositionDesiree.X || position.Y != hero2.PositionDesiree.Y))
                    {
                        AudioEngine.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X + 28;
                        positionDesiree.Y = position.Y;
                        droite = false;
                    }
                }
            }
        }

        public void Draw_Hero(SpriteBatch spriteBatch, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
        }

        public void Draw_Infos(SpriteBatch spriteBatch)
        {
            if (numeroHero == 1)
            {
                switch (currentState)
                {
                    case State.state_hadoken:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 1 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        spriteBatch.Draw(textureHadoken, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreHadoken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                    case State.state_ball:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 1 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        spriteBatch.Draw(textureBouleDeFeu, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombre_ball.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 1 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        spriteBatch.Draw(textureFumigene, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 55), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreFumigene.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 1 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        spriteBatch.Draw(textureShuriken, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreShuriken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 250 - (int)tempsCourir, Taille_Ecran.HAUTEUR_ECRAN - 75), new Rectangle(flamme.Width - (int)tempsCourir, 0, (int)tempsCourir, flamme.Height), Color.White);
            }
            else
            {
                switch (currentState)
                {
                    case State.state_hadoken:

                        spriteBatch.DrawString(ScreenManager.font, "Joueur 2 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        spriteBatch.Draw(textureHadoken, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 80), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreHadoken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                    case State.state_ball:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 2 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        spriteBatch.Draw(textureBouleDeFeu, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 80), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombre_ball.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 2 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        spriteBatch.Draw(textureFumigene, new Vector2(110, Taille_Ecran.HAUTEUR_ECRAN - 80), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreFumigene.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, "Joueur 2 ", new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        spriteBatch.Draw(textureShuriken, new Vector2(115, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.White);
                        spriteBatch.DrawString(ScreenManager.font, "*" + nombreShuriken.ToString(), new Vector2(145, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 250 - (int)tempsCourir, Taille_Ecran.HAUTEUR_ECRAN - 35), new Rectangle(flamme.Width - (int)tempsCourir, 0, (int)tempsCourir, flamme.Height), Color.White);
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

        public bool Regarder_Haut
        {
            get { return regarde_haut; }
            set { regarde_haut = value; }
        }

        public bool Regarder_Bas
        {
            get { return regarde_bas; }
            set { regarde_bas = value; }
        }

        public bool Regarder_Droite
        {
            get { return regarde_droite; }
            set { regarde_droite = value; }
        }

        public bool Regarder_Gauche
        {
            get { return regarde_gauche; }
            set { regarde_gauche = value; }
        }

        public int NombreShurikens
        {
            get { return nombreShuriken; }
            set { nombreShuriken = value; }
        }

        public int NombreHadokens
        {
            get { return nombreHadoken; }
            set { nombreHadoken = value; }
        }
    }
}