using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    interface IKeyboardService
    {
        bool TouchePressee(Keys key);
        bool ToucheAEtePressee(Keys key);
        bool ToucheRelevee(Keys key);
    }
}