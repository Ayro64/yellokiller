using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    interface IGamePadService
    {
        bool ManetteConnectee();

        bool AllerAGauche();

        bool AllerADroite();

        bool AllerEnHaut();

        bool AllerEnBas();

        bool Tirer();

        bool ChangerArme();

        bool Courir();

        void Vibration(int tempsDurantLequelLaManetteVibre);
    }
}