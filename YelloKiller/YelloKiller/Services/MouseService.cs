using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    public class MouseService : GameComponent, IMouseService
    {
        MouseState MState = Mouse.GetState(), LastMState;
        Rectangle rectangle;

        public MouseService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IMouseService>(this);
        }

        public bool ClicBoutonGauche()
        {
            return MState.LeftButton == ButtonState.Released && LastMState.LeftButton == ButtonState.Pressed;
        }

        public bool BoutonGauchePresse()
        {
            return MState.LeftButton == ButtonState.Pressed;
        }

        public bool BoutonGaucheEnfonce()
        {
            return MState.LeftButton == ButtonState.Pressed && LastMState.LeftButton == ButtonState.Released;
        }

        public Vector2 Coordonnees()
        {
            return new Vector2(MState.X, MState.Y);
        }

        public Rectangle Rectangle()
        {
            return rectangle;
        }

        public int Molette()
        {
            return MState.ScrollWheelValue / 12 - LastMState.ScrollWheelValue / 12;
        }

        public bool MoletteATournee()
        {
            return MState.ScrollWheelValue != LastMState.ScrollWheelValue;
        }

        public bool DansLEcran()
        {
            if (MState.X < 0)
                return false;
            if (MState.Y < 0)
                return false;
            if (MState.X > Taille_Ecran.LARGEUR_ECRAN)
                return false;
            if (MState.Y > Taille_Ecran.HAUTEUR_ECRAN)
                return false;
            return true;
        }
         

        public bool DansLaCarte()
        {
                if (MState.X > 0 && MState.X < Taille_Ecran.LARGEUR_ECRAN - 56 && MState.Y > 0 && MState.Y < Taille_Ecran.HAUTEUR_ECRAN)
                    return true;
                else
                    return false;
        }

        public override void Update(GameTime gameTime)
        {
            LastMState = MState;
            MState = Mouse.GetState();
            rectangle = new Rectangle(MState.X, MState.Y, 1, 1);
        }
    }
}
