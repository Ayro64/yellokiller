using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Yellokiller
{
    class Ascenseur
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        float difference;
        bool enableMove = false;


        public Ascenseur(ContentManager content)
        {
            texture = content.Load<Texture2D>("ascenseur");
            position = new Vector2(Taille_Ecran.LARGEUR_ECRAN - texture.Width, 0);
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void Update(Souris souris)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            if (rectangle.Intersects(souris.Rectangle) && souris.MState.LeftButton == ButtonState.Pressed && souris.LastMState.LeftButton == ButtonState.Released)
            {
                enableMove = true;
                difference = souris.MState.Y - position.Y;
            }
            else if (souris.MState.LeftButton == ButtonState.Released)
                enableMove = false;

            if (enableMove)
            {
                if (souris.MState.Y - difference <= 0)
                    position.Y = 0;
                else if(souris.MState.Y + texture.Height - difference >= Taille_Ecran.HAUTEUR_ECRAN)
                    position.Y = Taille_Ecran.HAUTEUR_ECRAN - texture.Height;
                else
                    position = new Vector2(position.X, souris.MState.Y - difference);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
