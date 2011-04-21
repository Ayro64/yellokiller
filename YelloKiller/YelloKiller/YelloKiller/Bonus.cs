using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    public enum TypeBonus
    {
        shuriken = 0,
    }

    class Bonus : Sprite
    {
        TypeBonus typeBonus;

        public Bonus(Vector2 position, TypeBonus pouvoir)
            : base(position)
        {
            this.typeBonus = pouvoir;

        }

        public void LoadContent(ContentManager content)
        {
            switch (typeBonus)
            {
                case TypeBonus.shuriken:
                    LoadContent(content, "bonusShuriken");
                    break;
            }
        }

        public TypeBonus TypeBonus
        {
            get { return typeBonus; }
        }
    }
}