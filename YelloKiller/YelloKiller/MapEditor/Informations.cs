using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace YelloKiller
{
    class Informations
    {
        Texture2D shuriken, hadoken, fumigene, traineeDeFlamme, plus, moins, heros1, heros2;
        Rectangle[] rectangles;
        int[] munitions;
        int limite;

        public int[] Munitions { get { return munitions; } }
        public int Salaire { get; private set; }

        public Informations(int limite)
        {
            this.limite = limite;
            rectangles = new Rectangle[18];

            rectangles[0] = new Rectangle(50, limite - 40, 20, 20);
            rectangles[1] = new Rectangle(160, limite - 40, 20, 20);

            rectangles[2] = new Rectangle(250, limite - 60, 20, 20);
            rectangles[3] = new Rectangle(320, limite - 60, 20, 20);
            rectangles[4] = new Rectangle(420, limite - 60, 20, 20);
            rectangles[5] = new Rectangle(490, limite - 60, 20, 20);
            rectangles[6] = new Rectangle(590, limite - 60, 20, 20);
            rectangles[7] = new Rectangle(660, limite - 60, 20, 20);
            rectangles[8] = new Rectangle(760, limite - 60, 20, 20);
            rectangles[9] = new Rectangle(830, limite - 60, 20, 20);

            rectangles[10] = new Rectangle(250, limite - 30, 20, 20);
            rectangles[11] = new Rectangle(320, limite - 30, 20, 20);
            rectangles[12] = new Rectangle(420, limite - 30, 20, 20);
            rectangles[13] = new Rectangle(490, limite - 30, 20, 20);
            rectangles[14] = new Rectangle(590, limite - 30, 20, 20);
            rectangles[15] = new Rectangle(660, limite - 30, 20, 20);
            rectangles[16] = new Rectangle(760, limite - 30, 20, 20);
            rectangles[17] = new Rectangle(830, limite - 30, 20, 20);

            munitions = new int[8];
            munitions[0] = 10;
            munitions[4] = 10;
            munitions[1] = 3;
            munitions[5] = 3;
            munitions[2] = 5;
            munitions[6] = 5;
            munitions[3] = 3;
            munitions[7] = 3;
            Salaire = 200000;
        }

        public void Update()
        {
            if (Salaire > 0 && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(rectangles[0]) && ServiceHelper.Get<IMouseService>().ClicBoutonGauche())
                Salaire -= 1000;

            if (Salaire < 1000000 && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(rectangles[1]) && ServiceHelper.Get<IMouseService>().ClicBoutonGauche())
                Salaire += 1000;

            for (int i = 2; i <= 9; i++)
                if (munitions[i - 2] > 0 && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(rectangles[2 * (i - 1)]) && ServiceHelper.Get<IMouseService>().ClicBoutonGauche())
                    munitions[i - 2]--;

            for (int i = 2; i <= 9; i++)
                if (munitions[i - 2] < 100 && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(rectangles[2 * i - 1]) && ServiceHelper.Get<IMouseService>().ClicBoutonGauche())
                    munitions[i - 2]++;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, bool hero1Existe, bool hero2Existe)
        {
            spriteBatch.Draw(moins, rectangles[0], Color.White);
            spriteBatch.DrawString(font, "SALAIRE", new Vector2(75, limite - 70), Color.Red);
            spriteBatch.DrawString(font, Salaire.ToString(), new Vector2(80, limite - 40), Color.Red);
            spriteBatch.Draw(plus, rectangles[1], Color.White);

            spriteBatch.Draw(shuriken, new Vector2(280, limite - 82), Color.White);
            spriteBatch.Draw(hadoken, new Vector2(450, limite - 85), Color.White);
            spriteBatch.Draw(fumigene, new Vector2(620, limite - 85), Color.White);
            spriteBatch.Draw(traineeDeFlamme, new Vector2(790, limite - 85), Color.White);

            if (hero1Existe)
            {
                spriteBatch.Draw(heros1, new Vector2(210, limite - 65), Color.White);

                spriteBatch.Draw(moins, rectangles[2], Color.White);
                spriteBatch.DrawString(font, munitions[0].ToString(), new Vector2(280, limite - 60), Color.Red);
                spriteBatch.Draw(plus, rectangles[3], Color.White);

                spriteBatch.Draw(moins, rectangles[4], Color.White);
                spriteBatch.DrawString(font, munitions[1].ToString(), new Vector2(450, limite - 60), Color.Red);
                spriteBatch.Draw(plus, rectangles[5], Color.White);

                spriteBatch.Draw(moins, rectangles[6], Color.White);
                spriteBatch.DrawString(font, munitions[2].ToString(), new Vector2(620, limite - 60), Color.Red);
                spriteBatch.Draw(plus, rectangles[7], Color.White);

                spriteBatch.Draw(moins, rectangles[8], Color.White);
                spriteBatch.DrawString(font, munitions[3].ToString(), new Vector2(790, limite - 60), Color.Red);
                spriteBatch.Draw(plus, rectangles[9], Color.White);
            }

            if (hero2Existe)
            {
                spriteBatch.Draw(heros2, new Vector2(210, limite - 35), Color.White);

                spriteBatch.Draw(moins, rectangles[10], Color.White);
                spriteBatch.DrawString(font, munitions[4].ToString(), new Vector2(280, limite - 30), Color.Red);
                spriteBatch.Draw(plus, rectangles[11], Color.White);

                spriteBatch.Draw(moins, rectangles[12], Color.White);
                spriteBatch.DrawString(font, munitions[5].ToString(), new Vector2(450, limite - 30), Color.Red);
                spriteBatch.Draw(plus, rectangles[13], Color.White);

                spriteBatch.Draw(moins, rectangles[14], Color.White);
                spriteBatch.DrawString(font, munitions[6].ToString(), new Vector2(620, limite - 30), Color.Red);
                spriteBatch.Draw(plus, rectangles[15], Color.White);

                spriteBatch.Draw(moins, rectangles[16], Color.White);
                spriteBatch.DrawString(font, munitions[7].ToString(), new Vector2(790, limite - 30), Color.Red);
                spriteBatch.Draw(plus, rectangles[17], Color.White);
            }
        }

        public void LoadContent(ContentManager content)
        {
            shuriken = content.Load<Texture2D>(@"Barre infos\shuriken barre");
            hadoken = content.Load<Texture2D>(@"Barre infos\hadoken barre");
            fumigene = content.Load<Texture2D>(@"Barre infos\fumigene barre");
            traineeDeFlamme = content.Load<Texture2D>(@"Barre infos\boule de feu barre");
            plus = content.Load<Texture2D>(@"Barre infos\plus");
            moins = content.Load<Texture2D>(@"Barre infos\moins");
            heros1 = content.Load<Texture2D>(@"Barre infos\Heros1 migna");
            heros2 = content.Load<Texture2D>(@"Barre infos\Heros2 migna");
        }
    }
}