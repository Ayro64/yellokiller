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
        int distance = 3 ;

        public BallParticleSystem(YellokillerGame game, int howManyEffects)
            : base(game, howManyEffects)
        {
            
        }

        protected override void InitializeConstants()
        {
            textureFilename = "explosion";

            maxInitialSpeed = 50 * distance;
            minInitialSpeed = 50 * distance;

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

        public Hero Hero // solution trouvé pour pas passé hero et Carte en parametre dans update override
        { get; set; }
        public Statue statue// solution trouvé pour pas passé hero et Carte en parametre dans update override
        { get; set; }
        public Carte Carte
        { get; set; }

        public int LongueurBall
        {
            get { return distance; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Hero != null)
            {
                distance = Hero.Distance_Hero_Mur(Carte);
                maxInitialSpeed = 50 * distance;
                minInitialSpeed = 50 * distance;
            }

        }

    }
}
