using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    public enum Pouvoir
    {
        shuriken = 0,
    }

    class Bonus : Sprite
    {
        Pouvoir pouvoir;

        public Bonus(Vector2 position, Pouvoir pouvoir)
            : base(position)
        {
            this.pouvoir = pouvoir;

        }

        public void LoadContent(ContentManager content)
        {
            switch (pouvoir)
            {
                case Pouvoir.shuriken:
                    LoadContent(content, "bonusShuriken");
                    break;
            }
        }
    }
}