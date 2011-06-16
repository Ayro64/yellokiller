using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Ascenseur
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        int limite;
        float difference;
        bool enableMove = false;

        public Ascenseur(ContentManager content, int limite)
        {
            texture = content.Load<Texture2D>(@"Menu Editeur de Maps\ascenseur");
            this.limite = limite;
            position = new Vector2(limite, 0);
            rectangle.Width = texture.Width;
            rectangle.Height = texture.Height;
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void Update(int limite2)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            position.X = limite2;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y + texture.Height > Taille_Ecran.HAUTEUR_ECRAN - 84)
                position.Y = Taille_Ecran.HAUTEUR_ECRAN - texture.Height - 84;

            if (rectangle.Intersects(ServiceHelper.Get<IMouseService>().Rectangle()) && ServiceHelper.Get<IMouseService>().BoutonGaucheEnfonce())
            {
                enableMove = true;
                difference = ServiceHelper.Get<IMouseService>().Coordonnees().Y - position.Y;
            }
            else if (!ServiceHelper.Get<IMouseService>().BoutonGauchePresse())
                enableMove = false;

            if (enableMove)
            {
                if (ServiceHelper.Get<IMouseService>().Coordonnees().Y - difference <= 0)
                    position.Y = 0;
                else if (ServiceHelper.Get<IMouseService>().Coordonnees().Y + texture.Height - difference >=  Taille_Ecran.HAUTEUR_ECRAN - 84)
                    position.Y = Taille_Ecran.HAUTEUR_ECRAN - texture.Height - 84;
                else
                    position = new Vector2(position.X, ServiceHelper.Get<IMouseService>().Coordonnees().Y - difference);
            }

            if (ServiceHelper.Get<IMouseService>().Coordonnees().X > limite2 && ServiceHelper.Get<IMouseService>().Coordonnees().X < limite2 + 28 && ServiceHelper.Get<IMouseService>().MoletteATournee())
                position.Y -= ServiceHelper.Get<IMouseService>().Molette();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}