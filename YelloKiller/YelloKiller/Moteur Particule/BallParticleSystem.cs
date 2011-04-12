#region Using Statements
using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace ParticleSample
{

    class BallParticleSystem : ParticleSystem
    {
        int distance;

        public BallParticleSystem(YellokillerGame game, int howManyEffects, SpriteBatch spriteBatch)
            : base(game, howManyEffects, spriteBatch)
        {
        }

        protected override void InitializeConstants()
        {

            textureFilename = "explosion";

            minInitialSpeed = 500;
            maxInitialSpeed = 500;


            minAcceleration = -20;
            maxAcceleration = -10;


            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = 1f;
            maxScale = 10f;

            minNumParticles = 30;
            maxNumParticles = 100;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            // additive blending is very good at creating fiery effects.
            spriteBlendMode = SpriteBlendMode.Additive;

            DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, Vector2 where, Hero hero)
        {
            base.InitializeParticle(p, where, hero);

            p.Acceleration = -p.Velocity / p.Lifetime;
        }

        public Hero Hero // solution trouv� pour pas pass� hero et Carte en parametre dans update override
        { get; set; }
        public Carte Carte
        { get; set; }

        public int LongueurHadoken
        {
            get { return distance; }
        }

        public float Maxvitesse
        { get { return maxInitialSpeed; } }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Hero != null)
            {
                distance = Hero.Distance_Hero_Mur(Carte);
                maxInitialSpeed = 500;
            }

        }

    }
}
