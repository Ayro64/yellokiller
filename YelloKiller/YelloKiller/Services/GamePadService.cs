using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    public class GamePadService : GameComponent, IGamePadService
    {
        GamePadState GPState, lastGPState;
        PlayerIndex joueur1 = new PlayerIndex();
        int compteur = 0, tempsDurantLequelLaManetteVibre = 0;
        bool vibreur = false;

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
            return GPState.Buttons.A == ButtonState.Pressed && lastGPState.Buttons.A == ButtonState.Released;
        }

        public bool ChangerArme()
        {
            return GPState.Buttons.RightShoulder == ButtonState.Released && lastGPState.Buttons.RightShoulder == ButtonState.Pressed ||
                   GPState.Buttons.LeftShoulder == ButtonState.Released && lastGPState.Buttons.LeftShoulder == ButtonState.Pressed;
        }

        public bool Courir()
        {
            return GPState.Triggers.Right > 0.7 || GPState.Triggers.Left > 0.7; ;
        }

        public void Vibration(int tempsDurantLequelLaManetteVibre)
        {
            this.tempsDurantLequelLaManetteVibre = tempsDurantLequelLaManetteVibre;
            compteur = 0;
            vibreur = true;
        }

        public override void Update(GameTime gameTime)
        {
            lastGPState = GPState;
            GPState = GamePad.GetState(joueur1);

            if (vibreur)
            {
                compteur++;
                GamePad.SetVibration(joueur1, 1, 1);
            }
            if (compteur > tempsDurantLequelLaManetteVibre)
            {
                vibreur = false;
                GamePad.SetVibration(joueur1, 0, 0);
            }
        }
    }
}
