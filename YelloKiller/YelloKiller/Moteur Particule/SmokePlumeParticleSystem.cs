#region Using Statements
using System;
using YelloKiller;
using Microsoft.Xna.Framework;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace YelloKiller.Moteur_Particule
{
   
     class SmokePlumeParticleSystem : ParticleSystem
    {
        public SmokePlumeParticleSystem(YellokillerGame game, int howManyEffects)
            : base(game,howManyEffects)
        {
        }

      
        protected override void InitializeConstants()
        {
            textureFilename = @"Particules\smoke";

            minInitialSpeed = 50;
            maxInitialSpeed = 500;

            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 5.0f;
            maxLifetime = 7.0f;

            minScale = 5f;
            maxScale = 10.0f;

            minNumParticles = 7;
            maxNumParticles = 15;

            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotationSpeed = MathHelper.PiOver4 / 2.0f;

            spriteBlendMode = SpriteBlendMode.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;

            spriteBlendMode = SpriteBlendMode.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }

      
        protected override Vector2 PickRandomDirection()
        {
            // Point the particles somewhere between 80 and 100 degrees.
            // tweak this to make the smoke have more or less spread.
            float radians = MoteurParticule.RandomBetween(
                MathHelper.ToRadians(80), MathHelper.ToRadians(100));

            Vector2 direction = Vector2.Zero;
           
            direction.X = (float)Math.Cos(radians);
            direction.Y = -(float)Math.Sin(radians);
            return direction;
        }


        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            // the base is mostly good, but we want to simulate a little bit of wind
            // heading to the right.
            p.Acceleration.X += MoteurParticule.RandomBetween(10, 50);
        }
    }
}
