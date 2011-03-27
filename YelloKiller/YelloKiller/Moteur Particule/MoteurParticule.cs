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

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        Rectangle rectangle_hadoken;

        public Rectangle Rectangle_Hadoken
        {
            get { return rectangle_hadoken; }
        }

        ExplosionParticleSystem hadoken;
        ExplosionSmokeParticleSystem fume_hadoken;
        SmokePlumeParticleSystem fume;

        

        const float TimeBetweenExplosions = 2.0f;

        const float TimeBetweenSmokePlumePuffs = .5f;
        float timeTillPuff = 0.0f;


        #endregion

        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch, Hero hero, Rectangle camera)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            // rectangle qui prend la taille de l explosion, j'ai prix des valeur approximative pour l'instant
            // par contre je sais pas si les width et height du rectangle c'est en case ou en pixel...
            rectangle_hadoken = new Rectangle((int)(hero.position.X - camera.X), (int)(hero.position.Y - camera.Y), 28, 224);

            hadoken = new ExplosionParticleSystem(game, 1, spriteBatch);
            game.Components.Add(hadoken);



            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2, spriteBatch);
            game.Components.Add(fume_hadoken);


            fume = new SmokePlumeParticleSystem(game, 9, spriteBatch);
            game.Components.Add(fume);
        }

        #endregion


       

        public Vector2 position(Hero hero, Rectangle camera)
        {
            Vector2 where = new Vector2(hero.position.X - camera.X, hero.position.Y - camera.Y);
            return where;
        }

        public void UpdateSmokePlume(float dt, Hero hero, Rectangle camera)
        {
            timeTillPuff -= dt;
            if (timeTillPuff < 0)
            {

                fume.AddParticles(position(hero, camera), hero);
                timeTillPuff = TimeBetweenSmokePlumePuffs;
            }
        }

        public void UpdateExplosions(float dt, Hero hero, Rectangle camera)
        {

            hadoken.AddParticles(position(hero, camera), hero);
            fume_hadoken.AddParticles(position(hero, camera), hero);
        }
        #region Draw

        public void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.End();

        }

        #endregion

       


        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

