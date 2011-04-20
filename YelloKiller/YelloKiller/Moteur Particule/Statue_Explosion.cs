using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace YelloKiller.Moteur_Particule
{
    class Statue_Explosion : ParticleSystem
    {

        Carte Carte;
        Statue Statue;
 
        public float maxspeed
        { get; set; }
        public float minspeed
        { get; set; }


        int distance = 6;

        public Statue_Explosion(YellokillerGame game, int howManyEffects, Carte carte, Statue statue)
            : base(game, howManyEffects)
        {
            this.Statue = statue;
            this.Carte = carte;
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

            minNumParticles = 50;
            maxNumParticles = 100;

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

        public int LongueurExplosion
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

