#region File Description
//-----------------------------------------------------------------------------
// ExplosionParticleSystem.cs
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
#endregion

namespace ParticleSample
{
   
    public class ExplosionParticleSystem : ParticleSystem
    {
        public ExplosionParticleSystem(YellokillerGame game, int howManyEffects, SpriteBatch spriteBatch)
            : base(game, howManyEffects, spriteBatch)
        {
        }

       
        protected override void InitializeConstants()
        {
            textureFilename = "explosion";

            minInitialSpeed = 40;
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

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
            
             p.Acceleration = -p.Velocity / p.Lifetime;
        }
    }
}
