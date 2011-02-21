using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Cette classe gere le curseur de selection des textures dans l'editeur de map. */

namespace YelloKiller
{
    class Cursor
    {
        Texture2D texture, fond;
        Vector2 position;
        TypeCase type;
        float tailleFond;

        public Cursor(ContentManager content)
        {
            position = new Vector2(0, 0);
            texture = content.Load<Texture2D>("herbefoncee");
            fond = content.Load<Texture2D>("fond");
            type = TypeCase.herbeFoncee;
            tailleFond = 1;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public TypeCase Type
        {
            get { return type; }
        }

        public void Update(ContentManager content, Menu menu)
        {
            if (ServiceHelper.Get<IMouseService>().DansLEcran())
                position = new Vector2((int)ServiceHelper.Get<IMouseService>().Coordonnees().X / 28, (int)ServiceHelper.Get<IMouseService>().Coordonnees().Y / 28);

            for (int i = 0; i < menu.nbTextures; i++)
            {
                if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(menu.ListeRectangles[i]))
                {
                    texture = menu.ListeTextures[i];
                    switch (i)
                    {
                        case (0):
                            type = TypeCase.herbe;
                            break;
                        case (1):
                            type = TypeCase.herbeFoncee;
                            break;
                        case (2):
                            type = TypeCase.mur;
                            break;
                        case (3):
                            type = TypeCase.maison;
                            break;
                        case (4):
                            type = TypeCase.arbre;
                            break;
                        case (5):
                            type = TypeCase.Ennemi;
                            break;
                        case (6):
                            type = TypeCase.Joueur1;
                            break;
                        case (7):
                            type = TypeCase.Joueur2;
                            break;
                        case (8):
                            type = TypeCase.arbre2;
                            break;
                    }

                    if (i >= 0 && i <= 7)
                        tailleFond = 1;
                    else
                        tailleFond = 1.88f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fond, new Vector2(position.X * 28 - 2, position.Y * 28 - 2), null, Color.White, 0, Vector2.Zero, tailleFond, SpriteEffects.None, 0);            
            spriteBatch.Draw(texture, new Vector2(position.X * 28, position.Y * 28), Color.White);
        }
    }
}
