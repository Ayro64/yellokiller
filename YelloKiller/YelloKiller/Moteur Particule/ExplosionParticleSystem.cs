#region Using Statements
using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace YelloKiller.Moteur_Particule
{

    class ExplosionParticleSystem : ParticleSystem
    {
        Heros heros;
        Carte carte;
        float maxspeed;

        int distance;

        public ExplosionParticleSystem(YellokillerGame game, int howManyEffects, Heros heros, Carte carte, int maxspeed)
            : base(game, howManyEffects)
        {
            this.maxspeed = maxspeed;

            this.heros = heros;
            this.carte = carte;
            distance = heros.Distance_Hero_Mur(carte);
        }

        protected override void InitializeConstants()
        {
            textureFilename = @"Particules\explosionR";

            minInitialSpeed = 40;
            maxInitialSpeed = 28 * distance - 16;

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

        public int LongueurHadoken
        {
            get { return distance; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (heros != null)
            {
                distance = heros.Distance_Hero_Mur(carte);
                maxInitialSpeed = maxspeed * distance;
            }
        }
    }
}
