using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleSample;

namespace YelloKiller.Moteur_Particule
{
    class MoteurParticule
    {
        #region Fields

        YellokillerGame game;
        SpriteBatch spriteBatch;
        int direction_hero_appele;

        bool rectangle_est_present = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectang_Est_Present
        {
            get { return rectangle_est_present; }
            set { rectangle_est_present = value; }
        }

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        Rectangle rectangle_hadoken; // rectangle qui approxime la hauteur et la largeur de mon hadoken 
        public Rectangle Rectangle_Hadoken(Hero hero) // pour gerer les collisions
        {
         //   Console.WriteLine(GameplayScreen.Enable_Timer + " , " + GameplayScreen.Timer);

            if (GameplayScreen.Timer > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer = false;
                GameplayScreen.Timer = 0;
            }

            if (hadoken.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                rectangle_est_present = true;// je reinitialise donc mon rectangle
                direction_hero_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken.FreeParticleCount < 100 && rectangle_est_present && GameplayScreen.Timer > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero_appele == 133) // haut
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y - 224, 28, 224);

                else if (direction_hero_appele == 198) // bas
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, 224);

                else if (direction_hero_appele == 230) // gauche
                    return rectangle_hadoken = new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, 224, 28);

                else // droite
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y, 224, 28);
            }
            else // pas de rectangle
                return rectangle_hadoken = new Rectangle(0, 0, 0, 0);

        }


        ExplosionParticleSystem hadoken;  // explosion 
        ExplosionSmokeParticleSystem fume_hadoken; // fume apres explosion
        SmokePlumeParticleSystem fume; // fumigene



        const float TimeBetweenExplosions = 2.0f;

        const float TimeBetweenSmokePlumePuffs = .5f;



        #endregion

        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;


            hadoken = new ExplosionParticleSystem(game, 1, spriteBatch);
            game.Components.Add(hadoken);



            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2, spriteBatch);
            game.Components.Add(fume_hadoken);


            fume = new SmokePlumeParticleSystem(game, 9, spriteBatch);
            game.Components.Add(fume);
        }

        #endregion


        public Vector2 position(Hero hero, Rectangle camera) // position d origine de mon hadoken
        {
            Vector2 where = new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y);
            return where;
        }

        public void UpdateSmokePlume(float dt, Hero hero, Rectangle camera)
        {
            fume.AddParticles(position(hero, camera), hero);

        }

        public void UpdateExplosions(float dt, Hero hero, Rectangle camera)
        {

            //  Console.WriteLine(rectangle_hadoken.X + " , " + rectangle_hadoken.Y + " , " + rectangle_hadoken.Width + " , " + rectangle_hadoken.Height);
            hadoken.AddParticles(position(hero, camera), hero);
            fume_hadoken.AddParticles(position(hero, camera), hero);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

