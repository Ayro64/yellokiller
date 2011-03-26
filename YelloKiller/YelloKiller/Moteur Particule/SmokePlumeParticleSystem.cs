#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YelloKiller;
using YelloKiller.Moteur_Particule;
#endregion

namespace ParticleSample
{
   
    public class SmokePlumeParticleSystem : ParticleSystem
    {
        public SmokePlumeParticleSystem(YellokillerGame game, int howManyEffects, SpriteBatch spriteBatch)
            : base(game,howManyEffects, spriteBatch )
        {
        }

      
        protected override void InitializeConstants()
        {
            textureFilename = "smoke";

            minInitialSpeed = 20;
            maxInitialSpeed = 100;

            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 5.0f;
            maxLifetime = 7.0f;

            minScale = 0.5f;
            maxScale = 1f;

            minNumParticles = 100;
            maxNumParticles =200;

            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            maxRotationSpeed = MathHelper.PiOver4 / 2.0f;

            spriteBlendMode = SpriteBlendMode.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }

      
        protected override Vector2 PickRandomDirection(Hero hero)
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


        protected override void InitializeParticle(Particle p, Vector2 where, Hero hero)
        {
            base.InitializeParticle(p, where, hero);

            // the base is mostly good, but we want to simulate a little bit of wind
            // heading to the right.
            p.Acceleration.X += MoteurParticule.RandomBetween(10, 50);
        }
    }
}
