using Microsoft.Xna.Framework;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Statue : Sprite
    {
        byte direction;
        double timer = 0;

        public Statue(Vector2 position, Carte carte, byte direction)
            :base(position)
            //: base(position, carte)
        {
            this.position = position;
            this.direction = direction;

            if (direction == 0) // bas
                SourceRectangle = new Rectangle(0, 0, 112, 94);
            else if (direction == 1) // gauche
                SourceRectangle = new Rectangle(0, 123, 112, 94);
            else if (direction == 2) // haut
                SourceRectangle = new Rectangle(0, 357, 112, 94);
            else if (direction == 3) // droite
                SourceRectangle = new Rectangle(0, 243, 112, 94);

            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 112, 94);
        }

        public int Distance_Statue_Mur(Carte carte)
        {
            int distance = 0;

            if (direction == 2)
                for (int i = 0; this.Y - i > 0 && carte.Cases[this.Y - i, this.X].EstFranchissable; i++)
                    distance++;
            else if (direction == 3)
                for (int i = 0; this.X + i < Taille_Map.LARGEUR_MAP && carte.Cases[this.Y, this.X + i].EstFranchissable; i++)
                    distance++;

            else if (direction == 1)
                for (int i = 0; this.X - i > 0 && carte.Cases[this.Y, this.X - i].EstFranchissable; i++)
                    distance++;

            else if (direction == 0)
                for (int i = 0; this.Y + i < Taille_Map.HAUTEUR_MAP && carte.Cases[this.Y + i, this.X].EstFranchissable; i++)
                    distance++;

            if (distance < 8)
                return distance;
            else
                return 8;
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, @"Feuilles de sprites\statue_dragon");
        }

        public void Update(GameTime gameTime, MoteurParticule particule)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 2)
            {
                AudioEngine.SoundBank.PlayCue("Bruitage des statues");
                particule.UpdateExplosions_statue(this);
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Rectangle camera)
        {
            base.Draw(spriteBatch, camera);
        }

        public Rectangle Rectangle { get; set; }
    }
}
