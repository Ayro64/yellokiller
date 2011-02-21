using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    interface IKeyboardService
    {
        bool TouchePresse(Keys key);
        bool ToucheAEtePressee(Keys key);
    }
}
