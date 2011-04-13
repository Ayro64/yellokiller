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
        Rectangle rectangle;
        Texture2D flamme;
        float vitesseAnimation, index, tempsCourir;
        int maxIndex, nombreShuriken = 25, nombreHadoken = 5, nombreFumigene = 10, nombre_ball = 5, vitesseSprite, numeroHero;
        public bool ishero;
        bool animation_sabre;
        bool monter, descendre, droite, gauche;
        bool regarde_droite, regarde_gauche, regarde_haut, regarde_bas;
        Keys up, down, right, left, changer_arme, courir, tirer;
        const int NumStates = 5;
        State currentState = State.state_shuriken;
        KeyboardState lastKeyboardState;
        int state_sabre = 0;

        public Hero(Vector2 position, Keys up, Keys down, Keys right, Keys left, Keys changer_arme, Keys tirer, Keys courir, int numeroHero)
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

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public int X
        {
            get { return (int)position.X / 28; }
        }

        public int Y
        {
            get { return (int)position.Y / 28; }
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
                base.LoadContent(content, "Hero1");
            else
                base.LoadContent(content, "Hero2");

            this.maxIndex = maxIndex;
            flamme = content.Load<Texture2D>("flamme");
            tempsCourir = flamme.Height;
        }


        private void HandleInput()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            bool keyboardSpace =
                currentKeyboardState.IsKeyUp(changer_arme) &&
                lastKeyboardState.IsKeyDown(changer_arme);

            if (keyboardSpace)
            {
                currentState = (State)((int)(currentState + 1) % NumStates);
            }

            lastKeyboardState = currentKeyboardState;
        }

        public void Update(GameTime gameTime, Carte carte, ref Rectangle camera, MoteurParticule particule, List<Shuriken> _shuriken, MoteurAudio moteurAudio, ContentManager content, Hero hero2)
        {
            this.Distance_Hero_Mur(carte);

            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

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

           // animation attaque au sabre
            if (currentState == State.state_sabre)
                state_sabre = 133;
            else
                state_sabre = 0;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && (currentState == State.state_sabre))
                animation_sabre = true;
            else
                animation_sabre = false;

            if (regarde_haut && animation_sabre)
            {
                Console.WriteLine(SourceRectangle);
                SourceRectangle = new Rectangle((int)index * 48, 0, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (Regarder_Bas && animation_sabre)
            {
                Console.WriteLine(SourceRectangle);
                SourceRectangle = new Rectangle((int)index * 48, 65, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (regarde_gauche && animation_sabre)
            {
                Console.WriteLine(SourceRectangle);
                SourceRectangle = new Rectangle((int)index * 48, 97, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }
            else if (regarde_droite && animation_sabre)
            {
                Console.WriteLine(SourceRectangle);
                SourceRectangle = new Rectangle((int)index * 48, 33, 16, 26);
                index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                if (index >= maxIndex)
                    index = 0f;
            }


            // animation moteur particule
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            HandleInput();

            switch (currentState)
            {
                case State.state_hadoken:
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && nombreHadoken > 0)
                    {
                        GameplayScreen.Enable_Timer = true; // je lance le timer                       
                        if (GameplayScreen.Timer == 0)
                        {
                            nombreHadoken--;
                            particule.UpdateExplosions(dt, this, carte, camera);
                            moteurAudio.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_ball:
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && nombre_ball > 0)
                    {
                        GameplayScreen.Enable_Timer = true; // je lance le timer                       
                        if (GameplayScreen.Timer == 0)
                        {
                            nombre_ball--;
                            particule.UpdateBall(dt, this, carte, camera);
                            moteurAudio.SoundBank.PlayCue("hadoken");
                        }
                    }
                    break;

                case State.state_fume:
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && nombreFumigene > 0)
                    {
                        nombreFumigene--;
                        particule.UpdateFumigene(dt, this, carte, camera);
                    }
                    break;

                case State.state_shuriken:
                    if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(tirer) && nombreShuriken > 0)
                    {
                        nombreShuriken--;
                        ishero = true;
                        _shuriken.Add(new Shuriken(position, this, content));
                        moteurAudio.SoundBank.PlayCue("shuriken");
                    }
                    else
                        ishero = false;
                    break;
            }

            if (tempsCourir > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) && !(monter && descendre && droite && gauche))
                tempsCourir -= 0.1f * gameTime.ElapsedGameTime.Milliseconds;
            else if (tempsCourir < flamme.Height && (ServiceHelper.Get<IKeyboardService>().ToucheRelevee(courir) || (monter && descendre && droite && gauche)))
                tempsCourir += 0.1f * gameTime.ElapsedGameTime.Milliseconds;

            if (!ServiceHelper.Get<IKeyboardService>().TouchePressee(up))    // arreter le sprite
            {
                if (SourceRectangle.Value.Y == 133 - state_sabre)
                { SourceRectangle = new Rectangle(24, 133 - state_sabre, 16, 26); }

                if (SourceRectangle.Value.Y == 198 - state_sabre)
                { SourceRectangle = new Rectangle(24, 198 - state_sabre, 16, 26); }

                if (SourceRectangle.Value.Y == 230 - state_sabre)
                { SourceRectangle = new Rectangle(24, 230 - state_sabre, 16, 26); }


                if (SourceRectangle.Value.Y == 166 - state_sabre)
                { SourceRectangle = new Rectangle(24, 166 - state_sabre, 16, 26); }
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
                if (ServiceHelper.Get<IKeyboardService>().TouchePressee(courir) && (int)tempsCourir != 0)
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
                        (int)carte.Cases[Y - 1, X].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }

                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(down) &&
                             (int)carte.Cases[Y + 1, X].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }

                    else if (position.X > 8 && ServiceHelper.Get<IKeyboardService>().TouchePressee(left) &&
                             (int)carte.Cases[Y, X - 1].Type > 0)
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }

                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(right) &&
                             (int)carte.Cases[Y, X + 1].Type > 0)
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
                        (int)carte.Cases[Y - 1, X].Type > 0 &&
                        (position.X != hero2.PositionDesiree.X || position.Y - 28 != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                    }

                    else if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && ServiceHelper.Get<IKeyboardService>().TouchePressee(down) &&
                              (int)carte.Cases[Y + 1, X].Type > 0 &&
                              (position.X != hero2.PositionDesiree.X || position.Y + 28 != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X;
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                    }

                    else if (position.X > 8 && ServiceHelper.Get<IKeyboardService>().TouchePressee(left) &&
                             (int)carte.Cases[Y, X - 1].Type > 0 &&
                             (position.X - 28 != hero2.PositionDesiree.X || position.Y != hero2.PositionDesiree.Y))
                    {
                        moteurAudio.SoundBank.PlayCue("pasBois");
                        positionDesiree.X = position.X - 28;
                        positionDesiree.Y = position.Y;
                        gauche = false;
                    }

                    else if (position.X < 28 * Taille_Map.LARGEUR_MAP - 23 && ServiceHelper.Get<IKeyboardService>().TouchePressee(right) &&
                             (int)carte.Cases[Y, X + 1].Type > 0 &&
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

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            if (numeroHero == 1)
            {
                switch (currentState)
                {
                    case State.state_hadoken:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreHadoken.ToString() + " hadoken au joueur 1.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreFumigene.ToString() + " fumigènes au joueur 1.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreShuriken.ToString() + " shurikens au joueur 1.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 50), Color.DarkBlue);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 50, Taille_Ecran.HAUTEUR_ECRAN - 25 - (int)tempsCourir), new Rectangle(0, flamme.Height - (int)tempsCourir, flamme.Width, (int)tempsCourir), Color.White);
            }
            else
            {
                switch (currentState)
                {
                    case State.state_hadoken:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreHadoken.ToString() + " hadoken au joueur 2.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                    case State.state_fume:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreFumigene.ToString() + " fumigènes au joueur 2.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                    case State.state_shuriken:
                        spriteBatch.DrawString(ScreenManager.font, "Il reste " + nombreShuriken.ToString() + " shurikens au joueur 2.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 75), Color.DarkBlue);
                        break;
                }

                spriteBatch.Draw(flamme, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 100, Taille_Ecran.HAUTEUR_ECRAN - 25 - (int)tempsCourir), new Rectangle(0, flamme.Height - (int)tempsCourir, flamme.Width, (int)tempsCourir), Color.White);
            }
            base.Draw(spriteBatch, camera);
        }
    }
}