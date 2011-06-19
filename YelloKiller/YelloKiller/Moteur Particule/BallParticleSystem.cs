#region Using Statements
using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace YelloKiller.Moteur_Particule
{

    class BallParticleSystem : ParticleSystem
    {
        Heros heros;
        Carte carte;
        int distance;

        public BallParticleSystem(YellokillerGame game, int howManyEffects, Heros heros, Carte carte)
            : base(game, howManyEffects)
        {
            this.heros = heros;
            this.carte = carte;
            distance = heros.Distance_Hero_Mur(carte);
        }

        protected override void InitializeConstants()
        {
            textureFilename = @"Particules\explosionB";

            maxInitialSpeed = 28 * distance - 16;
            minInitialSpeed = 28 * distance - 16;

            minAcceleration = -20;
            maxAcceleration = -10;

            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = 1f;
            maxScale = 10f;

            minNumParticles = 15;
            maxNumParticles = 100;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            // additive blending is very good at creating fiery effects.
            spriteBlendMode = SpriteBlendMode.Additive;

            DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, Vector2 where, Heros heros)
        {
            base.InitializeParticle(p, where, heros);

            p.Acceleration = -p.Velocity / p.Lifetime;
        }

        public int LongueurBall
        {
            get { return distance; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (heros != null)
            {
                distance = heros.Distance_Hero_Mur(carte);
                maxInitialSpeed = 50 * distance;
                minInitialSpeed = 50 * distance;
            }

        }

    }
}
