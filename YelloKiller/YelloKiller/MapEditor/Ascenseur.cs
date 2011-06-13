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
        float limite;

        float difference;
        bool enableMove = false;

        public Ascenseur(ContentManager content, float limite)
        {
            texture = content.Load<Texture2D>(@"Menu Editeur de Maps\ascenseur");
            position = new Vector2(limite, 0);
            this.limite = limite;
            rectangle.Width = texture.Width;
            rectangle.Height = texture.Height;
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void Update(int limite)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y + texture.Height > Taille_Ecran.HAUTEUR_ECRAN)
                position.Y = Taille_Ecran.HAUTEUR_ECRAN - texture.Height;

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
                else if (ServiceHelper.Get<IMouseService>().Coordonnees().Y + texture.Height - difference >= limite/* Taille_Ecran.HAUTEUR_ECRAN*/)
                    position.Y = limite /*Taille_Ecran.HAUTEUR_ECRAN */- texture.Height;
                else
                    position = new Vector2(position.X, ServiceHelper.Get<IMouseService>().Coordonnees().Y - difference);
            }

            if (ServiceHelper.Get<IMouseService>().Coordonnees().X > limite && ServiceHelper.Get<IMouseService>().Coordonnees().X < limite + 28 && ServiceHelper.Get<IMouseService>().MoletteATournee())
                position.Y -= ServiceHelper.Get<IMouseService>().Molette();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}