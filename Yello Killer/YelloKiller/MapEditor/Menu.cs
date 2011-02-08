using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            listeTextures.Add(content.Load<Texture2D>("arbre2"));

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
            for (int i = 0; i < nbTextures; i++)
                spriteBatch.Draw(listeTextures[i], new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + i * 80), null, Color.White, 0, Vector2.Zero, (float)28 / listeTextures[i].Height, SpriteEffects.None, 0);
            
            for (int u = 0; u < listeRectangles.Count; u++)
                if (ServiceHelper.Get<IMouseService>().Rectangle().Intersects(listeRectangles[u]))
                {
                    spriteBatch.Draw(fond, new Vector2(listeRectangles[u].X - 2, listeRectangles[u].Y - 2), Color.White);
                    spriteBatch.Draw(listeTextures[u], new Vector2(listeRectangles[u].X + 28 * (1 - listeTextures[u].Width / 28), listeRectangles[u].Y + 28 * (1 - listeTextures[u].Height / 28)), Color.White);
                }
        }
    }
}