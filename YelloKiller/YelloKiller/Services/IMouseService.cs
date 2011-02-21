using Microsoft.Xna.Framework;

namespace YelloKiller
{
    interface IMouseService
    {
        bool ClicBoutonGauche();

        bool BoutonGauchePresse();

        bool BoutonGaucheEnfonce();

        bool DansLEcran();

        bool DansLaCarte();

        Vector2 Coordonnees();
        
        Rectangle Rectangle();
    }
}
