#region Using Statements
using YelloKiller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace ParticleSample
{
   
     class ExplosionSmokeParticleSystem : ParticleSystem
    {
        public ExplosionSmokeParticleSystem(YellokillerGame game, int howManyEffects)
            : base(game, howManyEffects)
        {            
        }

       
        protected override void InitializeConstants()
        {
            textureFilename = "smoke";

            // less initial speed than the explosion itself
            minInitialSpeed = 10;
            maxInitialSpeed = 20;

            minAcceleration = -20;
            maxAcceleration = -10;

            // explosion smoke lasts for longer than the explosion itself, but not
            // as long as the plumes do.
            minLifetime = 1.0f;
            maxLifetime = 2f;

            minScale = 1.0f;
            maxScale = 3.0f;

            minNumParticles = 10;
            maxNumParticles = 20;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            spriteBlendMode = SpriteBlendMode.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }
    }
}
