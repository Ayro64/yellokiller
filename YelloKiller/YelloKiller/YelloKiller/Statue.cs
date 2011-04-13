using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using YelloKiller.Moteur_Particule;

namespace YelloKiller.YelloKiller
{
    class Statue : Ennemi
    {

        Rectangle rectangle;
        bool regarde_droite, regarde_gauche, regarde_haut, regarde_bas;
        int distance;
        State currentState = State.state_shuriken;

        

        public Statue(Vector2 position)
            : base(position)
        {
            this.position = position;
            SourceRectangle = new Rectangle(0, 0, 120, 120);
            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 120, 120);
         // distance = this.Distance_Statue_Mur(Carte);
        }

        public int Distance_Statue_Mur(Carte carte)
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

        public void LoadContent(ContentManager content)
        { base.LoadContent(content, "statue_dragon"); }



        public void Update(GameTime gameTime, Carte carte, ref Rectangle camera, MoteurParticule particule, MoteurAudio moteurAudio, ContentManager content)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            if (SourceRectangle.Value.Y == 168)
                regarde_haut = true;
            else
                regarde_haut = false;

            if (SourceRectangle.Value.Y == 0)
                regarde_bas = true;
            else
                regarde_bas = false;

            if (SourceRectangle.Value.Y == 56)
                regarde_gauche = true;
            else
                regarde_gauche = false;

            if (SourceRectangle.Value.Y == 112)
                regarde_droite = true;
            else
                regarde_droite = false;


            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (currentState)
            {
                case State.state_hadoken:
                    particule.UpdateExplosions(dt, this, carte, camera);
                    moteurAudio.SoundBank.PlayCue("hadoken");
                    break;

                case State.state_ball:
                    particule.UpdateBall(dt, this, carte, camera);
                    moteurAudio.SoundBank.PlayCue("hadoken");
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
        }
    }
}