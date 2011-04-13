#region Using Statements
using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace YelloKiller.Moteur_Particule
{

    abstract class ParticleSystem : DrawableGameComponent // c est a cause de cet heritage que jai du rajouter des 
    {                                                     // YelloKiller game; un peu partout
      
        public const int AlphaBlendDrawOrder = 100;
        public const int AdditiveDrawOrder = 200;


        private YellokillerGame game;
        protected SpriteBatch spriteBatch;

        // the texture this particle system will use.
        private Texture2D texture;

        // the origin when we're drawing textures. this will be the middle of the
        // texture.
        private Vector2 origin;

        // this number represents the maximum number of effects this particle system
        // will be expected to draw at one time. this is set in the constructor and is
        // used to calculate how many particles we will need.
        private int howManyEffects;

        // the array of particles used by this system. these are reused, so that calling
        // AddParticles will not cause any allocations.
        Particle[] particles;


        Queue<Particle> freeParticles; // comparable a une liste chaine

        public int FreeParticleCount 
        {
            get { return freeParticles.Count; }
        }



        #region constants to be set by subclasses

        /// <summary>
        /// minNumParticles and maxNumParticles control the number of particles that are
        /// added when AddParticles is called. The number of particles will be a random
        /// number between minNumParticles and maxNumParticles.
        /// </summary>
        protected int minNumParticles;
        protected int maxNumParticles; // on le reutilise dans moteur particule

        /// <summary>
        /// this controls the texture that the particle system uses. It will be used as
        /// an argument to ContentManager.Load.
        /// </summary>
        protected string textureFilename;

        /// <summary>
        /// minInitialSpeed and maxInitialSpeed are used to control the initial velocity
        /// of the particles. The particle's initial speed will be a random number 
        /// between these two. The direction is determined by the function 
        /// PickRandomDirection, which can be overriden.
        /// </summary>
        protected float minInitialSpeed;
        protected float maxInitialSpeed;

        /// <summary>
        /// minAcceleration and maxAcceleration are used to control the acceleration of
        /// the particles. The particle's acceleration will be a random number between
        /// these two. By default, the direction of acceleration is the same as the
        /// direction of the initial velocity.
        /// </summary>
        protected float minAcceleration;
        protected float maxAcceleration;

        /// <summary>
        /// minRotationSpeed and maxRotationSpeed control the particles' angular
        /// velocity: the speed at which particles will rotate. Each particle's rotation
        /// speed will be a random number between minRotationSpeed and maxRotationSpeed.
        /// Use smaller numbers to make particle systems look calm and wispy, and large 
        /// numbers for more violent effects.
        /// </summary>
        protected float minRotationSpeed;
        protected float maxRotationSpeed;

        /// <summary>
        /// minLifetime and maxLifetime are used to control the lifetime. Each
        /// particle's lifetime will be a random number between these two. Lifetime
        /// is used to determine how long a particle "lasts." Also, in the base
        /// implementation of Draw, lifetime is also used to calculate alpha and scale
        /// values to avoid particles suddenly "popping" into view
        /// </summary>
        protected float minLifetime;
        protected float maxLifetime;

        /// <summary>
        /// to get some additional variance in the appearance of the particles, we give
        /// them all random scales. the scale is a value between minScale and maxScale,
        /// and is additionally affected by the particle's lifetime to avoid particles
        /// "popping" into view.
        /// </summary>
        protected float minScale;
        protected float maxScale;

        /// <summary>
        /// different effects can use different blend modes. fire and explosions work
        /// well with additive blending, for example.
        /// </summary>
        protected SpriteBlendMode spriteBlendMode;

        #endregion

        protected ParticleSystem(YellokillerGame game, int howManyEffects)
            : base(game)
        {
            this.game = game;
            this.howManyEffects = howManyEffects;
        }


        public override void Initialize()
        {
            InitializeConstants();

            particles = new Particle[howManyEffects * maxNumParticles];
            freeParticles = new Queue<Particle>(howManyEffects * maxNumParticles);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
                freeParticles.Enqueue(particles[i]);
            }
            base.Initialize();
        }


        protected abstract void InitializeConstants();


        protected override void LoadContent()
        {
            // make sure sub classes properly set textureFilename.
            if (string.IsNullOrEmpty(textureFilename))
            {
                string message = "textureFilename wasn't set properly, so the " +
                    "particle system doesn't know what texture to load. Make " +
                    "sure your particle system's InitializeConstants function " +
                    "properly sets textureFilename.";
                throw new InvalidOperationException(message);
            }
            // load the texture....
            texture = game.Content.Load<Texture2D>(textureFilename);

            // ... and calculate the center. this'll be used in the draw call, we
            // always want to rotate and scale around this point.
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }


        public void AddParticles(Vector2 where, Hero hero)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                MoteurParticule.Random.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles.Dequeue();
                InitializeParticle(p, where, hero);
            }
        }

        public void AddParticles(Vector2 where)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                MoteurParticule.Random.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles.Dequeue();
                InitializeParticle(p, where);
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where, Hero hero)
        {
            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            Vector2 direction = PickRandomDirection(hero);

            // pick some random values for our particle
            float velocity =
                MoteurParticule.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration =
                MoteurParticule.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime =
                MoteurParticule.RandomBetween(minLifetime, maxLifetime);
            float scale =
                MoteurParticule.RandomBetween(minScale, maxScale);
            float rotationSpeed =
                MoteurParticule.RandomBetween(minRotationSpeed, maxRotationSpeed);

            // then initialize it with those random values. initialize will save those,
            // and make sure it is marked as active.
            p.Initialize(
                where, velocity * direction, acceleration * direction,
                lifetime, scale, rotationSpeed);
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            Vector2 direction = PickRandomDirection();

            // pick some random values for our particle
            float velocity =
                MoteurParticule.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration =
                MoteurParticule.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime =
                MoteurParticule.RandomBetween(minLifetime, maxLifetime);
            float scale =
                MoteurParticule.RandomBetween(minScale, maxScale);
            float rotationSpeed =
                MoteurParticule.RandomBetween(minRotationSpeed, maxRotationSpeed);

            // then initialize it with those random values. initialize will save those,
            // and make sure it is marked as active.
            p.Initialize(
                where, velocity * direction, acceleration * direction,
                lifetime, scale, rotationSpeed);
        }

        protected virtual Vector2 PickRandomDirection()
        {
            float angle = MoteurParticule.RandomBetween(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        protected virtual Vector2 PickRandomDirection(Hero hero)
        {
            if (hero.SourceRectangle.Value.Y == 133)
                return new Vector2(0, -1);

            else if (hero.SourceRectangle.Value.Y == 198)
                return new Vector2(0, 1);

            else if (hero.SourceRectangle.Value.Y == 230)
                return new Vector2(-1, 0);

            else
                return new Vector2(1, 0);
        }

        public override void Update(GameTime gameTime)
        {
            // calculate dt, the change in the since the last frame. the particle
            // updates will use this value.
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // go through all of the particles...
            foreach (Particle p in particles)
            {

                if (p.Active)
                {
                    // ... and if they're active, update them.
                    p.Update(dt);
                    // if that update finishes them, put them onto the free particles
                    // queue.
                    if (!p.Active)
                    {
                        freeParticles.Enqueue(p);
                    }
                }
            }

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(GraphicsDevice);

            // tell sprite batch to begin, using the spriteBlendMode specified in
            // initializeConstants
            spriteBatch.Begin(spriteBlendMode);


            foreach (Particle p in particles)
            {
                // skip inactive particles
                if (!p.Active)
                    continue;


                float normalizedLifetime = p.TimeSinceStart / p.Lifetime;


                float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
                Color color = new Color(new Vector4(1, 1, 1, alpha));

                // make particles grow as they age. they'll start at 75% of their size,
                // and increase to 100% once they're finished.
                float scale = p.Scale * (.1f + .05f * normalizedLifetime);

                spriteBatch.Draw(texture, p.Position, null, color,
                    p.Rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
