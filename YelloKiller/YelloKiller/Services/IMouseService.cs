using Microsoft.Xna.Framework;

namespace YelloKiller
{
    interface IMouseService
    {
        bool ClicBoutonGauche();

        bool ClicBoutonDroit();

        bool ClicBoutonMilieu();

        bool BoutonGauchePresse();

        bool BoutonGaucheEnfonce();

        bool DansLEcran();

        bool DansLaCarte();

        int Molette();

        bool MoletteATournee();

        Vector2 Coordonnees();

        Rectangle Rectangle();
    }
}
