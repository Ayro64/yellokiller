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
        private Texture2D texture;
        private Vector2 origin;
        private int howManyEffects;

        Particle[] particles;
        List<Particle> freeParticles; // liste ou je stock mes particules

        public int FreeParticleCount
        {
            get { return freeParticles.Count; }
        }

        #region constants to be set by subclasses


        protected int minNumParticles;
        protected int maxNumParticles; // on le reutilise dans moteur particule


        protected string textureFilename;


        protected float minInitialSpeed;
        protected float maxInitialSpeed;

        protected float minAcceleration;
        protected float maxAcceleration;

        protected float minRotationSpeed;
        protected float maxRotationSpeed;

        protected float minLifetime;
        protected float maxLifetime;

        protected float minScale;
        protected float maxScale;


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
            freeParticles = new List<Particle>(howManyEffects * maxNumParticles);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
                freeParticles.Add(particles[i]);
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


        public void AddParticles(Vector2 where, Heros heros) // surcharge 
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                MoteurParticule.Random.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles[0];
                freeParticles.RemoveAt(0);
                InitializeParticle(p, where, heros);
            }
        }

        public void AddParticles(Vector2 where, Statue statue) // surcharge 
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                MoteurParticule.Random.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles[0];
                freeParticles.RemoveAt(0);
                InitializeParticle(p, where, statue);
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
                Particle p = freeParticles[0];
                freeParticles.RemoveAt(0);
                InitializeParticle(p, where);
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where, Heros heros) // surcharge
        {
            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            Vector2 direction;

            if (heros.Regarde_Haut)
                direction = new Vector2(0, -1);
            else if (heros.Regarde_Bas)
                direction = new Vector2(0, 1);
            else if (heros.Regarde_Gauche)
                direction = new Vector2(-1, 0);
            else
                direction = new Vector2(1, 0);

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

        protected virtual void InitializeParticle(Particle p, Vector2 where, Statue statue) // surcharge
        {
            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            Vector2 direction;
            if (statue.SourceRectangle.Value.Y == 357)
                direction = new Vector2(0, -1);
            else if (statue.SourceRectangle.Value.Y == 0)
                direction = new Vector2(0, 1);
            else if (statue.SourceRectangle.Value.Y == 123)
                direction = new Vector2(-1, 0);
            else
                direction = new Vector2(1, 0);


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

            Vector2 direction;
            float angle = MoteurParticule.RandomBetween(0, MathHelper.TwoPi);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

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
                        freeParticles.Add(p);
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
