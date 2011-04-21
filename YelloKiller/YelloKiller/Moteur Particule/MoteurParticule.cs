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
        public Hero Hero
        { get; set; }

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        int direction_hero1_appele;
        int direction_hero2_appele;

        bool rectangle_hadoken_est_present_hero1 = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Hadoken_Est_Present_Hero1
        {
            get { return rectangle_hadoken_est_present_hero1; }
            set { rectangle_hadoken_est_present_hero1 = value; }
        }

        bool rectangle_hadoken_est_present_hero2 = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Hadoken_Est_Present_Hero2
        {        
            get { return rectangle_hadoken_est_present_hero2; }
            set { rectangle_hadoken_est_present_hero2 = value; }
        }

        bool rectangle_ball_est_present_hero1 = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Ball_Est_Present_Hero1
        {
            get { return rectangle_ball_est_present_hero1; }
            set { rectangle_ball_est_present_hero1 = value; }
        }

        bool rectangle_ball_est_present_hero2 = true; // utiliser pour effacer le rectangle apres collision contre boss
        public bool Rectangle_Ball_Est_Present_Hero2
        {
            get { return rectangle_ball_est_present_hero2; }
            set { rectangle_ball_est_present_hero2 = value; }
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

        #region Rectangle tout moche Hero 1

        public Rectangle Rectangle_Hadoken_hero1(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (hadoken_hero.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_hero1_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken_hero.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero1_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken_hero.LongueurHadoken * 28), 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero1_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero1_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle Rectangle_Ball_hero1(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (ball.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_hero1_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (ball.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero1_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (ball.LongueurBall * 28), 28, (ball.LongueurBall * 28));

                else if (direction_hero1_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (ball.LongueurBall * 28));

                else if (direction_hero1_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (ball.LongueurBall * 28), 28);

                else  // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (ball.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Hero 2

        public Rectangle Rectangle_Hadoken_hero2(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (hadoken_hero.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_hero2_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken_hero.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero2_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken_hero.LongueurHadoken * 28), 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero2_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken_hero.LongueurHadoken * 28));

                else if (direction_hero2_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken_hero.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle Rectangle_Ball_hero2(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (ball.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_hero2_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (ball.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero2_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (ball.LongueurBall * 28), 28, (ball.LongueurBall * 28));

                else if (direction_hero2_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (ball.LongueurBall * 28));

                else if (direction_hero2_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (ball.LongueurBall * 28), 28);

                else // droite 
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (ball.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion


        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch, Carte carte, Hero hero, List<Statue> _statue)
        {
            Hero = hero;
            this.game = game;
            this.spriteBatch = spriteBatch;

            hadoken_hero = new ExplosionParticleSystem(game, 1, hero, carte, 50);
            game.Components.Add(hadoken_hero);

            ball = new BallParticleSystem(game, 1, hero, carte);
            game.Components.Add(ball);

            foreach (Statue statue in _statue)
            {
                explosion_statue = new Statue_Explosion(game, 20, carte, statue, statue.Distance_Statue_Mur(carte));
                game.Components.Add(explosion_statue);
            }

            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2);
            game.Components.Add(fume_hadoken);

            fumigene = new Fumigene(game, 9, hero, carte);
            game.Components.Add(fumigene);

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);
        }

        #endregion

        public void UpdateFumigene(Hero hero, Rectangle camera)
        {
            fumigene.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public void UpdateExplosions_hero(Hero hero, Rectangle camera)
        {
            hadoken_hero.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
            fume_hadoken.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public void UpdateExplosions_statue(Rectangle camera, Statue statue)
        {
            explosion_statue.AddParticles(new Vector2(statue.position.X - camera.X + (statue.SourceRectangle.Value.Width / 2), statue.position.Y - camera.Y + (statue.SourceRectangle.Value.Height / 2)), statue);
        }

        public void UpdateBall(Hero hero, Rectangle camera)
        {
            ball.AddParticles(new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y), hero);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

