using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace YelloKiller.Moteur_Particule
{
    class MoteurParticule
    {
        #region Fields

        YellokillerGame game;
        SpriteBatch spriteBatch;
        public Statue statue
        { get; set; }
        Hero hero;

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        int direction_hero_appele;
        int direction_statue_appele;

        bool rectangle_hadoken_est_present = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Hadoken_Est_Present
        {
            get { return rectangle_hadoken_est_present; }
            set { rectangle_hadoken_est_present = value; }
        }

        bool rectangle_ball_est_present = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Ball_Est_Present
        {
            get { return rectangle_ball_est_present; }
            set { rectangle_ball_est_present = value; }
        }

        ExplosionParticleSystem hadoken_hero;  // explosion 
        ExplosionSmokeParticleSystem fume_hadoken; // fume apres explosion
        Statue_Explosion explosion_statue;
        SmokePlumeParticleSystem fume; // fumigene
        BallParticleSystem ball;
        Fumigene fumigene;

        const float TimeBetweenExplosions = 2.0f;
        const float TimeBetweenSmokePlumePuffs = .5f;

        #endregion

        #region Rectangle tout moche

        public Rectangle Rectangle_Hadoken(Hero hero) // pour gerer les collisions
        {
            //   Console.WriteLine(GameplayScreen.Enable_Timer + " , " + GameplayScreen.Timer);

            if (GameplayScreen.Timer > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer = false;
                GameplayScreen.Timer = 0;
            }

            if (hadoken_hero.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present = true;// je reinitialise donc mon rectangle
                direction_hero_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken_hero.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present && GameplayScreen.Timer > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken_hero.LongueurHadoken * 28), 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle Rectangle_Ball(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer = false;
                GameplayScreen.Timer = 0;
            }

            if (ball.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present = true;// je reinitialise donc mon rectangle
                direction_statue_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (ball.FreeParticleCount < 100 && Rectangle_Ball_Est_Present && GameplayScreen.Timer > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_statue_appele == 357) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (ball.LongueurBall * 28), 28, (ball.LongueurBall * 28));

                else if (direction_statue_appele == 0) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (ball.LongueurBall * 28));

                else if (direction_statue_appele == 123) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (ball.LongueurBall * 28), 28);

                else // droite (243)
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (ball.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch, Hero hero, Carte carte)
        {
            this.hero = hero;
            this.game = game;
            this.spriteBatch = spriteBatch;

            hadoken_hero = new ExplosionParticleSystem(game, 1, hero, carte);
            game.Components.Add(hadoken_hero);

            explosion_statue = new Statue_Explosion(game, 10, carte, statue);
            game.Components.Add(explosion_statue);

            ball = new BallParticleSystem(game, 1, hero, carte);
            game.Components.Add(ball);

            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2);
            game.Components.Add(fume_hadoken);

            fumigene = new Fumigene(game, 9, hero, carte);
            game.Components.Add(fumigene);

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);
        }

        #endregion

        public void UpdateFumigene(float dt, Rectangle camera)
        {
            fumigene.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public void UpdateExplosions_hero(float dt, Rectangle camera)
        {
            hadoken_hero.maxspeed = 50;
            hadoken_hero.minspeed = 50;           

            hadoken_hero.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
            fume_hadoken.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public void UpdateExplosions_statue(float dt, Statue statue, ref Rectangle camera)
        {
            this.statue = statue;
            explosion_statue.maxspeed = 50;
            explosion_statue.minspeed = 50;

            explosion_statue.AddParticles(new Vector2(statue.position.X - camera.X + (statue.SourceRectangle.Value.Width / 2), statue.position.Y - camera.Y + (statue.SourceRectangle.Value.Height / 2)), statue);
  }

        public void UpdateBall(float dt, Rectangle camera)
        {
            ball.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

