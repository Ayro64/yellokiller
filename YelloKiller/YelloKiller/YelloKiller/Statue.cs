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
        byte direction;

        float timer = 0;

        public Statue(Vector2 position, Carte carte, byte direction)
            : base(position, carte)
        {
            this.position = position;
            this.direction = direction;

            if (direction == 0) // bas
                SourceRectangle = new Rectangle(0, 0, 95, 90);
            if (direction == 1) // gauche
                SourceRectangle = new Rectangle(0, 123, 95, 90);
            if (direction == 2) // droite
                SourceRectangle = new Rectangle(0, 243, 95, 90);
            if (direction == 3) // haut
                SourceRectangle = new Rectangle(0, 357, 95, 90);


            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 95, 90);
            // distance = this.Distance_Statue_Mur(Carte);
        }

        public int Distance_Statue_Mur(Carte carte)
        {
            int distance = 0;

            if (direction == 3)
                for (int i = 0; this.Y - i > 0 && carte.Cases[this.Y - i, this.X].Type > 0; i++)
                    distance++;

            else if (direction == 2)
                for (int i = 0; this.X + i < Taille_Map.LARGEUR_MAP && carte.Cases[this.Y, this.X + i].Type > 0; i++)
                    distance++;

            else if (direction == 1)
                for (int i = 0; this.X - i > 0 && carte.Cases[this.Y, this.X - i].Type > 0; i++)
                    distance++;

            else if (direction == 0)
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


            timer += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 5)
            {
                particule.UpdateExplosions_statue(dt, this, carte, camera);
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
        }
    }
}
