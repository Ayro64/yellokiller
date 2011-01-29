using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class Menu
    {
        List<Rectangle> listeRectangles = new List<Rectangle>();
        List<Texture2D> listeTextures = new List<Texture2D>();
        Texture2D fond;

        public int nbTextures;

        public Menu(ContentManager content, int nbTextures)
        {
            this.nbTextures = nbTextures;
            for (int i = 0; i < nbTextures; i++)
                listeRectangles.Add(new Rectangle(0, 0, 28, 28));

            listeTextures.Add(content.Load<Texture2D>("herbe"));
            listeTextures.Add(content.Load<Texture2D>("herbeFoncee"));
            listeTextures.Add(content.Load<Texture2D>("mur"));
            listeTextures.Add(content.Load<Texture2D>("maison"));
            listeTextures.Add(content.Load<Texture2D>("arbre"));
            listeTextures.Add(content.Load<Texture2D>("origineEnnemi1"));
            listeTextures.Add(content.Load<Texture2D>("origine1"));
            listeTextures.Add(content.Load<Texture2D>("origine2"));

            fond = content.Load<Texture2D>("fond");
        }

        public List<Rectangle> ListeRectangles
        {
            get { return listeRectangles; }
        }

        public List<Texture2D> ListeTextures
        {
            get { return listeTextures; }
        }

        public void Update(Ascenseur ascenseur)
        {
            for (int i = 0; i < nbTextures; i++)
                listeRectangles[i] = new Rectangle(Taille_Ecran.LARGEUR_ECRAN - 56, (int)-ascenseur.Position.Y + i * 80, 28, 28);
        }

        public void Draw(SpriteBatch spriteBatch, Ascenseur ascenseur)
        {
            foreach (Rectangle rect in listeRectangles)
                if (ServiceHelper.Get<IMouseService>().Rectangle().Intersects(rect))
                    spriteBatch.Draw(fond, new Vector2(rect.X - 2, rect.Y - 2), Color.White);

            for (int i = 0; i < nbTextures; i++)
                spriteBatch.Draw(listeTextures[i], new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + i * 80), Color.White);
        }
    }
}