using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class KeyboardService : GameComponent, IKeyboardService
    {
        KeyboardState KBState, lastKBState;

        public KeyboardService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IKeyboardService>(this);
        }

        public bool ToucheAEtePressee(Keys key)
        {
            return lastKBState.IsKeyUp(key) && KBState.IsKeyDown(key);
        }

        public bool ToucheRelevee(Keys key)
        {
            return KBState.IsKeyUp(key);
        }

        public bool TouchePressee(Keys key)
        {
            return KBState.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            lastKBState = KBState;
            KBState = Keyboard.GetState();
        }
    }
}