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

        ExplosionParticleSystem explosion;
        ExplosionSmokeParticleSystem smoke;
        SmokePlumeParticleSystem smokePlume;

        enum State
        {
            Explosions,
            SmokePlume
        };

        const int NumStates = 2;
        State currentState = State.Explosions;

        const float TimeBetweenExplosions = 2.0f;

        const float TimeBetweenSmokePlumePuffs = .5f;
        float timeTillPuff = 0.0f;

        KeyboardState lastKeyboardState;

        #endregion

        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            
            explosion = new ExplosionParticleSystem(game, 1, spriteBatch);
            game.Components.Add(explosion);

            

            smoke = new ExplosionSmokeParticleSystem(game, 2, spriteBatch);
            game.Components.Add(smoke);


            smokePlume = new SmokePlumeParticleSystem(game, 9, spriteBatch);
            game.Components.Add(smokePlume);
        }

        #endregion


        public void Update(GameTime gameTime, Hero hero, Rectangle camera)
        {

            HandleInput();
            if (gameTime.ElapsedGameTime.Milliseconds > 0 && Keyboard.GetState().IsKeyDown(Keys.V))
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                switch (currentState)
                {

                    case State.Explosions:
                        UpdateExplosions(dt, hero, camera);
                        break;

                    case State.SmokePlume:
                        UpdateSmokePlume(dt, hero, camera);
                        break;
                }
            }
          }

        public Vector2 position(Hero hero, Rectangle camera)
        {
            Console.WriteLine("" + (hero.position.X - Taille_Ecran.LARGEUR_ECRAN) + " , " + (hero.position.Y - -Taille_Ecran.HAUTEUR_ECRAN));
       Vector2 where = new Vector2(hero.position.X - camera.X  , hero.position.Y - camera.Y);
          //Vector2 where = new Vector2(400, 400);
            return where;
        }

        private void UpdateSmokePlume(float dt, Hero hero, Rectangle camera)
        {
            timeTillPuff -= dt;
            if (timeTillPuff < 0)
            {

                smokePlume.AddParticles(position(hero, camera));
                timeTillPuff = TimeBetweenSmokePlumePuffs;
            }
        }

        private void UpdateExplosions(float dt, Hero hero, Rectangle camera)
        {

            explosion.AddParticles(position(hero, camera));
            smoke.AddParticles(position(hero ,camera));
        }
        #region Draw

        public void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.End();

        }

        #endregion

        private void HandleInput()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();


            bool keyboardSpace =
                currentKeyboardState.IsKeyUp(Keys.C) &&
                lastKeyboardState.IsKeyDown(Keys.C);



            if (keyboardSpace)
            {
                currentState = (State)((int)(currentState + 1) % NumStates);
            }

            lastKeyboardState = currentKeyboardState;
        }


        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }

    #region Entry Point




    #endregion
}

