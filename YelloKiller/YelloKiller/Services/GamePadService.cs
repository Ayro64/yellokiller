using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    public class GamePadService : GameComponent, IGamePadService
    {
        GamePadState GPState, lastGPState;
        PlayerIndex joueur1 = new PlayerIndex();

        public GamePadService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IGamePadService>(this);
        }

        public bool ManetteConnectee()
        {
            return GPState.IsConnected;
        }

        public bool AllerAGauche()
        {
            return GPState.ThumbSticks.Left.X < -0.7;
        }

        public bool AllerADroite()
        {
            return GPState.ThumbSticks.Left.X > 0.7;
        }

        public bool AllerEnHaut()
        {
            return GPState.ThumbSticks.Left.Y > 0.7;
        }

        public bool AllerEnBas()
        {
            return GPState.ThumbSticks.Left.Y < -0.7;
        }

        public bool Tirer()
        {
            return GPState.Buttons.A == ButtonState.Released && lastGPState.Buttons.A == ButtonState.Pressed;
        }

        public bool ChangerArme()
        {
            return GPState.Buttons.RightShoulder == ButtonState.Released && lastGPState.Buttons.RightShoulder == ButtonState.Pressed;
        }

        public bool Courir()
        {
            return GPState.Triggers.Right > 0.7;
        }

        public override void Update(GameTime gameTime)
        {
            lastGPState = GPState;
            GPState = GamePad.GetState(joueur1);
        }
    }
}
