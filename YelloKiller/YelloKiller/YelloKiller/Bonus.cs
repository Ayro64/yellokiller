using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    public enum TypeBonus
    {
        shuriken = 0,
        hadoken = 1,
        checkPoint = 2
    }

    public class Bonus : Sprite
    {
        public TypeBonus TypeBonus { get; private set; }

        public Bonus(Vector2 position, TypeBonus pouvoir)
            : base(position)
        {
            this.TypeBonus = pouvoir;
        }

        public void LoadContent(ContentManager content)
        {
            switch (TypeBonus)
            {
                case TypeBonus.shuriken:
                    LoadContent(content, @"Menu Editeur de Maps\bonusShuriken");
                    break;
                case TypeBonus.hadoken:
                    LoadContent(content, @"Menu Editeur de Maps\bonusHadoken");
                    break;
                case TypeBonus.checkPoint:
                    LoadContent(content, "CheckPoint");
                    break;
            }
        }

        public void SauvegarderCheckPoint(ref StreamWriter file)
        {
            file.WriteLine(position.X + "," + position.Y + "," + (int)TypeBonus);
        }
    }
}