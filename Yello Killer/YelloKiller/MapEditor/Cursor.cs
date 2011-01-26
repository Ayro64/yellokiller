using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Yellokiller
{
    class Cursor
    {
        Texture2D texture, fond;
        Vector2 position;
        TypeCase type;

        public Cursor(ContentManager content)
        {
            position = new Vector2(0, 0);
            LoadContent(content, "herbefoncee");
            fond = content.Load<Texture2D>("fond");
            type = TypeCase.herbeFoncee;
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

        private void LoadContent(ContentManager Content, string nom)
        {
            texture = Content.Load<Texture2D>(nom);
        }

        public void Update(ContentManager content, Menu menu)
        {
            if (ServiceHelper.Get<IMouseService>().DansLEcran())
                position = new Vector2((int)ServiceHelper.Get<IMouseService>().Coordonnees().X / 28, (int)ServiceHelper.Get<IMouseService>().Coordonnees().Y / 28);

            for (int i = 0; i < menu.nbTextures; i++)
            {
                if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(menu.ListesRectangles[i]))
                {
                    switch (i)
                    {
                        case (0):
                            type = TypeCase.herbe;
                            LoadContent(content, "herbe");
                            break;
                        case (1):
                            type = TypeCase.herbeFoncee;
                            LoadContent(content, "herbeFoncee");
                            break;
                        case (2):
                            type = TypeCase.mur;
                            LoadContent(content, "mur");
                            break;
                        case (3):
                            type = TypeCase.maison;
                            LoadContent(content, "maison");
                            break;
                        case (4):
                            type = TypeCase.arbre;
                            LoadContent(content, "arbre");
                            break;
                        case (5):
                            type = TypeCase.Ennemi;
                            LoadContent(content, "origineEnnemi1");
                            break;
                        case (6):
                            type = TypeCase.Joueur1;
                            LoadContent(content, "origine1");
                            break;
                        case (7):
                            type = TypeCase.Joueur2;
                            LoadContent(content, "origine2");
                            break;                       
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fond, new Vector2(position.X * texture.Width - 2, position.Y * texture.Height - 2), Color.White);
            spriteBatch.Draw(texture, new Vector2(position.X * texture.Width, position.Y * texture.Height), Color.White);
        }
    }
}
