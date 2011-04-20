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

        Hero hero; 
        Carte carte;
        public float maxspeed
        { get; set; }
        public float minspeed
        { get; set; }

        int distance = 3;

        public ExplosionParticleSystem(YellokillerGame game, int howManyEffects, Hero hero, Carte carte)
            : base(game, howManyEffects)
        {
            this.hero = hero;
            this.carte = carte;
        }

        protected override void InitializeConstants()
        {
            
            textureFilename = "explosion";

            minInitialSpeed = 40;
            maxInitialSpeed = 50 * distance;

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

        protected override void InitializeParticle(Particle p, Vector2 where, Hero hero)
        {
            base.InitializeParticle(p, where, hero);

            p.Acceleration = -p.Velocity / p.Lifetime;
        }

        public int LongueurHadoken
        {
            get { return distance; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hero != null)
            {
                distance = hero.Distance_Hero_Mur(carte);
                maxInitialSpeed = maxspeed * distance;
                minInitialSpeed = minspeed;
            }

        }

    }
}
