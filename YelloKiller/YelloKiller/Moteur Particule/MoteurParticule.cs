﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YelloKiller.YelloKiller;

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

            if (hadoken.FreeParticleCount == 100 && ball.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                rectangle_est_present = true;// je reinitialise donc mon rectangle
                direction_hero_appele = hero.SourceRectangle.Value.Y;
            }// direction du hero au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon hero.


            if ((hadoken.FreeParticleCount < 100 || ball.FreeParticleCount < 100) && rectangle_est_present && GameplayScreen.Timer > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_hero_appele == 133) // haut
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y - (hadoken.LongueurHadoken * 28), 28, (hadoken.LongueurHadoken * 28));

                else if (direction_hero_appele == 198) // bas
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y, 28, (hadoken.LongueurHadoken * 28));

                else if (direction_hero_appele == 230) // gauche
                    return rectangle_hadoken = new Rectangle((int)hero.position.X - 224, (int)hero.position.Y, (hadoken.LongueurHadoken * 28), 28);

                else // droite
                    return rectangle_hadoken = new Rectangle((int)hero.position.X, (int)hero.position.Y, (hadoken.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return rectangle_hadoken = new Rectangle(0, 0, 0, 0);

        }


        ExplosionParticleSystem hadoken;  // explosion 
        ExplosionSmokeParticleSystem fume_hadoken; // fume apres explosion
        SmokePlumeParticleSystem fume; // fumigene
        BallParticleSystem ball;
        Fumigene fumigene;



        const float TimeBetweenExplosions = 2.0f;

        const float TimeBetweenSmokePlumePuffs = .5f;



        #endregion

        #region Initialization

        public MoteurParticule(YellokillerGame game, Hero hero, Carte carte, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            hadoken = new ExplosionParticleSystem(game, 1);
            game.Components.Add(hadoken);

            ball = new BallParticleSystem(game, 1);
            game.Components.Add(ball);

            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2);
            game.Components.Add(fume_hadoken);

            fumigene = new Fumigene(game, 9);
            game.Components.Add(fumigene);

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);
        }

        #endregion


        public Vector2 position(Hero hero, Rectangle camera) // position d origine de mon hadoken
        {
            Vector2 where = new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y);
            return where;
        }
    
        public void UpdateFumigene(float dt, Hero hero, Carte carte, Rectangle camera)
        {
            fumigene.Hero = hero;
            fumigene.Carte = carte;
            fumigene.AddParticles(position(hero, camera), hero);
        }

        public void UpdateExplosions(float dt, Hero hero, Carte carte, Rectangle camera)
        {
            hadoken.Hero = hero; // initialisation des propriétés de explosion particule system
            hadoken.Carte = carte;
            hadoken.AddParticles(position(hero, camera), hero);
            fume_hadoken.AddParticles(position(hero, camera), hero);
        }

        public void UpdateExplosions(float dt, Statue statue, Carte carte, Rectangle camera)
        {
            hadoken.statue = statue; // initialisation des propriétés de explosion particule system
            hadoken.Carte = carte;
            hadoken.AddParticles(new Vector2(statue.position.X - camera.X, statue.position.Y - camera.Y), statue);
            fume_hadoken.AddParticles(new Vector2(statue.position.X - camera.X, statue.position.Y - camera.Y), statue);
            Console.WriteLine((statue.position.X - camera.X) + " , " + (statue.position.Y - camera.Y));
        }

        public void UpdateBall(float dt, Hero hero, Carte carte, Rectangle camera)
        {
            ball.Hero = hero; // initialisation des propriétés de explosion particule system
            ball.Carte = carte;
            ball.AddParticles(position(hero, camera), hero);
        }

        public void UpdateBall(float dt, Statue statue, Carte carte, Rectangle camera)
        {
            ball.statue = statue; // initialisation des propriétés de explosion particule system
            ball.Carte = carte;
            ball.AddParticles(new Vector2(statue.position.X - camera.X, statue.position.Y - camera.Y), statue);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

