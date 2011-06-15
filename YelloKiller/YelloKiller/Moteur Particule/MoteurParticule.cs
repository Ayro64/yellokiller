using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace YelloKiller.Moteur_Particule
{
    class MoteurParticule
    {
        #region Fields

        public static Vector2 Camera
        { get; set; }

        YellokillerGame game;
        SpriteBatch spriteBatch;

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        int direction_hero2_appele;
        int direction_hero1_appele;

        ExplosionParticleSystem hadoken_hero1;
        ExplosionParticleSystem hadoken_hero2;
        ExplosionSmokeParticleSystem fume_hadoken;
        Statue_Explosion explosion_statue;
        SmokePlumeParticleSystem fume;
        BallParticleSystem ball1;
        BallParticleSystem ball2;
        Fumigene fumigene1;
        Fumigene fumigene2;

        const float TimeBetweenExplosions = 2.0f;
        const float TimeBetweenSmokePlumePuffs = .5f;

        #endregion

        #region Rectangle tout moche Hero 1

        bool rectangle_hadoken_est_present_hero1 = true; // utiliser pour effacer le rectangle apres collision 
        public bool Rectangle_Hadoken_Est_Present_Hero1
        {
            get { return rectangle_hadoken_est_present_hero1; }
            set { rectangle_hadoken_est_present_hero1 = value; }
        }

        public Rectangle Rectangle_Hadoken_hero1(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (hadoken_hero1.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_hero1_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken_hero1.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero1_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken_hero1.LongueurHadoken * 28), 28, (hadoken_hero1.LongueurHadoken * 28));

                else if (direction_hero1_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken_hero1.LongueurHadoken * 28));

                else if (direction_hero1_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - (hadoken_hero1.LongueurHadoken * 28), (int)hero.position.Y, (hadoken_hero1.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken_hero1.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        bool rectangle_ball_est_present_hero1 = true;
        public bool Rectangle_Ball_Est_Present_Hero1
        {
            get { return rectangle_ball_est_present_hero1; }
            set { rectangle_ball_est_present_hero1 = value; }
        }

        public Rectangle Rectangle_Ball_hero1(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (ball1.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_hero1_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (ball1.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero1_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (ball1.LongueurBall * 28), 28, (ball1.LongueurBall * 28));

                else if (direction_hero1_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (ball1.LongueurBall * 28));

                else if (direction_hero1_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - (ball1.LongueurBall * 28), (int)hero.position.Y, (ball1.LongueurBall * 28), 28);

                else  // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (ball1.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Hero 2

        bool rectangle_hadoken_est_present_hero2 = true;
        public bool Rectangle_Hadoken_Est_Present_Hero2
        {
            get { return rectangle_hadoken_est_present_hero2; }
            set { rectangle_hadoken_est_present_hero2 = value; }
        }

        public Rectangle Rectangle_Hadoken_hero2(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (hadoken_hero2.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_hero2_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (hadoken_hero2.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero2_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken_hero2.LongueurHadoken * 28), 28, (hadoken_hero2.LongueurHadoken * 28));

                else if (direction_hero2_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken_hero2.LongueurHadoken * 28));

                else if (direction_hero2_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - (hadoken_hero2.LongueurHadoken * 28), (int)hero.position.Y, (hadoken_hero2.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken_hero2.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        bool rectangle_ball_est_present_hero2 = true;
        public bool Rectangle_Ball_Est_Present_Hero2
        {
            get { return rectangle_ball_est_present_hero2; }
            set { rectangle_ball_est_present_hero2 = value; }
        }

        public Rectangle Rectangle_Ball_hero2(Hero hero) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (ball2.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_hero2_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if (ball2.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero2_appele == 133) // haut
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y - (ball2.LongueurBall * 28), 28, (ball2.LongueurBall * 28));

                else if (direction_hero2_appele == 198) // bas
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (ball2.LongueurBall * 28));

                else if (direction_hero2_appele == 230) // gauche
                    return new Rectangle((int)hero.position.X - (ball2.LongueurBall * 28), (int)hero.position.Y, (ball2.LongueurBall * 28), 28);

                else // droite 
                    return new Rectangle((int)hero.position.X, (int)hero.position.Y, (ball2.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Statue

        bool rectangle_hadoken_est_present_statue = true; // utiliser pour effacer le rectangle apres collision 
        public bool Rectangle_Hadoken_Est_Present_Statue
        {
            get { return rectangle_hadoken_est_present_statue; }
            set { rectangle_hadoken_est_present_statue = value; }
        }

        public Rectangle Rectangle_Hadoken_Statue(Statue statue) // pour gerer les collisions
        {
            if (explosion_statue.FreeParticleCount == 2000) // lorsque freeparticulecount = 2000 le hadoken est termine 
                Rectangle_Hadoken_Est_Present_Statue = true;// je reinitialise donc mon rectangle

            if (explosion_statue.FreeParticleCount < 2000 && Rectangle_Hadoken_Est_Present_Statue)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (statue.SourceRectangle.Value.Y == 357) // haut
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y - (explosion_statue.LongueurExplosion * 28) + (statue.SourceRectangle.Value.Height / 2), 28, (explosion_statue.LongueurExplosion * 28));

                else if (statue.SourceRectangle.Value.Y == 0) // bas
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), 28, (explosion_statue.LongueurExplosion * 28));

                else if (statue.SourceRectangle.Value.Y == 123) // gauche
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2) - (explosion_statue.LongueurExplosion * 28), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), (explosion_statue.LongueurExplosion * 28), 28);

                else // droite
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), (explosion_statue.LongueurExplosion * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Fumigene

        bool rectangle_fumigene_est_present = true; // utiliser pour effacer le rectangle apres collision 
        public bool Rectangle_Fumigene_Est_Present
        {
            get { return rectangle_fumigene_est_present; }
            set { rectangle_fumigene_est_present = value; }
        }

        public Rectangle Rectangle_Fumigene(Hero hero) // pour gerer les collisions
        {
            if (fumigene1.FreeParticleCount == 135) // lorsque freeparticulecount = 135 le fumigene est termine 
                Rectangle_Fumigene_Est_Present = true;// je reinitialise donc mon rectangle

            if (fumigene1.FreeParticleCount < 135 && Rectangle_Fumigene_Est_Present)
                return new Rectangle((int)hero.position.X - 28 * 3, (int)hero.position.Y - 28 * 3, 28 * 6, 28 * 6);
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion



        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch, Carte carte, Hero hero1, Hero hero2, List<Statue> _statue)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            hadoken_hero1 = new ExplosionParticleSystem(game, 1, hero1, carte, 50);
            game.Components.Add(hadoken_hero1);

            ball1 = new BallParticleSystem(game, 1, hero1, carte);
            game.Components.Add(ball1);

            foreach (Statue statue in _statue)
            {
                explosion_statue = new Statue_Explosion(game, 20, carte, statue, statue.Distance_Statue_Mur(carte));
                game.Components.Add(explosion_statue);
            }

            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2);
            game.Components.Add(fume_hadoken);

            fumigene1 = new Fumigene(game, 9, hero1, carte);
            game.Components.Add(fumigene1);

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);

            try
            {
                hadoken_hero2 = new ExplosionParticleSystem(game, 1, hero2, carte, 50);
                game.Components.Add(hadoken_hero2);
                ball2 = new BallParticleSystem(game, 1, hero2, carte);
                fumigene2 = new Fumigene(game, 9, hero2, carte);
                game.Components.Add(fumigene2);
                game.Components.Add(ball2);
            }
            catch (NullReferenceException)
            { }
        }

        #endregion

        public void UpdateFumigene(Hero hero)
        {           
            if (hero.NumeroHero == 1)
                fumigene1.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
            else
                fumigene2.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
        }

        public void UpdateExplosions_hero(Hero hero)
        {
            if (hero.NumeroHero == 1)
                hadoken_hero1.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
            else
                hadoken_hero2.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);

            fume_hadoken.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
        }

        public void UpdateExplosions_statue(Statue statue)
        {
            explosion_statue.AddParticles(new Vector2(statue.position.X - Camera.X + (statue.SourceRectangle.Value.Width / 2), statue.position.Y - Camera.Y + (statue.SourceRectangle.Value.Height / 2)), statue);
        }

        public void UpdateBall(Hero hero)
        {
            if (hero.NumeroHero == 1)
                ball1.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
            else
                ball2.AddParticles(new Vector2(hero.position.X - Camera.X, hero.position.Y - Camera.Y), hero);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

