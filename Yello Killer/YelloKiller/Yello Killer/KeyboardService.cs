using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class KeyboardService : GameComponent, IKeyboardService
    {
        KeyboardState KBState;
        KeyboardState lastKBState;

        public KeyboardService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IKeyboardService>(this);
        }

        public bool ToucheAEtePressee(Keys key)
        {
            return lastKBState.IsKeyUp(key) && KBState.IsKeyDown(key);
        }

        public bool TouchePresse(Keys key)
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