using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    public class EnnemiMort : Sprite
    {
        public Rectangle Rectangle { get; private set; }
        public TypeEnnemiMort Type { get; set; }

        public enum TypeEnnemiMort
        {
            garde = 0,
            patrouilleur = 1,
            patrouilleurACheval = 2,
            boss = 3
        }

        public EnnemiMort(Vector2 position, ContentManager content, TypeEnnemiMort type)
            :base(position)
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 28, 28);
            this.Type = type;
            switch (type)
            {
                case TypeEnnemiMort.garde:
                    LoadContent(content, @"Menu Editeur de Maps\gardeMort");
                    break;
                case TypeEnnemiMort.patrouilleur:
                    LoadContent(content, @"Menu Editeur de Maps\Patrouilleur mort");
                    break;
                case TypeEnnemiMort.patrouilleurACheval:
                    LoadContent(content, @"Menu Editeur de Maps\patrouilleurAChevalMort");
                    break;
                case TypeEnnemiMort.boss:
                    LoadContent(content, @"Menu Editeur de Maps\Boss mort");
                    break;
            }
        }
    }
}
