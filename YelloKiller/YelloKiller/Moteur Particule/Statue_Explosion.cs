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
        int distance_statue_mur;

        public Statue_Explosion(YellokillerGame game, int howManyEffects, Carte carte, Statue statue, int distance)
            : base(game, howManyEffects)
        {
            Carte = carte;
            this.distance_statue_mur = distance;
        }

        protected override void InitializeConstants()
        {

            textureFilename = "explosion";

            minInitialSpeed = 50;
            maxInitialSpeed = 50 * distance_statue_mur;

            minAcceleration = -20;
            maxAcceleration = -10;

            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = 1f;
            maxScale = 10f;

            minNumParticles = 100;
            maxNumParticles = 200;

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
            get { return distance_statue_mur; }
        }
    }
}

