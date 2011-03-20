using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ParticleSample;

namespace YelloKiller
{

    public class YellokillerGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

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
        float timeTillExplosion = 0.0f;

        const float TimeBetweenSmokePlumePuffs = .5f;
        float timeTillPuff = 0.0f;

        KeyboardState lastKeyboardState;

        #endregion

        #region Initialization

        public YellokillerGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = Taille_Ecran.HAUTEUR_ECRAN;
            graphics.PreferredBackBufferWidth = Taille_Ecran.LARGEUR_ECRAN;
            //graphics.IsFullScreen = true;

            explosion = new ExplosionParticleSystem(this, 1);
            Components.Add(explosion);


            smoke = new ExplosionSmokeParticleSystem(this, 2);
            Components.Add(smoke);


            smokePlume = new SmokePlumeParticleSystem(this, 9);
            Components.Add(smokePlume);


            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        #endregion
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {

            HandleInput();
           if (gameTime.ElapsedGameTime.Milliseconds > 0 && Keyboard.GetState().IsKeyDown(Keys.V))
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                switch (currentState)
                {

                    case State.Explosions:
                        UpdateExplosions(dt);
                        break;

                    case State.SmokePlume:
                        UpdateSmokePlume(dt);
                        break;
                }
        }

            base.Update(gameTime);
        }

        public Vector2 position()
        {
          Vector2 where = new Vector2(400,400);

            return where;
        }

        private void UpdateSmokePlume(float dt)
        {
            timeTillPuff -= dt;
            if (timeTillPuff < 0)
            { 
                
                smokePlume.AddParticles(position());
                timeTillPuff = TimeBetweenSmokePlumePuffs;
            }
        }

        private void UpdateExplosions(float dt)
        {
           // timeTillExplosion -= dt;
          //  if (timeTillExplosion < 0)
          //  {
                explosion.AddParticles(position());
                smoke.AddParticles(position());


                timeTillExplosion = TimeBetweenExplosions;
            //}
        }
        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.End();


            base.Draw(gameTime);
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


    static class Program
    {
        static void Main()
        {
            using (YellokillerGame game = new YellokillerGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}