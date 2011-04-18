using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller.Moteur_Particule
{
    class Statue_Explosion : ParticleSystem
    {
   
        public Carte Carte
        { get; set; }
        public Statue Statue
        { get; set; }
        public float maxspeed
        { get; set; }
        public float minspeed
        { get; set; }


        int distance = 6;

        public Statue_Explosion(YellokillerGame game, int howManyEffects)
            : base(game, howManyEffects)
        {

        }

        protected override void InitializeConstants()
        {
            
            textureFilename = "explosion";

            minInitialSpeed = 50 * distance;
            maxInitialSpeed = 50 * distance;

            minAcceleration = -20;
            maxAcceleration = -10;

            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = 1f;
            maxScale = 10f;

            minNumParticles = 0;
            maxNumParticles = 500;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            // additive blending is very good at creating fiery effects.
            spriteBlendMode = SpriteBlendMode.Additive;

            DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, Vector2 where, Statue statue)
        {

            base.InitializeParticle(p, where, statue);

            p.Acceleration = -p.Velocity / p.Lifetime;
        }

        public int LongueurHadoken
        {
            get { return distance; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Statue != null)
            {
                distance = Statue.Distance_Statue_Mur(Carte);
                maxInitialSpeed = maxspeed * distance;
                minInitialSpeed = minspeed * distance;
            }

        }

    }
}

