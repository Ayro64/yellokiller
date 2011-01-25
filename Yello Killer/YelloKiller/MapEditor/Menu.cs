using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class Menu
    {
        List<Rectangle> listeRectangles = new List<Rectangle>();
        Texture2D arbre, maison, mur, herbeFoncee, origine1, origine2, ennemi, fond, herbe;
        public int nbTextures;

        public Menu(ContentManager content, int nbTextures)
        {
            this.nbTextures = nbTextures;
            for (int i = 0; i < nbTextures; i++)
                listeRectangles.Add(new Rectangle(0, 0, 28, 28));

            herbe = content.Load<Texture2D>("herbe");
            arbre = content.Load<Texture2D>("arbre");
            maison = content.Load<Texture2D>("maison");
            mur = content.Load<Texture2D>("mur");
            herbeFoncee = content.Load<Texture2D>("herbeFoncee");
            ennemi = content.Load<Texture2D>("origineEnnemi1");
            origine1 = content.Load<Texture2D>("origine1");
            origine2 = content.Load<Texture2D>("origine2");            
            fond = content.Load<Texture2D>("fond");
        }

        public List<Rectangle> ListesRectangles
        {
            get { return listeRectangles; }
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

            spriteBatch.Draw(herbe, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y), Color.White);
            spriteBatch.Draw(herbeFoncee, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 80), Color.White);
            spriteBatch.Draw(mur, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 160), Color.White);
            spriteBatch.Draw(maison, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 240), Color.White);
            spriteBatch.Draw(arbre, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 320), Color.White);
            spriteBatch.Draw(ennemi, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 400), Color.White);
            spriteBatch.Draw(origine1, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 480), Color.White);
            spriteBatch.Draw(origine2, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseur.Position.Y + 560), Color.White);
        }
    }
}