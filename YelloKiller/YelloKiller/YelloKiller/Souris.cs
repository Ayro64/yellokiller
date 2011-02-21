using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class Souris
    {
        Rectangle rectangle;
        MouseState mState, lastMState;
        bool dansLaCarte;

        public Souris()
        {
        }

        public Rectangle Rectangle
        {
           get {return rectangle;}
        }

        public MouseState MState
        {
            get { return mState; }
        }

        public MouseState LastMState
        {
            get { return lastMState; }
        }

        public bool DansLaCarte
        {
            get { return dansLaCarte; }
        }

        public void Update()
        {
            lastMState = MState;
            mState = Mouse.GetState();
            rectangle = new Rectangle(MState.X, MState.Y, 1, 1);

            if (Rectangle.X > 28 && Rectangle.X < Taille_Ecran.LARGEUR_ECRAN - 84 && Rectangle.Y > 28 && Rectangle.Y < Taille_Ecran.HAUTEUR_ECRAN - 28)
                dansLaCarte = true;
            else
                dansLaCarte = false;
        }
    }
}